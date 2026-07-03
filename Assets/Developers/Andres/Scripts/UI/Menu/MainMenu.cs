using UnityEngine;


public class MainMenu : MenuManager {

	[SerializeField] private SceneReference sceneToLoadOnStart;


	private void Start() {
		InputManager.Instance.Pause.Disable();
		
		if (GetScene(0)) {
			_StartBtn.onClick.AddListener(StartGame);
			_ExitBtn.onClick.AddListener(ExitGame);
		}
	}


	public override async void StartGame() {
		await WorldSceneManager.Instance.LoadScene(sceneToLoadOnStart);
		await WorldSceneManager.Instance.UnloadScene(BuildSettingsLoader.StartupScene);

		EventSystemController.Instance.StartGame();
	}


	public override void ExitGame() {
		Application.Quit();
		Debug.Log("Exit");
	}

}