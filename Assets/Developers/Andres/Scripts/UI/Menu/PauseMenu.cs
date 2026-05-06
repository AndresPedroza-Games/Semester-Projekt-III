using TMPro;

public class PauseMenu : MenuManager
{
    public static PauseMenu pauseMenu;

    private void Awake()
    {
        if (pauseMenu == null)
            pauseMenu = this;
    }

    private void Start()
    {
        if (GetScene(1))
        {
            _StartBtn.onClick.AddListener(ResumeGame);
            _ExitBtn.onClick.AddListener(ExitGame);
        }

        _StartBtn.GetComponentInChildren<TMP_Text>().text = "Resume";
    }
    public void ShowMenu(bool status)
    {
        mainMenuHUD.SetActive(status);
    }

    public override void ResumeGame()
    {
        ShowMenu(false);
    }

    public override void ExitGame()
    {
        ChangeScene(0);
    }
}
