using UnityEngine;


public class PersistentSceneSetup : MonoBehaviour {

	private void Awake() {
		DontDestroyOnLoad(gameObject);
	}


	private async void Start() {
		await WorldSceneManager.Instance.LoadScene("MainMenu");
	}

}