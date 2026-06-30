using UnityEngine;
using UnityEngine.UI;


public class LoadLevelButton : MonoBehaviour {

	[Header("---LEVEL TO LOAD---")]
	public SceneReference lvlToLoad;

	private Button _btn;


	private void Awake() {
		_btn = GetComponent<Button>();
	}


	private void OnEnable() {
		_btn.onClick.AddListener(Loadlevel);
	}


	private void OnDisable() {
		_btn.onClick.RemoveListener(Loadlevel);
	}


	private async void Loadlevel() {
		await WorldSceneManager.Instance.LoadScene(lvlToLoad);
		await WorldSceneManager.Instance.UnloadScene(BuildSettingsLoader.StartupScene);

		GameObject player = PersistentStartup.SearchForPlayer();
		GameObject playerSpawn = PersistentStartup.SearchForPlayerSpawn(lvlToLoad);

		player.transform.position = playerSpawn.transform.position;

		EventSystemController.Instance.StartGame();
	}


}