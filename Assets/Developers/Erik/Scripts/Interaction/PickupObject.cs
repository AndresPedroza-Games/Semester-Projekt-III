using UnityEngine;


public class PickupObject : MonoBehaviour, IInteractable, IPickable {

	private Rigidbody rb;
	private Collider col;


	private void Awake() {
		rb = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
	}


	public void Interact(Interactor interactor) {
		PickUp(interactor.Inventory);
	}


	public void PickUp(Inventory inventory) {
		if (inventory.HoldingObject != null) return;

		inventory.HoldingObject = this.gameObject;

		rb.isKinematic = true;
		col.enabled = false;

		transform.position = inventory.HoldPosition.position;
		transform.SetParent(inventory.HoldPosition.transform);
	}


	public void Drop(Inventory inventory) {
		transform.SetParent(null);
		transform.position = inventory.dropPosition.position;

		rb.isKinematic = false;
		col.enabled = true;
	}

}