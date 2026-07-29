using UnityEngine;


public class EventTrigger : MonoBehaviour {

	[Header("---Game Event---")]
	[SerializeField] private GameEvent gameEvent;


	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			gameEvent.Raise();
			gameObject.SetActive(false);
		}
	}

}