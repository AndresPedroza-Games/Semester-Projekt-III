using TMPro;

public class PauseMenu : MenuManager
{
    private void Start()
    {
        if (GetScene(1))
        {
            _StartBtn.onClick.AddListener(ResumeGame);
            _ExitBtn.onClick.AddListener(ExitGame);
        }

        _StartBtn.GetComponentInChildren<TMP_Text>().text = "Resume";
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
