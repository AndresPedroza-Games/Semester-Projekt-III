using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadLevelButton : MonoBehaviour {

	[Header("---LEVEL TO LOAD---")]
	public List<SceneReference> scenesToLoad;

	private bool _hasSpawn;
	private Button _btn;


	private void Awake() {
		_btn = GetComponent<Button>();
	}


	private void OnEnable() {
		_btn.onClick.AddListener(Loadlevel);
	}


	private void OnDisable() {
		_btn.onClick.RemoveListener(Loadlevel);
	}


	private async void Loadlevel() {
		foreach (SceneReference scene in scenesToLoad) {
			await WorldSceneManager.Instance.LoadScene(scene);
			if (!_hasSpawn)
				SetPlayerPos(scene);
		}

		await WorldSceneManager.Instance.UnloadScene(BuildSettingsLoader.StartupScene);

		EventSystemController.Instance.StartGame();
	}


	private void SetPlayerPos(SceneReference scene) {
		GameObject spawn = PersistentStartup.SearchForPlayerSpawn(scene);

		if (!spawn) return;

		GameManager.Instance.Player.transform.position = spawn.transform.position;

		_hasSpawn = true;
	}


}