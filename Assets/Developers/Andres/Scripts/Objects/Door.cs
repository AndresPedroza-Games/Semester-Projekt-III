using UnityEngine;


public class Door : MonoBehaviour, IInteractable {

	[SerializeField] private Transform doorHinge;
	[SerializeField] private Key requiredKey;

	private bool canOpen = true; // should be false at beginning

	private EventSystemController eventSystemController;

    private void Start()
    {
		eventSystemController = EventSystemController.eventSystemController;
		eventSystemController.onCloseDoor += CloseDoor;
    }


	// set canInteract bool via Event e.g. OnKeyPickedUp -> canOpen = true
	public bool CanInteract(HoldController holdController) {
		return canOpen && holdController.HasObject;
	}


	public void Interact() {
		if (canOpen)
			OpenDoor();
		//else
		//PlayLockedDoorSound...

	}


	// If canInteract gets set via Event, the door can just be opened without key check
	private void OpenDoor() {
		doorHinge.rotation = new Quaternion(0f, 0f, 0f, 0f);

		canOpen = false;
	}


// I think Events should handle closing the doors
	public void CloseDoor() {
		doorHinge.rotation = Quaternion.Euler(0f, 90f, 0f);
		Debug.Log("Door Closed");
	}
}