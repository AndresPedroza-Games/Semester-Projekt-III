using System;
using UnityEngine;


public class ProjectorPowerButton : MonoBehaviour, IInteractable, ILeftClickable, ICrosshair {

	private bool _state;
	private bool _canInteract = true;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");
	}


	private void OnEnable() {
		EventSystemPrincipalsOffice.Instance.onPuzzleSolved += SetBoolOnSolved;
	}


	private void OnDisable() {
		EventSystemPrincipalsOffice.Instance.onPuzzleSolved -= SetBoolOnSolved;
	}


	private void SetBoolOnSolved(bool state) {
		_canInteract = false;
	}


	public bool CanInteract(HoldController holdController) {
		return false;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return _canInteract ? CrosshairType.HandPointer : CrosshairType.Default;
	}


	public void Interact() {
		_state = !_state;

		EventSystemPrincipalsOffice.Instance.ProjectorPowerButtonPressed(_state);
	}


	public bool CanInteractWithLeftClick(HoldController holdController) {
		return !holdController.HasObject && _canInteract;
	}


	public void OnLeftClick() {
		Interact();
	}

}