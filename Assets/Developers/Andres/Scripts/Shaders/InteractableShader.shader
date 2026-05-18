Shader "Custom/InteractableShader"
{
    Properties
    {
        
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [HDR] _ReflectionColor("Reflection Color", Color) = (1,1,1,1)
        _AmbientColor("Ambient Color", Color) = (1,1,1,1)


        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}

        _BorderThickness("Border Thickness", Range(0,0.05)) = 0.01

        _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalStrength("Normal Strength", Float) = 1
        _SpecularInt("Specular Intensity", Range(0.0,1.0)) = 0.5
        _SpecularPow("Specular Power", Float) = 64
        _FresnelInt("Fresnel Intensity", Range(0.0,1.0)) = 0.5
        _FresnelPow("Fresnel Power", Float) = 5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                float4 tangent : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD2;
                float3 viewDir : TEXCOORD3;  
                float4 tangentWS: TEXCOORD4;
                float3 binormalWS : TEXCOORD5;    
                float3 posWS :TEXCOORD6; 
                float3 objectPos : TEXCOORD7;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _ReflectionColor;
                half4 _AmbientColor;
                float4 _BaseMap_ST;
                float _BorderThickness;
                float _SpecularInt;
                float _SpecularPow;
                float _FresnelInt;
                float _FresnelPow;
                float _NormalStrength;
            CBUFFER_END
            
            half4 SetBorderColor(half4 color, float2 uv)
            {
                float left = smoothstep(0.0, _BorderThickness, uv.x);
                float right = smoothstep(0.0, _BorderThickness, 1.0 - uv.x);
                float top = smoothstep(0.0, _BorderThickness, uv.y);
                float bottom = smoothstep(0.0, _BorderThickness, 1.0 - uv.y);

                float interior = left * right * top * bottom;

                color = lerp(_ReflectionColor, color, interior);

                return color;
            }

            float Lambert(float3 normal, float3 lightDir)
            {
                return saturate(dot(normal,lightDir));
            }

            float Phong(float3 normal, float3 lightDir,float3 viewDir, float power)
            {
               float3 halfV = normalize(lightDir + viewDir);
                return pow(saturate(dot(normal,halfV)), power);
            }

            float Fresnel(float3 normal, float3 viewDir, float power)
            {
                return pow(1 - saturate(dot(normal,viewDir)),power);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

                OUT.normalWS = normalize(mul(unity_ObjectToWorld, float4(IN.normalOS.xyz,0)));
                OUT.viewDir = GetWorldSpaceViewDir(mul(unity_ObjectToWorld,IN.positionOS).xyz);

                float3 posWS = mul(unity_ObjectToWorld, IN.positionOS).xyz;
                OUT.posWS = posWS;

                OUT.tangentWS = normalize(mul(IN.tangent, unity_WorldToObject));
                OUT.binormalWS = normalize(cross(OUT.normalWS.xyz,OUT.tangentWS.xyz)*IN.tangent.w);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;

                color = SetBorderColor(color, IN.uv);

                float3 normalMap = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, IN.uv));
                float3x3 TBN_matrix = float3x3
                (
                    IN.tangentWS.xyz,
                    IN.binormalWS,
                    IN.normalWS
                );

                float3 normal = normalize(mul(normalMap, TBN_matrix));
                normal = normalize(lerp(IN.normalWS, normal, _NormalStrength));

                Light light = GetMainLight(TransformWorldToShadowCoord(IN.posWS));
                float3 lightDir = normalize(light.direction);

                float3 diffuse = color.xyz;
                float3 radiance = light.color * Lambert(normal, lightDir) * light.shadowAttenuation;
                float specular = _SpecularInt * Phong(normal,lightDir,normalize(IN.viewDir),_SpecularPow);
                float fresnel = _FresnelInt * Fresnel(normal,normalize(IN.viewDir),_FresnelPow);

                float3 ambientColor = color.xyz * _AmbientColor.xyz;

                color.xyz = (diffuse + specular) * radiance + ambientColor + fresnel;

                return color;
            }

            ENDHLSL
        }

                Pass
        {   
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            HLSLPROGRAM

            #pragma vertex ShadowVert
            #pragma fragment ShadowFrag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            float3 _LightDirection;
            float3 _LightPosition;

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
            };

            float4 GetShadowPositionHClip(Attributes input)
            {
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

                normalWS *= dot(normalWS, _LightDirection) > 0 ? 1 : -1;

                #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                    float3 lightDirectionWS = normalize(_LightPosition - positionWS);
                #else
                    float3 lightDirectionWS = _LightDirection;
                #endif

                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
                positionCS = ApplyShadowClamping(positionCS);
                return positionCS;
            }

            Varyings ShadowVert(Attributes input)
            {
                Varyings output;

                output.positionCS = GetShadowPositionHClip(input);
                return output;
            }

            half4 ShadowFrag(Varyings input) : SV_TARGET
            {          
                return 0;
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }

            HLSLPROGRAM

            #pragma vertex DepthVert
            #pragma fragment DepthFrag
           
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"

            struct Attributes
            {
                float4 position     : POSITION;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
            };

            Varyings DepthVert(Attributes input)
            {
                Varyings output;

                output.positionCS = TransformObjectToHClip(input.position.xyz);
                return output;
            }

            half4 DepthFrag(Varyings input) : SV_TARGET
            {
                return input.positionCS.z;
            }
            ENDHLSL
        }
    }
}
