using UnityEngine;


public class Picker : MonoBehaviour {

	[Header("---Hold/Drop Point---")]
	[SerializeField] private Transform holdPoint;
	[SerializeField] private Transform dropPoint;
	
	[Tooltip("Drop Offset is used when the drop position is located within a different object")]
	[SerializeField] [Range(0.1f, 0.5f)] float dropOffset = 0.1f;
	
	private Vector3 altDropPoint;

	private Pickable currentPickedObj;
	private Interactor interactor;


	private void Awake() {
		interactor = GetComponent<Interactor>();
	}


	public void PickUp(Pickable pickedObj) {
		currentPickedObj = pickedObj;

		pickedObj.Rb.isKinematic = true;
		pickedObj.Col.enabled = false;

		pickedObj.transform.SetParent(holdPoint);
		pickedObj.transform.position = holdPoint.position;
	}


	public void Drop() {
		Ray ray = new(interactor.cam.transform.position, interactor.cam.transform.forward);
		float distance = (ray.origin - dropPoint.position).magnitude;

		if (Physics.Raycast(ray, out RaycastHit hit, distance)) {
			Vector3 offsetDir = (ray.origin - hit.point).normalized;
			altDropPoint = hit.point + offsetDir * dropOffset;

			SetPosAndParent(altDropPoint);
		}
		else {
			SetPosAndParent(dropPoint.position);
		}
	}


	private void SetPosAndParent(Vector3 dropPos) {
		currentPickedObj.transform.position = dropPos;
		currentPickedObj.transform.SetParent(null);

		currentPickedObj.Rb.isKinematic = false;
		currentPickedObj.Col.enabled = true;

		currentPickedObj = null;
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

		if (interactor != null) {
			Ray ray = new(interactor.cam.transform.position, interactor.cam.transform.forward);
			float distance = (ray.origin - dropPoint.position).magnitude;
			if (Physics.Raycast(ray, out RaycastHit hit, distance)) {
				Vector3 offsetDir = (ray.origin - hit.point).normalized;
				altDropPoint = hit.point + offsetDir * dropOffset;

				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(altDropPoint, 0.1f);
			}
		}
	}

}