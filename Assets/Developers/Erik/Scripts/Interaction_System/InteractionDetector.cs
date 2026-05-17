using UnityEngine;


public class InteractionDetector : MonoBehaviour {

	[Header(("---Interaction Distance---"))]
	[SerializeField] private float interactionDistance = 2.0f;

	private Camera cam;

	public IInteractable CurrentTarget { get; private set; }


	private void Awake() {
		cam = Camera.main;
	}


	private void Update() {
		Detect();
	}


	private void Detect() {
		CurrentTarget = null;

		Ray ray = new Ray(cam.transform.position, cam.transform.forward);

		if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
			return;

		if (!hit.collider.TryGetComponent(out IInteractable interactable))
			return;

		CurrentTarget = interactable;
	}
	
	private void OnDrawGizmos() {
		if (cam == null) {
			cam = Camera.main;
		}
	
		Debug.DrawRay(cam.transform.position, cam.transform.forward * interactionDistance, Color.blue);
	}

}