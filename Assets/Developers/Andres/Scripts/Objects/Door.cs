using UnityEngine;


public class Door : MonoBehaviour, IInteractable {

	[SerializeField] private Transform doorHinge;
	[SerializeField] private Key requiredKey;

	private bool canInteract = true;


	// private void Start() {
	// 	// EventSystem.eventSystem.openDoor += Open;
	// 	// EventSystem.eventSystem.keyPicked += () => _DoorIsOpenAble = true;
	// }


	// set canInteract bool via Event e.g. OnKeyPickedUp -> canInteract = true
	public bool CanInteract(Interactor interactor) {
		return canInteract && interactor.CurrentHeldObject != null;
	}


	public void Interact(Interactor interactor) {
		if (interactor.CurrentHeldObject == null) return;

		Open(interactor);
	}


	
	// If canInteract gets set via Event, the door can just be opened without key check
	private void Open(Interactor interactor) {
		if (interactor.CurrentHeldObject.GetGameObject().TryGetComponent(out Key key)) {
			if (key == requiredKey) {
				key.UseKey();
				doorHinge.rotation = new Quaternion(0f, 0f, 0f, 0f);

				canInteract = false;

				interactor.CurrentHeldObject = null;
			}
		}
	}


	// I think Events should handle closing the doors
	public void Close() {
		doorHinge.rotation = Quaternion.Euler(0f, 90f, 0f);
	}

}