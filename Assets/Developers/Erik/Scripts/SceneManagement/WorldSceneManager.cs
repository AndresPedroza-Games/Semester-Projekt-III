using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WorldSceneManager : MonoBehaviour {

	public static WorldSceneManager Instance { get; private set; }

	private readonly HashSet<string> _loadedScenes = new();


	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;

		CacheLoadedScenes();
	}


	private void CacheLoadedScenes() {
		_loadedScenes.Clear();

		for (int i = 0; i < SceneManager.sceneCount; i++) {
			Scene scene = SceneManager.GetSceneAt(i);

			if (scene.isLoaded) {
				_loadedScenes.Add(scene.name);
			}
		}
	}


	private bool IsSceneLoaded(string scene) {
		return _loadedScenes.Contains(scene);
	}


	public async Task LoadScene(string scene) {
		if (IsSceneLoaded(scene)) {
			Debug.Log($"Scene {scene} already loaded.");
			return;
		}

		AsyncOperation operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

		while (operation is { isDone: false }) {
			await Task.Yield();
		}

		_loadedScenes.Add(scene);

		Debug.Log($"Loaded scene: {scene}");
	}


	public async Task UnloadScene(string scene) {
		if (!IsSceneLoaded(scene)) {
			Debug.Log($"Scene {scene} not loaded.");
			return;
		}

		AsyncOperation operation = SceneManager.UnloadSceneAsync(scene);

		while (operation is { isDone: false }) {
			await Task.Yield();
		}

		_loadedScenes.Remove(scene);

		Debug.Log($"Unloaded scene: {scene}");
	}

}