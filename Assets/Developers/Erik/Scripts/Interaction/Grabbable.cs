using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Grabbable : MonoBehaviour, IInteractable {

	private Rigidbody rb;
	private Grabber grabber;


	private void Awake() {
		rb = GetComponent<Rigidbody>();

		rb.interpolation = RigidbodyInterpolation.Interpolate;
		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}


	public void Interact(Interactor interactor) {
		grabber = interactor.Grabber;

		if (grabber.IsHolding)
			return;
		else
			grabber.Grab(rb);
	}


	void OnJointBreak(float force) {
		Debug.Log("Joint broken with force: " + force);

		grabber?.Drop();
		grabber = null;
	}

}