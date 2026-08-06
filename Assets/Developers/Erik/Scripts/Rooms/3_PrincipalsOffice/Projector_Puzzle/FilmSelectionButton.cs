using DG.Tweening;
using UnityEngine;


public class FilmSelectionButton : MonoBehaviour, IInteractable, ILeftClickable, ICrosshair {

	[Header("---Button Config---")]
	[SerializeField] private bool selectUpwards;

	[Header("---Animation---")]
	[SerializeField] private Vector3 endPosition;
	[SerializeField] private float animationDuration;

	private bool _isAnimating;

	private bool _canInteract;
	private ObjectSfx _sfx;


	private void Awake() {
		_sfx = GetComponent<ObjectSfx>();

		gameObject.layer = LayerMask.NameToLayer("Interactable");
	}


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
		if (_isAnimating)
			return;

		EventSystemPrincipalsOffice.Instance.FilmSelectionButtonPressed(selectUpwards);

		_sfx?.PlaySfx(SfxEvent.OnInteract);
		AnimateVisuals();
	}


	public bool CanInteractWithLeftClick(HoldController holdController) {
		return !holdController.HasObject && _canInteract;
	}


	public void OnLeftClick() {
		Interact();
	}


	private void AnimateVisuals() {
		_isAnimating = true;

		transform.DOLocalMove(endPosition, animationDuration).SetLoops(2, LoopType.Yoyo).OnComplete(() => { _isAnimating = false; });
	}

}