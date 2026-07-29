using System.Collections;
using UnityEngine;


public class Key001EventTrigger : MonoBehaviour {

	[Header("---Event Delay---")]
	[SerializeField] private float eventDelay;

	private Coroutine _delayRoutine;

	private bool _alreadyExecuted;


	private void OnEnable() {
		EventSystemController.Instance.onItemPicked += TriggerEvent002;
	}


	private void OnDisable() {
		EventSystemController.Instance.onItemPicked -= TriggerEvent002;
	}


	private void TriggerEvent002(GameObject obj) {
		if (_alreadyExecuted) return;

		if (obj == this.gameObject) {
			_delayRoutine ??= StartCoroutine(EventDelayRoutine());
		}
	}


	private IEnumerator EventDelayRoutine() {
		_alreadyExecuted = true;
		yield return new WaitForSeconds(eventDelay);
		Hall0EventManager.Instance.Key001PickedUp();
		_delayRoutine = null;
	}


}