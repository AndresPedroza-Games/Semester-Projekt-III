using UnityEngine;
using UnityEngine.Events;


public class CloseDoorAction : MonoBehaviour {

	[Header("---Trigger Action---")]
	[SerializeField] private UnityEvent onEnterAction;


	private void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		onEnterAction?.Invoke();
		gameObject.SetActive(false);
	}

}