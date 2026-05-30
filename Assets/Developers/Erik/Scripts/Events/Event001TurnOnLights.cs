using UnityEngine;


public class Event001TurnOnLights : MonoBehaviour {

	[SerializeField] private Material tubelampOnMaterial;

	private Renderer _renderer;
	private Light _light;


	private void Awake() {
		_renderer = GetComponentInChildren<Renderer>();
		_light = GetComponentInChildren<Light>();

		_light.enabled = false;
	}


	private void OnEnable() {
		EventSystemController.eventSystemController.onEvent001 += TurnOnLight;
	}


	private void OnDisable() {
		EventSystemController.eventSystemController.onEvent001 -= TurnOnLight;

	}


	private void TurnOnLight() {
		_renderer.material = tubelampOnMaterial;
		_light.enabled = true;
	}

}