using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "EditorSceneCollection", menuName = "Scriptable Objects/EditorSceneCollection")]
public class EditorSceneCollection : ScriptableObject {

	[SerializeField] private SceneAsset[] sceneAssets;


	public void Open() {
		int index = 0;
		
		foreach (SceneAsset asset in  sceneAssets) {
			string path = AssetDatabase.GetAssetPath(asset);
			EditorSceneManager.OpenScene(path, (index == 0) ? OpenSceneMode.Single : OpenSceneMode.Additive);
			index++;
		}
		
	}

}
