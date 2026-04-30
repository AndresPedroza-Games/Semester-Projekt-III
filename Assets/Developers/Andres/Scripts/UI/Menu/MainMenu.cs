using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MenuManager
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
    }
}
