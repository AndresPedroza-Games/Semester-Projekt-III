using DG.Tweening;
using UnityEngine;


public class Door : MonoBehaviour, IInteractable {

	[Header("Animation Settings")]
	[SerializeField] private Ease _Ease;
	[SerializeField] private float _Duration = 1f;
	private float _Angle;

	[SerializeField] private Transform doorHinge;
	[SerializeField] private GameObject requiredKey;

	private bool _canOpen = false;

	private EventSystemController eventSystemController;


	private void Start() {
		eventSystemController = EventSystemController.Instance;
		eventSystemController.onCloseDoor += CloseDoor;
		eventSystemController.onItemPicked += SetCanOpenTrue;
		eventSystemController.onItemDropped += SetCanOpenFalse;
		eventSystemController.onOpenDoor += UseKey;
	}


	private void OnDisable() {
		eventSystemController.onCloseDoor -= CloseDoor;
		eventSystemController.onItemPicked -= SetCanOpenTrue;
		eventSystemController.onItemDropped -= SetCanOpenFalse;
		eventSystemController.onOpenDoor -= UseKey;
	}


	private void SetCanOpenTrue(GameObject obj) {
		if (obj == requiredKey)
			_canOpen = true;
	}


	private void SetCanOpenFalse(GameObject obj) {
		_canOpen = false;
	}


	private void UseKey(GameObject obj) {
		if (obj == requiredKey) {
			obj.GetComponent<Key>().UseKey();
		}
	}


	public bool CanInteract(HoldController holdController) {
		return _canOpen && holdController.HasObject;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return _canOpen ? CrosshairType.Interactable : CrosshairType.Default;
	}


	public void Interact() {
		if (_canOpen)
			OpenDoor();
		//else
		//PlayLockedDoorSound...

	}


	private void OpenDoor() {
		eventSystemController.OpenDoor(requiredKey);

		Rotate(_Ease, _Duration, -90);

		_canOpen = false;

		GetComponent<BoxCollider>().enabled = false;

		Debug.Log("Door Opened");
	}


	public void CloseDoor() {
		Rotate(_Ease, _Duration, 0f);
		Debug.Log("Door Closed");
	}


	public void Rotate(Ease ease, float duration, float angle) {
		_Angle = angle;

		AnimateVisuals(ease, duration);
	}


	private void AnimateVisuals(Ease ease, float duration) {

		doorHinge.DOLocalRotateQuaternion(Quaternion.Euler(0f, _Angle, 0f), duration).SetEase(ease).SetLink(gameObject);

	}

}