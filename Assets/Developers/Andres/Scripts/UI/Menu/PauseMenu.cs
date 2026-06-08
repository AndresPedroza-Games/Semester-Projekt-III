using TMPro;


public class PauseMenu : MenuManager {

	public static PauseMenu pauseMenu;


	private void Awake() {
		if (pauseMenu == null)
			pauseMenu = this;
		
		ShowMenu(false);
	}


	private void Start() {

        _StartBtn.onClick.AddListener(EventSystemController.Instance.ResumeGame);
        _ExitBtn.onClick.AddListener(ExitGame);

        _StartBtn.GetComponentInChildren<TMP_Text>().text = "Resume";

		EventSystemController.Instance.onPauseGame += () => ShowMenu(true);
		EventSystemController.Instance.onResumeGame += () => ShowMenu(false);
    }


	public void ShowMenu(bool status) {
		mainMenuHUD.SetActive(status);
	}


	public override void ExitGame() {
		_ = LoadScene("MainMenu");
	}

}