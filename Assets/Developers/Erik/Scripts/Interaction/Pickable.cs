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


	public void Interact(Interactor i) {

	}


	public bool CanInteract(Interactor i) {
		return i.CurrentHeldObject == null;
	}


	public GameObject GetGameObject() {
		return this.gameObject;
	}


	public void PickUp(Interactor i) {
		picker = i.Picker;
		picker.PickUp(this);

		i.CurrentHeldObject = this;
	}


	public void Drop() {
		picker?.Drop();
		picker = null;
	}

}