using System.Collections.Generic;
using UnityEngine;


public class LoadSceneTrigger : MonoBehaviour {

	[SerializeField] private List<SceneField> scenesToLoad;
	[SerializeField] private List<SceneField> scenesToUnload;


	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			LoadUnloadRooms();
			gameObject.SetActive(false);
		}
	}


	private async void LoadUnloadRooms() {
		if (scenesToLoad.Count > 0) {
			for (int i = 0; i < scenesToLoad.Count; i++) {
				await WorldSceneManager.Instance.LoadScene(scenesToLoad[i]);
			}
		}

		if (scenesToUnload.Count > 0) {
			for (int i = 0; i < scenesToUnload.Count; i++) {
				await WorldSceneManager.Instance.UnloadScene(scenesToUnload[i]);
			}
		}
	}

}