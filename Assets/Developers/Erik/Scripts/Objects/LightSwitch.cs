using System.Collections.Generic;
using UnityEngine;


public class LightSwitch : MonoBehaviour, IInteractable {

	[Header("---Light---")]
	[SerializeField] private List<Light> lightSources;
	[SerializeField] private bool enableOnAwake;

	private bool _lightIsEnabled;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");

		_lightIsEnabled = enableOnAwake;

		if (!enableOnAwake)
			RotateSwitch();
	}


	public void Interact() {
		RotateSwitch();
		_lightIsEnabled = !_lightIsEnabled;

		foreach (Light lightSource in lightSources) {
			lightSource.enabled = _lightIsEnabled;
		}
	}


	private void RotateSwitch() {
		transform.eulerAngles = new Vector3(0, 0, (transform.eulerAngles.z + 180 + 360) % 360);
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return CrosshairType.HandPointer;
	}

}