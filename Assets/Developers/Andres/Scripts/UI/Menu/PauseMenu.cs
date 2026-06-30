using TMPro;


public class PauseMenu : MenuManager {

	public static PauseMenu pauseMenu;


	private void Awake() {
		if (pauseMenu == null)
			pauseMenu = this;

		HideMenu();
	}


	private void Start() {

		_StartBtn.onClick.AddListener(EventSystemController.Instance.ResumeGame);
		_ExitBtn.onClick.AddListener(ExitGame);

		_StartBtn.GetComponentInChildren<TMP_Text>().text = "Resume";
	}


	private void OnEnable() {
		EventSystemController.Instance.onPauseGame += ShowMenu;
		EventSystemController.Instance.onResumeGame += HideMenu;
		EventSystemController.Instance.onStartGame += HideMenu;
	}


	private void OnDisable() {
		EventSystemController.Instance.onPauseGame -= ShowMenu;
		EventSystemController.Instance.onResumeGame -= HideMenu;
		EventSystemController.Instance.onStartGame -= HideMenu;
	}


	private void ShowMenu() {
		mainMenuHUD.SetActive(true);

	}


	private void HideMenu() {
		mainMenuHUD.SetActive(false);
		_SettingsMenu.SetActive(false);
		_CreditMenu.SetActive(false);
	}


	public override async void ExitGame() {
		// _ = LoadScene("MainMenu");
		
		EventSystemController.Instance.MainMenuEntered();
		
		HideMenu();
		
		await LoadScene(BuildSettingsLoader.StartupScene);

		await WorldSceneManager.Instance.UnloadAllExcept(WorldSceneManager.persistent, BuildSettingsLoader.StartupScene);

	}

}