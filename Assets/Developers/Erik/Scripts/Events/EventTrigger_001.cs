using UnityEngine;


public class EventTrigger001 : MonoBehaviour {

	private void Awake() {
		Collider col = GetComponent<BoxCollider>();

		col.includeLayers = LayerMask.GetMask("Player");
	}


	private void OnTriggerEnter(Collider other) {
		EventSystemController.Instance.Event001();

		gameObject.SetActive(false);
	}

}