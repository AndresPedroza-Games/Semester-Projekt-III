using DG.Tweening;
using UnityEngine;


public class ToiletLid : MonoBehaviour, IInteractable, ICrosshair {

	[Header("Animation Settings")]
	[SerializeField] private Ease ease;
	[SerializeField] private float duration;
	[SerializeField] private float openedRotation;


	private float _angleToRotateTo;
	private bool _shouldOpen;
	private bool _canInteract = true;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");
	}


	public void Interact() {
		if (!_canInteract)
			return;
		
		OpenCloseLid();
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject && _canInteract;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return _canInteract ? CrosshairType.Interactable : CrosshairType.Default;
	}


	private void OpenCloseLid() {
		_shouldOpen = !_shouldOpen;
		_canInteract = false;

		_angleToRotateTo = _shouldOpen ? openedRotation : 0;

		AnimateVisuals();
	}


	private void AnimateVisuals() {
		transform.DOLocalRotateQuaternion(Quaternion.Euler(_angleToRotateTo, 0f, 0f), duration).SetEase(ease).SetLink(gameObject)
			.OnComplete(() => _canInteract = true);
	}

}