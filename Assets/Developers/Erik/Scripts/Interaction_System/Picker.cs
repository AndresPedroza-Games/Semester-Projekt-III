using UnityEngine;


public class Picker : MonoBehaviour {

	[Header("---Hold/Drop Point---")]
	[SerializeField] private Transform holdPoint;
	[SerializeField] private Transform dropPoint;

	[Tooltip("Drop Offset is used when the drop position is located within a different object")]
	[SerializeField] [Range(0.1f, 0.5f)] float dropOffset = 0.1f;
	[SerializeField] [Range(0.1f, 0.5f)] float defaultOffset = 0.1f;

	private Camera cam;


	private void Awake() {
		cam = Camera.main;
	}


	public void Attach(Holdable holdable) {
		holdable.gameObject.layer = LayerMask.NameToLayer("SocketHold");
		for (int i = 0; i < holdable.transform.childCount; i++) {
			holdable.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("SocketHold");
		}


		holdable.Rigidbody.interpolation = RigidbodyInterpolation.None;

		holdable.transform.SetParent(holdPoint);

		holdable.Rigidbody.isKinematic = true;
		holdable.Rigidbody.useGravity = false;
		holdable.Collider.enabled = false;

		holdable.transform.localPosition = Vector3.zero;
		holdable.transform.localRotation = Quaternion.identity;
	}


	public void Detach(Holdable holdable) {
		holdable.gameObject.layer = LayerMask.NameToLayer("Interactable");
		for (int i = 0; i < holdable.transform.childCount; i++) {
			holdable.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Interactable");
		}

		Ray ray = new(cam.transform.position, cam.transform.forward);
		float distance = (ray.origin - dropPoint.position).magnitude;

		Vector3 targetPos;

		if (Physics.Raycast(ray, out RaycastHit hit, distance)) {
			Vector3 offsetDir = (ray.origin - hit.point).normalized;
			targetPos = hit.point + offsetDir * dropOffset;
		}
		else {
			targetPos = dropPoint.position - cam.transform.forward.normalized * defaultOffset;
		}

		SetPosAndParent(targetPos, holdable);
	}


	private void SetPosAndParent(Vector3 dropPos, Holdable holdable) {

		holdable.transform.SetParent(holdable.StartParentIfSocketHold);
		holdable.transform.eulerAngles = new Vector3(0f, holdPoint.eulerAngles.y, Random.Range(-90f, 90f));


		holdable.Rigidbody.isKinematic = false;
		holdable.Rigidbody.useGravity = true;
		holdable.Collider.enabled = true;

		holdable.transform.position = dropPos;

	}


	private void OnDrawGizmos() {
		if (holdPoint != null) {
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(holdPoint.position, 0.1f);
		}

		if (dropPoint != null) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(dropPoint.position, 0.1f);
		}

		if (!cam)
			cam = Camera.main;

		if (cam && dropPoint) {

			Ray ray = new(cam.transform.position, cam.transform.forward);
			float distance = (ray.origin - dropPoint.position).magnitude;
			Vector3 targetPos;
			if (Physics.Raycast(ray, out RaycastHit hit, distance)) {
				Vector3 offsetDir = (ray.origin - hit.point).normalized;
				targetPos = hit.point + offsetDir * dropOffset;

				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(targetPos, 0.1f);
			}
		}
	}

}