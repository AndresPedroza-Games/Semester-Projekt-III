using System.Collections;
using UnityEngine;


public class Key001Event : MonoBehaviour {

	[Header("---Event---")]
	[SerializeField] private GameEvent gameEvent;
	[SerializeField] private float eventDelay;
	
	private Coroutine _delayRoutine;

	private bool _alreadyExecuted;


	private void OnEnable() {
		EventSystemController.Instance.onItemPicked += OnItemPicked;
	}
	
	
	private void OnDisable() {
		EventSystemController.Instance.onItemPicked -= OnItemPicked;
	}


	private void OnItemPicked(GameObject obj) {
		if (_alreadyExecuted) return;

		if (obj == this.gameObject) {
			_delayRoutine ??= StartCoroutine(EventDelayRoutine());
		}
	}


	private IEnumerator EventDelayRoutine() {
		_alreadyExecuted = true;
		yield return new WaitForSeconds(eventDelay);
		gameEvent.Raise();
		_delayRoutine = null;
	}


}