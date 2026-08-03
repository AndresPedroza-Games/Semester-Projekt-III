using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    [Header("---Btns---")]
    [SerializeField] protected Button _StartBtn;
    [SerializeField] protected Button _LoadBtn;
    [SerializeField] protected Button _ExitBtn;

    [Header("---Menus---")]
    [SerializeField] protected GameObject _SettingsMenu;
    [SerializeField] protected GameObject _CreditMenu;

    public GameObject mainMenuHUD;

    protected bool GetScene(int index)
    {
        return SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(index) ? true : false;
    }

    protected async Task LoadScene(string sceneName)
    {
	    await WorldSceneManager.Instance.LoadScene(sceneName);
    }

    public virtual void StartNewGame() { }
    public virtual void ExitGame() { }
}
