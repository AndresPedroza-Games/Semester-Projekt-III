using System.Collections.Generic;
using UnityEngine;


public class LightSwitch : MonoBehaviour, IInteractable, ICrosshair {

	[Header("---Light---")]
	[SerializeField] private List<Light> lightSources;
	[SerializeField] private bool enableLightOnAwake = true;

	private bool _lightIsEnabled;
	private ObjectSfx _sfx;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");
		_sfx = GetComponent<ObjectSfx>();

		_lightIsEnabled = enableLightOnAwake;

		if (!enableLightOnAwake)
			RotateSwitch();

		if (enableLightOnAwake) {
			foreach (Light lightSource in lightSources) {
				lightSource.enabled = true;
			}

			_lightIsEnabled = true;
		}
		else {
			foreach (Light lightSource in lightSources) {
				lightSource.enabled = false;
			}

			_lightIsEnabled = false;
		}
	}


	public void Interact() {
		RotateSwitch();
		_sfx?.PlaySfx(SfxEvent.OnInteract);
		_lightIsEnabled = !_lightIsEnabled;

		foreach (Light lightSource in lightSources) {
			lightSource.enabled = _lightIsEnabled;
		}
	}


	private void RotateSwitch() {
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, (transform.eulerAngles.z + 180 + 360) % 360);
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return CrosshairType.Interactable;
	}

}