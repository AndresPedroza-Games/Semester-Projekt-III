using Cinemachine;
using UnityEngine;


public class MainMenu : MenuManager {

	[SerializeField] private SceneReference sceneToLoadOnStart;
	private CinemachinePOV _cinePovComp;


	private void Start() {
		_cinePovComp = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();

		InputManager.Instance.Pause.Disable();

		if (GetScene(0)) {
			_StartBtn.onClick.AddListener(StartGame);
			_ExitBtn.onClick.AddListener(ExitGame);
		}
	}


	public override async void StartGame() {
		CheckAndLoadScene();

		await WorldSceneManager.Instance.UnloadScene(BuildSettingsLoader.StartupScene);

		EventSystemController.Instance.StartGame();
	}


	private async void CheckAndLoadScene() {
		string lastScene = DataManager.LastScene;

		if (!string.IsNullOrEmpty(lastScene)) {
			await WorldSceneManager.Instance.LoadScene(lastScene);
			
			Door door = PersistentStartup.GetDoor(lastScene);
			door.CloseDoor();
			
			GameObject spawn = PersistentStartup.SearchForPlayerSpawn(lastScene);
			if (!spawn) return;
			SetPlayerPosAndRot(spawn.transform);
		}
		else {
			await WorldSceneManager.Instance.LoadScene(sceneToLoadOnStart);
			GameObject spawn = PersistentStartup.SearchForPlayerSpawn(sceneToLoadOnStart);
			if (!spawn) return;
			SetPlayerPosAndRot(spawn.transform);
		}
	}


	private void SetPlayerPosAndRot(Transform spawn) {
		GameManager.Instance.Player.transform.position = spawn.position;
		_cinePovComp.m_HorizontalAxis.Value = spawn.eulerAngles.y;
		_cinePovComp.m_VerticalAxis.Value = 0f;
	}


	private Door GetDoor() {
		return FindFirstObjectByType<Door>();
	}


	public override void ExitGame() {
		Application.Quit();
		Debug.Log("Exit");
	}

}