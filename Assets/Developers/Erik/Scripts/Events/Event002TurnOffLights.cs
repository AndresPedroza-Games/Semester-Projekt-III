using UnityEngine;


public class Event002TurnOffLights : MonoBehaviour {

	[SerializeField] private Material tubelampOffMaterial;

	private Renderer _renderer;
	private Light _light;


	private void Awake() {
		_renderer = GetComponentInChildren<Renderer>();
		_light = GetComponentInChildren<Light>();
	}


	private void OnEnable() {
		EventSystemController.eventSystemController.OnKey001PickedUp += TurnOffLight;
	}


	private void OnDisable() {
		EventSystemController.eventSystemController.OnKey001PickedUp -= TurnOffLight;

	}


	private void TurnOffLight() {
		_renderer.material = tubelampOffMaterial;
		_light.enabled = false;
	}

}