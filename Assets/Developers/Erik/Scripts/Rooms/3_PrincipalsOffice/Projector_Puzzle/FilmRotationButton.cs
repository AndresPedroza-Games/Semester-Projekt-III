using UnityEngine;


public class FilmRotationButton : MonoBehaviour, IInteractable, ILeftClickable {

	private bool _canInteract;


	private void OnEnable() {
		EventSystemPrincipalsOffice.Instance.onPuzzleSolved += OnPuzzleSolved;
		EventSystemPrincipalsOffice.Instance.onProjectorPowerButtonPressed += OnPowerButtonPressed;
	}


	private void OnDisable() {
		EventSystemPrincipalsOffice.Instance.onPuzzleSolved -= OnPuzzleSolved;
		EventSystemPrincipalsOffice.Instance.onProjectorPowerButtonPressed -= OnPowerButtonPressed;
	}


	private void OnPuzzleSolved(bool state) {
		_canInteract = false;
	}


	private void OnPowerButtonPressed(bool isOn) {
		_canInteract = isOn;
	}


	public bool CanInteract(HoldController holdController) {
		return false;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return _canInteract ? CrosshairType.RotateCw : CrosshairType.Default;
	}


	public void Interact() {
		EventSystemPrincipalsOffice.Instance.FilmRotationButtonPressed();
	}


	public bool CanInteractWithLeftClick(HoldController holdController) {
		return _canInteract;
	}


	public void OnLeftClick() {
		Interact();
	}

}