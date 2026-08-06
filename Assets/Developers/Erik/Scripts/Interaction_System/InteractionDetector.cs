using UnityEngine;
using UnityEngine.UI;


public class InteractionDetector : MonoBehaviour {

	[Header(("---Interaction Distance---"))]
	public float interactionDistance = 2.0f;

	[Header("---LayerMask---")]
	private Camera cam;
	[SerializeField] private LayerMask layermask;

	public GameObject CurrentTarget { get; private set; }
	public Vector3 HitPoint { get; private set; }

	private Vector3 _LastPositionMouse;
	private GraphicRaycaster _GraphicRaycaster;


	private void Awake() {
		cam = Camera.main;

		gameObject.layer = LayerMask.NameToLayer("Player");

		_GraphicRaycaster = FindFirstObjectByType<GraphicRaycaster>(FindObjectsInactive.Include);

	}


	private void Update() {
		Detect();
	}


	private void Detect() {
		CurrentTarget = null;
		HitPoint = Vector3.zero;

		Ray ray = new Ray(cam.transform.position, cam.transform.forward);

		if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance, layermask, QueryTriggerInteraction.Ignore))
			return;

		CurrentTarget = hit.collider.gameObject;

		HitPoint = hit.point;
	}


	public Vector3 GetRayPosition(LayerMask layerDetector) {
		Ray ray = new Ray(cam.transform.position, cam.transform.forward);

		if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance, layermask))
			return new Vector3(0, 0, 0);

		if (Physics.Raycast(ray, out hit, interactionDistance, layerDetector))
			_LastPositionMouse = hit.point;

		return _LastPositionMouse;
	}


	private void OnDrawGizmos() {

		if (cam == null) {
			cam = Camera.main;
		}

		if (cam)
			Debug.DrawRay(cam.transform.position, cam.transform.forward * interactionDistance, Color.blue);
	}

}