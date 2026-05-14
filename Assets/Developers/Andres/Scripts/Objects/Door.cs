using UnityEngine;


public class Door : MonoBehaviour, IInteractable {

	[SerializeField] private Transform doorHinge;
	[SerializeField] private Key requiredKey;

	private bool canInteract = true;


	// private void Start() {
	// 	// EventSystem.eventSystem.openDoor += Open;
	// 	// EventSystem.eventSystem.keyPicked += () => _DoorIsOpenAble = true;
	// }


	public bool CanInteract(Interactor interactor) {
		return canInteract && interactor.CurrentPickedObj != null;
	}


	public void Interact(Interactor interactor) {
		if (interactor.CurrentPickedObj == null) return;

		Open(interactor);
	}


	private void Open(Interactor interactor) {
		if (interactor.CurrentPickedObj.GetGameObject().TryGetComponent(out Key key)) {
			if (key == requiredKey) {
				key.UseKey();
				doorHinge.rotation = new Quaternion(0f, 0f, 0f, 0f);

				canInteract = false;

				interactor.CurrentPickedObj = null;
			}
		}
	}


	// I think Events should handle closing the doors
	public void Close() {
		doorHinge.rotation = Quaternion.Euler(0f, 90f, 0f);
	}

}