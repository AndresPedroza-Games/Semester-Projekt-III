using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Grabbable : MonoBehaviour, IInteractable, IPickable {

	private Rigidbody rb;
	public Grabber grabber;
	private Interactor interactor;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");
		
		rb = GetComponent<Rigidbody>();

		rb.interpolation = RigidbodyInterpolation.Interpolate;
		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}


	public GameObject GetGameObject() {
		return this.gameObject;
	}


	public void Interact(Interactor i) {
		
	}


	public bool CanInteract(Interactor i) {
		return i.CurrentHeldObject == null;
	}


	public void PickUp(Interactor i) {
		interactor = i;
		grabber = interactor.Grabber;
		
		grabber.Grab(rb);
		interactor.CurrentHeldObject = this;
	}


	public void Drop() {
		grabber?.Drop();
		grabber = null;

		if (interactor) interactor.CurrentHeldObject = null; 
	}


	public void Break() {
		grabber = null;

		if (interactor) interactor.CurrentHeldObject = null;
	}


	// void OnJointBreak(float force) {
	// 	Debug.Log("Joint broken with force: " + force);
	//
	// 	Drop();
	// 	grabber = null;
	// }

}