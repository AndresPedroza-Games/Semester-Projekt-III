using System.Collections;
using UnityEngine;


public class Event002TurnOffLights : MonoBehaviour {

	[SerializeField] private Material tubelampOffMaterial;
	[SerializeField] private ParticleSystem _Particles;

	[SerializeField] private float _CoolDown;

	private Renderer _renderer;
	private Light _light;

	private bool _CanPlay;


	private void Awake() {
		_renderer = GetComponentInChildren<Renderer>();
		_light = GetComponentInChildren<Light>();
	}

    private void Update()
    {
		if (_CanPlay)
		{
            _CanPlay = false;
            StartCoroutine(RestartParticles());
        }
    }


    private void OnEnable() {
		EventSystemController.Instance.OnKey001PickedUp += TurnOffLight;
	}


	private void OnDisable() {
		EventSystemController.Instance.OnKey001PickedUp -= TurnOffLight;

	}


	private void TurnOffLight() {
		_renderer.material = tubelampOffMaterial;
		_light.enabled = false;
		_Particles.Play();
		_CanPlay = true;
    }

	private IEnumerator RestartParticles()
	{
		yield return new WaitForSeconds(_CoolDown);
        _Particles.Play();
        _CanPlay = true;
    }

}