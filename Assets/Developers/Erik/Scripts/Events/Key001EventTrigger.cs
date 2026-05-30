using UnityEngine;


public class Key001EventTrigger : MonoBehaviour {

	private bool _alreadyExecuted;


	private void OnEnable() {
		EventSystemController.eventSystemController.onItemPicked += TriggerEvent002;
	}


	private void OnDisable() {
		EventSystemController.eventSystemController.onItemPicked -= TriggerEvent002;
	}


	private void TriggerEvent002(GameObject obj) {
		if (_alreadyExecuted) return;

		if (obj == this.gameObject) {
			EventSystemController.eventSystemController.Key001PickedUp();
			_alreadyExecuted = true;
		}
	}

}