using UnityEngine;


public class ProjectorPowerButton : MonoBehaviour, IInteractable {

	private bool _state;
	private bool _canInteract = true;


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
		return !holdController.HasObject && _canInteract;
	}


	public void Interact() {
		_state = !_state;

		EventSystemPrincipalsOffice.Instance.ProjectorPowerButtonPressed(_state);
	}

}