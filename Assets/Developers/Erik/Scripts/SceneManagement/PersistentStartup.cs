using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
#endif


public class PersistentStartup : MonoBehaviour {

	private async void Start() {
#if UNITY_EDITOR
		string sceneName = EditorPrefs.GetString("Bootstrap_StartScene", "");

		if (!string.IsNullOrEmpty(sceneName)) {

			GameObject player = SearchForPlayer();

			await WorldSceneManager.Instance.LoadScene(sceneName);

			if (sceneName != "MainMenu") {
				GameObject playerSpawn = SearchForPlayerSpawn(sceneName);

				if (playerSpawn && player) {
					player.transform.position = playerSpawn.transform.position;
				}

				EventSystemController.Instance.StartGame();
			}
		}
		else {
			await WorldSceneManager.Instance.LoadScene("MainMenu");
		}
#else
			await WorldSceneManager.Instance.LoadScene("MainMenu");
#endif
	}


	private GameObject SearchForPlayer() {
		GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();

		foreach (GameObject root in roots) {
			if (root.name == "Player") {
				GameObject player = root;
				return player;
			}
		}

		return null;
	}


	private GameObject SearchForPlayerSpawn(string sceneName) {
		GameObject[] roots = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();

		foreach (GameObject root in roots) {
			PlayerSpawn playerSpawn = root.GetComponentInChildren<PlayerSpawn>(true);
			if (playerSpawn) {
				return playerSpawn.gameObject;
			}
		}

		Debug.LogWarning("No PlayerSpawn found!");
		return null;
	}

}