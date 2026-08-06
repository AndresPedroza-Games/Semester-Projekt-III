using DG.Tweening;
using UnityEngine;


public class ProjectorPowerButton : MonoBehaviour, IInteractable, ILeftClickable, ICrosshair {

	[Header("---Animation---")]
	[SerializeField] private Vector3 endPosition;
	[SerializeField] private float animationDuration;

	private bool _isAnimating;
	private bool _state;
	private bool _canInteract = true;

	private ObjectSfx _sfx;


	private void Awake() {
		_sfx = GetComponent<ObjectSfx>();

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
		if (_isAnimating)
			return;

		_state = !_state;

		EventSystemPrincipalsOffice.Instance.ProjectorPowerButtonPressed(_state);

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