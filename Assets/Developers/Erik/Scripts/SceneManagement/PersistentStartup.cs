using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class PersistentStartup : MonoBehaviour {


	private async void Start() {
#if UNITY_EDITOR
		string sceneName = EditorPrefs.GetString("Bootstrap_StartScene", "");

		if (!string.IsNullOrEmpty(sceneName)) {

			if (sceneName == "MainMenu" || sceneName == "TestBuildMainMenu") {
				await WorldSceneManager.Instance.LoadScene(BuildSettingsLoader.StartupScene);
				return;
			}

			GameObject player = SearchForPlayer();

			await WorldSceneManager.Instance.LoadScene(sceneName);

			GameObject playerSpawn = SearchForPlayerSpawn(sceneName);
			Debug.Log(sceneName);

			if (playerSpawn && player) {
				player.transform.position = playerSpawn.transform.position;
			}

			EventSystemController.Instance.StartGame();

		}
		else {
			await WorldSceneManager.Instance.LoadScene(BuildSettingsLoader.StartupScene);
		}
#else
		await WorldSceneManager.Instance.LoadScene(BuildSettingsLoader.StartupScene);
#endif
	}


	public static GameObject SearchForPlayer() {
		GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();

		foreach (GameObject root in roots) {
			if (root.name == "Player") {
				GameObject player = root;
				return player;
			}
		}

		Debug.LogWarning("No Player found!");
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


	public static GameObject SearchForPlayerSpawn(SceneReference scene) {
		GameObject[] roots = SceneManager.GetSceneByPath(scene).GetRootGameObjects();

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