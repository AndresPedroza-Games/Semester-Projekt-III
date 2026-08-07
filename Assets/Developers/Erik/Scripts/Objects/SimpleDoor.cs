using DG.Tweening;
using UnityEngine;


public class SimpleDoor : MonoBehaviour, IInteractable, ICrosshair {

	[Header("Animation Settings")]
	[SerializeField] private Vector3 openedRotation;
	[SerializeField] private Vector3 closedRotation;
	[SerializeField] private Ease ease;
	[SerializeField] private float duration = 1f;

	private Collider _collider;
	private Tween _rotationTween;
	private bool _isOpen;
	private bool _canOpen = true;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");
		_collider = GetComponent<Collider>();
	}


	private void OpenDoor() {
		Rotate(ease, duration, openedRotation);

		_canOpen = false;
		_collider.enabled = false;
		_isOpen = true;
	}


	private void CloseDoor() {
		Rotate(ease, duration, closedRotation);

		_canOpen = false;
		_collider.enabled = false;
		_isOpen = false;
	}


	private void Rotate(Ease e, float d, Vector3 targetRotation) {
		_rotationTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(targetRotation), d).SetEase(e).SetLink(gameObject);

		_rotationTween.OnComplete(() => {
			_collider.enabled = true;
			_canOpen = true;
		});
	}


	public void Interact() {
		if (!_canOpen)
			return;

		if (!_isOpen)
			OpenDoor();
		else
			CloseDoor();
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return holdController.HasObject ? CrosshairType.Default : CrosshairType.Interactable;
	}

}