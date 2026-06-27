using DG.Tweening;
using UnityEngine;


public class Door : MonoBehaviour, IInteractable {

	[Header("Animation Settings")]
	[SerializeField] private Ease _Ease;
	[SerializeField] private float _Duration = 1f;
	private float _Angle;

	[SerializeField] private Transform doorHinge;
	[SerializeField] private GameObject requiredKey;

	public bool canOpen = false;
	public bool isLocked;

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
            canOpen = true;
	}


	private void SetCanOpenFalse(GameObject obj) {
        canOpen = false;
	}


	private void UseKey(GameObject obj) {
		if (obj == requiredKey) {
			obj.GetComponent<IUseable>().UseItem();
		}
	}


	public bool CanInteract(HoldController holdController) {
		return canOpen;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return canOpen ? CrosshairType.Interactable : CrosshairType.Default;
	}


	public void Interact() {

        if (EventSystemBathroom.instance != null && ShowerValve._IsCompleted)
		{
            isLocked = true;
			canOpen = false;
			EventSystemBathroom.instance.LockDoor();
            Debug.Log("Locked");
        }

        if (canOpen && !isLocked)
			OpenDoor();
		//else
		//PlayLockedDoorSound...

    }


	private void OpenDoor() {
		eventSystemController.OpenDoor(requiredKey);

		Rotate(_Ease, _Duration, -90);

        canOpen = false;

		GetComponent<BoxCollider>().enabled = false;

		Debug.Log("Door Opened");
	}


	public void CloseDoor() {
        canOpen = false;
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