using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class PersistentStartup : MonoBehaviour {

	private async void Start() {
#if UNITY_EDITOR
		string sceneName = EditorPrefs.GetString("Bootstrap_StartScene", "");

		if (!string.IsNullOrEmpty(sceneName)) {
			await WorldSceneManager.Instance.LoadScene(sceneName);
			if (sceneName != "MainMenu")
				EventSystemController.Instance.StartGame();
		}
		else {
			await WorldSceneManager.Instance.LoadScene("MainMenu");
		}
#else
			await WorldSceneManager.Instance.LoadScene("MainMenu");
#endif
	}

}