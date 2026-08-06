using Cinemachine;
using TMPro;
using UnityEngine;


public class MainMenu : MenuManager {

	[Header("---Scene To Load---")]
	[SerializeField] private SceneReference sceneToLoadOnStart;

	private TMP_Text _loadBtnText;

	private CinemachinePOV _cinePovComp;


	private void Start() {
		_cinePovComp = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();

		InputManager.Instance.Pause.Disable();

		if (GetScene(0)) {
			_StartBtn.onClick.AddListener(StartNewGame);
			_LoadBtn.onClick.AddListener(LoadGame);
			_ExitBtn.onClick.AddListener(ExitGame);
		}

		_loadBtnText = _LoadBtn.GetComponentInChildren<TMP_Text>();
		_loadBtnText.color = HasLastScene() ? Color.white : Color.gray;
		_LoadBtn.interactable = HasLastScene();
	}


	public override async void StartNewGame() {
		await WorldSceneManager.Instance.UnloadScene(BuildSettingsLoader.StartupScene);

		await WorldSceneManager.Instance.LoadScene(sceneToLoadOnStart);
		GameObject spawn = PersistentStartup.SearchForPlayerSpawn(sceneToLoadOnStart);
		if (!spawn) return;
		SetPlayerPosAndRot(spawn.transform);

		DataManager.ClearSavedScene();

		EventSystemController.Instance.StartGame();
	}


	private async void LoadGame() {
		if (!HasLastScene()) {
			StartNewGame();
			return;
		}

		CheckAndLoadScene();

		await WorldSceneManager.Instance.UnloadScene(BuildSettingsLoader.StartupScene);

		EventSystemController.Instance.StartGame();
	}


	private async void CheckAndLoadScene() {
		string lastScene = DataManager.LastScene;

		await WorldSceneManager.Instance.LoadScene(lastScene);

		Door door = PersistentStartup.GetDoor(lastScene);
		if (door)
			door.CloseDoorWithoutExtras();

		GameObject spawn = PersistentStartup.SearchForPlayerSpawn(lastScene);
		if (!spawn) return;
		SetPlayerPosAndRot(spawn.transform);
	}


	private bool HasLastScene() {
		string lastScene = DataManager.LastScene;

		if (string.IsNullOrWhiteSpace(lastScene))
			return false;

		if (string.IsNullOrEmpty(lastScene))
			return false;

		return true;
	}


	private void SetPlayerPosAndRot(Transform spawn) {
		GameManager.Instance.Player.transform.position = spawn.position;
		_cinePovComp.m_HorizontalAxis.Value = spawn.eulerAngles.y;
		_cinePovComp.m_VerticalAxis.Value = 0f;
	}


	public override void ExitGame() {
		Application.Quit();
	}

}