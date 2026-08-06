using System.Collections;
using DG.Tweening;
using UnityEngine;


public class FilmRotationButton : MonoBehaviour, IInteractable, ILeftClickable, ICrosshair {

	[Header("---PuzzleProjector---")]
	[SerializeField] private PuzzleProjector puzzleProjector;
	private Coroutine _waitRoutine;

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
		return _canInteract ? CrosshairType.RotateCw : CrosshairType.Default;
	}


	public void Interact() {
		if (_isAnimating)
			return;

		EventSystemPrincipalsOffice.Instance.FilmRotationButtonPressed();

		_sfx?.PlaySfx(SfxEvent.OnInteract);
		AnimateVisuals();
		_waitRoutine ??= StartCoroutine(WaitForAnimationEnd());
	}


	public bool CanInteractWithLeftClick(HoldController holdController) {
		return _canInteract;
	}


	public void OnLeftClick() {
		Interact();
	}


	private void AnimateVisuals() {
		_isAnimating = true;
		transform.DOLocalMove(endPosition, animationDuration).SetLoops(2, LoopType.Yoyo);
	}


	private IEnumerator WaitForAnimationEnd() {
		yield return new WaitForSeconds(puzzleProjector.filmRotationDuration);
		_isAnimating = false;
		_waitRoutine = null;
	}

}