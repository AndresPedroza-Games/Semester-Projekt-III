using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WorldSceneManager : MonoBehaviour {

	public static WorldSceneManager Instance { get; private set; }
	public static string persistent = "Persistent";
	public static Action onSceneUnloaded;


	private void Awake() {
		if (Instance && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}


	private bool IsSceneLoadedByPath(string scenePath) {
		Scene scene = SceneManager.GetSceneByPath(scenePath);
		return scene.IsValid() && scene.isLoaded;
	}


	private bool IsSceneLoadedByName(string sceneName) {
		Scene scene = SceneManager.GetSceneByName(sceneName);
		return scene.IsValid() && scene.isLoaded;
	}


	public async Task LoadScene(SceneReference scene) {
		if (IsSceneLoadedByPath(scene)) {
			Debug.Log($"Scene {scene.ScenePath} already loaded.");
			return;
		}

		AsyncOperation operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

		if (operation == null) {
			Debug.LogError($"Could not load scene '{scene.ScenePath}'.");
			return;
		}

		while (operation is { isDone: false }) {
			await Task.Yield();
		}

		Debug.Log($"Loaded scene: {scene.ScenePath}");
	}


	public async Task LoadScene(string sceneName) {
		if (IsSceneLoadedByName(sceneName)) {
			Debug.Log($"Scene {sceneName} already loaded.");
			return;
		}

		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		if (operation == null) {
			Debug.LogError($"Could not load scene '{sceneName}'.");
			return;
		}

		while (operation is { isDone: false }) {
			await Task.Yield();
		}

		Debug.Log($"Loaded scene: {sceneName}");
	}


	public async Task UnloadScene(SceneReference scene) {
		if (!IsSceneLoadedByPath(scene)) {
			Debug.Log($"Scene {scene.ScenePath} not loaded.");
			return;
		}

		AsyncOperation operation = SceneManager.UnloadSceneAsync(scene);

		if (operation == null) {
			Debug.LogError($"Could not load scene '{scene.ScenePath}'.");
			return;
		}

		while (operation is { isDone: false }) {
			await Task.Yield();
		}

		onSceneUnloaded?.Invoke();

		Debug.Log($"Unloaded scene: {scene.ScenePath}");
	}


	public async Task UnloadScene(string sceneName) {
		if (!IsSceneLoadedByName(sceneName)) {
			Debug.Log($"Scene {sceneName} not loaded.");
			return;
		}

		AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);

		if (operation == null) {
			Debug.LogError($"Could not load scene '{sceneName}'.");
			return;
		}

		while (operation is { isDone: false }) {
			await Task.Yield();
		}

		onSceneUnloaded?.Invoke();

		Debug.Log($"Unloaded scene: {sceneName}");
	}


	public async Task UnloadAllExcept(params string[] keepScenes) {
		HashSet<string> keep = new(keepScenes);

		List<string> unloadScenes = new();

		for (int i = 0; i < SceneManager.sceneCount; i++) {
			Scene scene = SceneManager.GetSceneAt(i);

			if (keep.Contains(scene.name))
				continue;

			unloadScenes.Add(scene.name);
		}

		foreach (string sceneName in unloadScenes) {
			await UnloadScene(sceneName);
		}
	}

}