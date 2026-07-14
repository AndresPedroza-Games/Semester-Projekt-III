using UnityEngine;


public class RoomManager : MonoBehaviour {

	[SerializeField] private SceneReference additionalSceneToLoad;


	private void OnEnable() {
		if (additionalSceneToLoad != null)
			LoadAdditionalScene(additionalSceneToLoad);
	}


	private async void LoadAdditionalScene(SceneReference scene) {
		await WorldSceneManager.Instance.LoadScene(scene);
	}

}