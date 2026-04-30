using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManager;

    public GameObject mainMenuHUD;
    public GameObject optionsMenuHUD;
    public GameObject creditsMenuHUD;

    private void Awake()
    {
        if (menuManager == null)
            menuManager = this;

        mainMenuHUD = FindFirstObjectByType<MainMenu>(FindObjectsInactive.Include).gameObject;
        optionsMenuHUD = FindFirstObjectByType<OptionsMenu>(FindObjectsInactive.Include).gameObject;
        creditsMenuHUD = FindFirstObjectByType<CreditsMenu>(FindObjectsInactive.Include).gameObject;
    }
}
