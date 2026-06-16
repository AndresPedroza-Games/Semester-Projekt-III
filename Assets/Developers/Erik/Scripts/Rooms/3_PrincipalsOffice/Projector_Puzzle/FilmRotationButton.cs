using UnityEngine;


public class FilmRotationButton : MonoBehaviour, IInteractable {

	private bool _canInteract = true;


	private void OnEnable() {
		EventSystemPrincipalsOffice.Instance.onPuzzleSolved += SetBool;
		EventSystemPrincipalsOffice.Instance.onProjectorPowerButtonPressed += SetBoolOnPowerButton;
	}


	private void OnDisable() {
		EventSystemPrincipalsOffice.Instance.onPuzzleSolved -= SetBool;
		EventSystemPrincipalsOffice.Instance.onProjectorPowerButtonPressed -= SetBoolOnPowerButton;
	}


	private void SetBool(bool state) {
		_canInteract = false;
	}
	
	private void SetBoolOnPowerButton(bool isOn) {
		_canInteract = isOn;
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject && _canInteract;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return CrosshairType.Interactable;
	}


	public void Interact() {
		EventSystemPrincipalsOffice.Instance.FilmRotationButtonPressed();
	}

}