using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("---Btns---")]
    [SerializeField] protected Button _StartBtn;
    [SerializeField] protected Button _ExitBtn;

    public GameObject mainMenuHUD;
    public static MenuManager menuManager;

    private void Awake()
    {
        mainMenuHUD = FindFirstObjectByType<MainMenu>(FindObjectsInactive.Include).gameObject;

        if (menuManager == null)
            menuManager = this;
    }

    protected bool GetScene(int index)
    {
        return SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(index) ? true : false;
    }

    protected void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ShowMenu(bool status)
    {
        mainMenuHUD.SetActive(status);
    }

    public virtual void StartGame() { }
    public virtual void ExitGame() { }
    public virtual void ResumeGame() { }
}
