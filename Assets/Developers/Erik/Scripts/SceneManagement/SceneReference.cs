using System;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

// Credits: JohannesMP (2018-08-12)


[Serializable]
public class SceneReference : ISerializationCallbackReceiver {

#if UNITY_EDITOR
	[SerializeField] private Object sceneAsset = null;
	bool IsValidSceneAsset {
		get {
			if (sceneAsset == null)
				return false;
			return sceneAsset.GetType().Equals(typeof(SceneAsset));
		}
	}
#endif

	[SerializeField]
	private string scenePath = string.Empty;

	public string ScenePath {
		get {
#if UNITY_EDITOR
			return GetScenePathFromAsset();
#else
            return scenePath;
#endif
		}
		set {
			scenePath = value;
#if UNITY_EDITOR
			sceneAsset = GetSceneAssetFromPath();
#endif
		}
	}


	public static implicit operator string(SceneReference sceneReference) {
		return sceneReference.ScenePath;
	}


	public void OnBeforeSerialize() {
#if UNITY_EDITOR
		HandleBeforeSerialize();
#endif
	}


	public void OnAfterDeserialize() {
#if UNITY_EDITOR
		EditorApplication.update += HandleAfterDeserialize;
#endif
	}


#if UNITY_EDITOR
	private SceneAsset GetSceneAssetFromPath() {
		if (string.IsNullOrEmpty(scenePath))
			return null;
		return AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
	}


	private string GetScenePathFromAsset() {
		if (sceneAsset == null)
			return string.Empty;
		return AssetDatabase.GetAssetPath(sceneAsset);
	}


	private void HandleBeforeSerialize() {
		if (IsValidSceneAsset == false && string.IsNullOrEmpty(scenePath) == false) {
			sceneAsset = GetSceneAssetFromPath();
			if (sceneAsset == null)
				scenePath = string.Empty;

			EditorSceneManager.MarkAllScenesDirty();
		}
		else {
			scenePath = GetScenePathFromAsset();
		}
	}


	private void HandleAfterDeserialize() {
		EditorApplication.update -= HandleAfterDeserialize;
		if (IsValidSceneAsset)
			return;

		if (string.IsNullOrEmpty(scenePath) == false) {
			sceneAsset = GetSceneAssetFromPath();
			if (sceneAsset == null)
				scenePath = string.Empty;

			if (Application.isPlaying == false)
				EditorSceneManager.MarkAllScenesDirty();
		}
	}
#endif

}