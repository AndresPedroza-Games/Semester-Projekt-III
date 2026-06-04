using UnityEngine;


public class EventTrigger001 : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			EventSystemController.Instance.Event001();

			gameObject.SetActive(false);
		}
	}

}