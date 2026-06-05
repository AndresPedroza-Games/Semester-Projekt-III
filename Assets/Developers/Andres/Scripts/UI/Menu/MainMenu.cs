using UnityEngine;


public class MainMenu : MenuManager {

	[SerializeField] private SceneField sceneToLoadOnStart;


	private void Start() {
		if (GetScene(0)) {
			_StartBtn.onClick.AddListener(StartGame);
			_ExitBtn.onClick.AddListener(ExitGame);
		}
	}


	public override async void StartGame() {
		await WorldSceneManager.Instance.LoadScene(sceneToLoadOnStart);

		await WorldSceneManager.Instance.UnloadScene("MainMenu");

		EventSystemController.Instance.StartGame();
	}


	public override void ExitGame() {
		Debug.Log("Exit");
	}

}