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

    public override async void StartGame() {
	    await WorldSceneManager.Instance.LoadScene("0_Tutorial_Hallway");
	    
	    await WorldSceneManager.Instance.UnloadScene("MainMenu");
	    
	    EventSystemController.eventSystemController.StartGame();
    }

    public override void ExitGame()
    {
        Debug.Log("Exit");
    }
}
