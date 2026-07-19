using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InteractionDetector : MonoBehaviour {

	[Header(("---Interaction Distance---"))]
	public float interactionDistance = 2.0f;

	private Camera cam;
	private LayerMask layermask;

	public IInteractable CurrentTarget { get; private set; }

    private Vector3 _LastPositionMouse;
	private GraphicRaycaster _GraphicRaycaster;

    private void Awake() {
		cam = Camera.main;

		gameObject.layer = LayerMask.NameToLayer("Player");
		layermask = ~LayerMask.GetMask("Player");

		_GraphicRaycaster = FindFirstObjectByType<GraphicRaycaster>(FindObjectsInactive.Include);

    }

    private void Update() {
		Detect();
    }


	private void Detect() {
		CurrentTarget = null;

		Ray ray = new Ray(cam.transform.position, cam.transform.forward);

		if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance, layermask, QueryTriggerInteraction.Ignore))
			return;

		if (!hit.collider.TryGetComponent(out IInteractable interactable))
			return;

		CurrentTarget = interactable;
    }

    public Vector3 GetRayPosition(LayerMask layerDetector)
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance, layermask))
            return new Vector3(0,0,0);

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