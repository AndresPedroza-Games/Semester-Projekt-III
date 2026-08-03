using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class MenuSettings : MenuManager {

	[Header("---Mouse---")]
	[SerializeField] private float mouseSensitivityMinValue = 0.01f;
	[SerializeField] private float mouseSensitivityMaxValue = 1f;
	[SerializeField] private AudioSlider mouseSensitivitySlider;
	private CinemachinePOV _cinePov;
	
    [Header("---Audio---")]
    [SerializeField] private AudioMixer _AudioMixer;
    [SerializeField] private List<AudioSlider> _AudioSliders;

    [Header("---Fullscreen---")]
    [SerializeField] private Toggle fullscreenToggle;

    private const string FirstLaunchKey = "FirstLaunchDone";
    
    
    private void Awake() {
	    if (!PlayerPrefs.HasKey(FirstLaunchKey)) {

		    SetDefaultVolume();
		    
		    PlayerPrefs.SetInt(FirstLaunchKey, 1);
		    PlayerPrefs.Save();
	    }
    }


    private void Start()
    {
        foreach (AudioSlider audioSlider in _AudioSliders)
        {
	        if (PlayerPrefs.HasKey(audioSlider.name))
	        {
		        float volume = PlayerPrefs.GetFloat(audioSlider.name);
		        audioSlider.slider.value = volume;
		        
		        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
		        _AudioMixer.SetFloat(audioSlider.name, dB);

		        audioSlider.slider.value = volume;
		        UpdateVolumeToText(audioSlider, volume);
	        } 
	        else if (_AudioMixer.GetFloat(audioSlider.name, out float dbValue))
	        {
		        float volume = Mathf.Pow(10f, dbValue / 20f);
		        audioSlider.slider.value = volume;
		        UpdateVolumeToText(audioSlider, volume);
	        }

            audioSlider.slider.onValueChanged.AddListener((volume) => SetVolume(audioSlider, volume));
            audioSlider.inputField.onValueChanged.AddListener((name) => OnInputFieldChanged(audioSlider, name));
            audioSlider.inputField.onEndEdit.AddListener((name) => OnInputFieldConfirmed(audioSlider, name));
        }

        if (fullscreenToggle) {
		    fullscreenToggle.isOn = Screen.fullScreen;
	        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }
        
        SetupMouseSensitivitySlider();
    }
    
    private void OnDisable()
    {
	    PlayerPrefs.Save();
    }
    
    private void OnDestroy()
    {
	    if (fullscreenToggle)
		    fullscreenToggle.onValueChanged.RemoveListener(SetFullscreen);
    }


    private void SetDefaultVolume() {
	    foreach (AudioSlider audioSlider in _AudioSliders)
	    {
		    float defaultVolume = 0.3f;

		    float dB = Mathf.Log10(Mathf.Clamp(defaultVolume, 0.0001f, 1f)) * 20f;
		    _AudioMixer.SetFloat(audioSlider.name, dB);
		    PlayerPrefs.SetFloat(audioSlider.name, defaultVolume);
	    }
    }
    

    private void SetVolume(AudioSlider audioSlider,float volume)
    {
	    float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
	    
	    _AudioMixer.SetFloat(audioSlider.name, dB);
	    PlayerPrefs.SetFloat(audioSlider.name, volume);
	    
        UpdateVolumeToText(audioSlider, volume);
    }

    private void UpdateVolumeToText(AudioSlider audioSlider, float linearVolume)
    {
        float percent = Mathf.InverseLerp(audioSlider.slider.minValue, audioSlider.slider.maxValue, linearVolume);
        int volumeInPercent = Mathf.RoundToInt(percent * 100f);
        audioSlider.inputField.SetTextWithoutNotify($"{volumeInPercent} %");
    }

    private void OnInputFieldChanged(AudioSlider audioSlider, string name)
    {
        string cleanedInput = new string(name.Where(char.IsDigit).ToArray());

        if (cleanedInput.Length > 3)
            cleanedInput = cleanedInput.Substring(0, 3);

        audioSlider.inputField.text = cleanedInput;
    }

    private void OnInputFieldConfirmed(AudioSlider audioSlider, string name)
    {
        if (int.TryParse(name, out int percent))
        {
            percent = Mathf.Clamp(percent, 0, 100);
            float normalized = percent / 100f;
            float linearValue = Mathf.Lerp(audioSlider.slider.minValue, audioSlider.slider.maxValue, normalized);

            audioSlider.slider.value = linearValue;
            SetVolume(audioSlider, linearValue);
        }
        else
        {
            if (_AudioMixer.GetFloat(audioSlider.name, out float dB))
            {
                float linear = Mathf.Pow(10f, dB / 20f);
                UpdateVolumeToText(audioSlider, linear);

            }
        }
    }


    private void SetupMouseSensitivitySlider() {
	    _cinePov = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();

	    mouseSensitivitySlider.slider.minValue = mouseSensitivityMinValue;
	    mouseSensitivitySlider.slider.maxValue = mouseSensitivityMaxValue;
        
	    mouseSensitivitySlider.slider.value = _cinePov.m_HorizontalAxis.m_MaxSpeed;
	    UpdateMouseSensitivityText(_cinePov.m_HorizontalAxis.m_MaxSpeed);
        
	    mouseSensitivitySlider.slider.onValueChanged.AddListener(SetMouseSensitivity);
	    mouseSensitivitySlider.inputField.onEndEdit.AddListener(OnMouseSensitivityConfirmed);
    }


    private void SetMouseSensitivity(float value) {
	    _cinePov.m_HorizontalAxis.m_MaxSpeed = value;
	    _cinePov.m_VerticalAxis.m_MaxSpeed = value;
	    
	    UpdateMouseSensitivityText(value);
    }
    
    
    private void UpdateMouseSensitivityText(float value)
    {
	    mouseSensitivitySlider.inputField.text = value.ToString("0.00");
    }
    
    
    private void OnMouseSensitivityConfirmed(string text)
    {
	    if (float.TryParse(text, out float value))
	    {
		    value = Mathf.Clamp(value, mouseSensitivitySlider.slider.minValue, mouseSensitivitySlider.slider.maxValue);

		    mouseSensitivitySlider.slider.value = value;
		    SetMouseSensitivity(value);
	    }
	    else
	    {
		    UpdateMouseSensitivityText(_cinePov.m_HorizontalAxis.m_MaxSpeed);
	    }
    }


    private void SetFullscreen(bool isFullscreen) {
	    Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
}
