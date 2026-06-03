#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class PlayModeBootstrap {

	private const string PersistentScenePath = "Assets/Scenes/Persistent.unity";


	static PlayModeBootstrap() {
		EditorApplication.playModeStateChanged += OnPlayModeChanged;
	}


	private static void OnPlayModeChanged(PlayModeStateChange state) {
		if (state == PlayModeStateChange.ExitingEditMode) {
			var activeScene = SceneManager.GetActiveScene();

			if (activeScene.name == "Persistent") {
				EditorPrefs.SetString("Bootstrap_StartScene", "");
				return;
			}


			EditorPrefs.SetString("Bootstrap_StartScene", activeScene.name);

			var persistentScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(PersistentScenePath);

			EditorSceneManager.playModeStartScene = persistentScene;
		}

		if (state == PlayModeStateChange.EnteredEditMode) {
			EditorSceneManager.playModeStartScene = null;
		}
	}

}
#endif