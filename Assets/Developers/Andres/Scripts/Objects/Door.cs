using UnityEngine;


public class Door : MonoBehaviour, IInteractable {

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
		eventSystemController.onItemPicked += SetCanOpenTrue;
		eventSystemController.onItemDropped += SetCanOpenFalse;
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
		return _canOpen ? CrosshairType.Interactable: CrosshairType.Default;
	}


	public void Interact() {
		if (_canOpen)
			OpenDoor();
		//else
		//PlayLockedDoorSound...

	}


	private void OpenDoor() {
		eventSystemController.OpenDoor(requiredKey);

		doorHinge.rotation = new Quaternion(0f, 90f, 0f, 0f);

		_canOpen = false;

		GetComponent<BoxCollider>().enabled = false;

		Debug.Log("Door Opened");
	}


// I think Events should handle closing the doors
	public void CloseDoor() {
		doorHinge.rotation = new Quaternion(0f, 0f, 0f, 0f);
		Debug.Log("Door Closed");
	}

}