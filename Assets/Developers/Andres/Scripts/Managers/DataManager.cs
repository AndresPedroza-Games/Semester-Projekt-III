using UnityEngine;


public class DataManager : MonoBehaviour {

	public static string LastScene;


	private void Start() {
		LastScene = PlayerPrefs.GetString("LastScene");

		EventSystemController.Instance.onDoorClosed += SaveGame;
	}


	private void SaveGame(string scene) {
		if (scene == LastScene)
			return;

		LastScene = scene;

		PlayerPrefs.SetString("LastScene", scene);
		PlayerPrefs.Save();

		EventSystemController.Instance.SaveGame();
	}


	public static void ClearSavedScene() {
		LastScene = null;

		PlayerPrefs.DeleteKey("LastScene");
		PlayerPrefs.Save();
	}

}