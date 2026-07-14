using UnityEngine;


public class MainMenu : MenuManager {

	[SerializeField] private SceneReference sceneToLoadOnStart;
	private bool _hasSpawn;


	private void Start() {
		InputManager.Instance.Pause.Disable();

		if (GetScene(0)) {
			_StartBtn.onClick.AddListener(StartGame);
			_ExitBtn.onClick.AddListener(ExitGame);
		}
	}


	public override async void StartGame() {
		//If lastScene != null
		// Load lastScene
		// SetPlayerPos(lastScene)
		//else
		await WorldSceneManager.Instance.LoadScene(sceneToLoadOnStart);
		if (!_hasSpawn)
			SetPlayerPos(sceneToLoadOnStart);

		await WorldSceneManager.Instance.UnloadScene(BuildSettingsLoader.StartupScene);

		EventSystemController.Instance.StartGame();
	}


	private void SetPlayerPos(SceneReference scene) {
		GameObject spawn = PersistentStartup.SearchForPlayerSpawn(scene);

		if (!spawn) return;

		GameManager.Instance.Player.transform.position = spawn.transform.position;

		_hasSpawn = true;
	}


	public override void ExitGame() {
		Application.Quit();
		Debug.Log("Exit");
	}

}