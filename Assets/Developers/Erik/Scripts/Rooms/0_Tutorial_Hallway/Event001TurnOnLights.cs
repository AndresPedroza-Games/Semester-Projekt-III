using UnityEngine;


public class Event001TurnOnLights : MonoBehaviour {

	private Light _light;
	private AudioSource _audioSource;


	private void Awake() {
		_light = GetComponentInChildren<Light>();
		_audioSource = GetComponent<AudioSource>();

		_light.enabled = false;
	}


	private void OnEnable() {
		EventSystemController.Instance.onEvent001 += TurnOnLight;
	}


	private void OnDisable() {
		EventSystemController.Instance.onEvent001 -= TurnOnLight;

	}


	private void TurnOnLight() {
		_light.enabled = true;
		_audioSource.Play();
	}

}