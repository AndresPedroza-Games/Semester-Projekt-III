using System;
using UnityEngine;
using Object = UnityEngine.Object;


[Serializable]
public class SceneField {

	[SerializeField] private Object sceneAsset;
	[SerializeField] private string sceneName = "";

	public string SceneName => sceneName;


	public static implicit operator string(SceneField sceneField) {
		return sceneField.SceneName;
	}

}