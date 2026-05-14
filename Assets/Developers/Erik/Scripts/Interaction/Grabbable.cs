using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Grabbable : MonoBehaviour, IInteractable, IPickable {

	private Rigidbody rb;
	private Grabber grabber;
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


	public bool CanInteract(Interactor i) {
		return i.CurrentPickedObj == null;
	}


	public void Interact(Interactor i) {
		interactor = i;
		grabber = interactor.Grabber;
		PickUp();

		interactor.CurrentPickedObj = this;
	}


	public void PickUp() {
		grabber.Grab(rb);
	}


	public void Drop() {
		grabber?.Drop();
		grabber = null;

		if (interactor != null) interactor.CurrentPickedObj = null; 
	}


	void OnJointBreak(float force) {
		Debug.Log("Joint broken with force: " + force);

		Drop();
		grabber = null;
	}

}