using UnityEngine;


public class Inventory : MonoBehaviour {

	public Transform HoldPosition => holdPosition;
	public Transform dropPosition;
	public GameObject HoldingObject { get; set; }
	
	public bool HasKey { get; set; }

	[SerializeField] private Transform holdPosition;


	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(holdPosition.position, 0.1f);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(dropPosition.position, 0.1f);
	}

}