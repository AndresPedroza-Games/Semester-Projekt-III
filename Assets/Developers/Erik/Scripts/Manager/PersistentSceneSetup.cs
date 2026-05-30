using UnityEngine;


public class PersistentSceneSetup : MonoBehaviour {

	private async void Awake() {
		DontDestroyOnLoad(gameObject);
		await WorldSceneManager.Instance.LoadScene("MainMenu");
	}

}