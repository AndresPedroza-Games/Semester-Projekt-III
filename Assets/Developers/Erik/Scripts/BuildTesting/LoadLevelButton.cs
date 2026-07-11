using UnityEngine;
using UnityEngine.UI;


public class LoadLevelButton : MonoBehaviour {

	[Header("---LEVEL TO LOAD---")]
	public SceneReference sceneToLoad;

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
		await WorldSceneManager.Instance.LoadScene(sceneToLoad);
		if (!_hasSpawn)
			SetPlayerPos(sceneToLoad);

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