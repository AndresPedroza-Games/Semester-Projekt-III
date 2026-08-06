using UnityEngine;


public class FilmSelectionButton : MonoBehaviour, IInteractable, ILeftClickable {

	[Header("---Button Config---")]
	[SerializeField] private bool selectUpwards;

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
		if (!_canInteract)
			return CrosshairType.Default;

		return selectUpwards ? CrosshairType.ArrowUp : CrosshairType.ArrowDown;
	}


	public void Interact() {
		EventSystemPrincipalsOffice.Instance.FilmSelectionButtonPressed(selectUpwards);
	}


	public bool CanInteractWithLeftClick(HoldController holdController) {
		return !holdController.HasObject && _canInteract;
	}


	public void OnLeftClick() {
		Interact();	
	}

}