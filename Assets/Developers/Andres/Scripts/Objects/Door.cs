using UnityEngine;


public class Door : MonoBehaviour, IInteractable {

	[SerializeField] private Transform doorHinge;
	[SerializeField] private GameObject requiredKey;


	// private void Start() {
	// 	// EventSystem.eventSystem.openDoor += Open;
	// 	// EventSystem.eventSystem.keyPicked += () => _DoorIsOpenAble = true;
	// }


	public void Open(GameObject obj) {
		// if (_DoorIsOpenAble)
		//     Debug.Log("Opening Door");
		// else
		//     Debug.Log("You don't have the key");

		doorHinge.rotation = new Quaternion(0f, 0f, 0f, 0f);
		gameObject.layer = 0;
	}


	// I think Events should handle closing the doors
	public void Close() {
		doorHinge.rotation = Quaternion.Euler(0f, 90f, 0f);
	}


	public void Interact(Interactor interactor) {
		if (interactor.Inventory.HoldingObject != null) {
			GameObject obj = interactor.Inventory.HoldingObject;

			if (obj == requiredKey) {
				Open(obj);
				obj.transform.SetParent(null);
				obj.SetActive(false);
				interactor.Inventory.HoldingObject = null;
			}
		}
	}

}