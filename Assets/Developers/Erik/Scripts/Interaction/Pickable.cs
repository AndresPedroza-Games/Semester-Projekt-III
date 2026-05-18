using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Pickable : MonoBehaviour, IInteractable, IPickable {

	public Rigidbody Rb { get; private set; }
	public Collider Col { get; private set; }

	private Picker picker;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");

		Rb = GetComponent<Rigidbody>();
		Col = GetComponent<Collider>();

		Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}


	public GameObject GetGameObject() {
		return this.gameObject;
	}
	
	public bool CanInteract(Interactor interactor) {
		return interactor.CurrentPickedObj == null;
	}


	public void Interact(Interactor interactor) {
		picker = interactor.Picker;
		PickUp();

		interactor.CurrentPickedObj = this;
	}


	public void PickUp() {
		picker.PickUp(this);
		EventSystemController.eventSystemController.PickItem();
	}


	public void Drop() {
		picker?.Drop();
		picker = null;
        EventSystemController.eventSystemController.DropItem();
    }

}