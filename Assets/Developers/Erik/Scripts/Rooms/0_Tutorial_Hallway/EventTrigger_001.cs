using UnityEngine;


public class EventTrigger001 : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			Hall0EventManager.Instance.Event001();

			gameObject.SetActive(false);
		}
	}

}