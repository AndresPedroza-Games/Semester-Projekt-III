using UnityEngine;

public class MainMenu : MenuManager
{
    private void Start()
    {
        if (GetScene(0))
        {
            _StartBtn.onClick.AddListener(StartGame);
            _ExitBtn.onClick.AddListener(ExitGame);
        }
    }

    public override void StartGame()
    {
        ChangeScene(1);
    }

    public override void ExitGame()
    {
        Debug.Log("Exit");
    }
}
