using UnityEngine;
using UnityEngine.Serialization;


public class Picker : MonoBehaviour {

	[FormerlySerializedAs("holdPosition")]
	[Header("---Hold/Drop Point---")]
	[SerializeField] private Transform holdPoint;
	[SerializeField] private Transform dropPoint;

	private Pickable currentPickedObj;


	public void PickUp(Pickable pickedObj) {
		currentPickedObj = pickedObj;

		pickedObj.Rb.isKinematic = true;
		pickedObj.Col.enabled = false;

		pickedObj.transform.SetParent(holdPoint);
		pickedObj.transform.position = holdPoint.position;
	}


	public void Drop() {
		currentPickedObj.transform.SetParent(null);
		currentPickedObj.transform.position = dropPoint.position;

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
	}

}