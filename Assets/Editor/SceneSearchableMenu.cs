using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;


public class SceneSearchableMenu : ScriptableObject, ISearchWindowProvider {

	public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
		List<SearchTreeEntry> tree = new List<SearchTreeEntry>();

		SearchTreeGroupEntry group = new SearchTreeGroupEntry(new GUIContent("Scene Asset"), 0);
		tree.Add(group);

		SearchTreeGroupEntry collections = new SearchTreeGroupEntry(new GUIContent("Collections"), 1);
		tree.Add(collections);

		EditorSceneCollection[] sceneCollections = Resources.LoadAll<EditorSceneCollection>("SceneCollections");
		foreach (EditorSceneCollection sceneCollection in sceneCollections) {
			SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(sceneCollection.name));
			entry.level = 2;
			entry.userData = sceneCollection;
			tree.Add(entry);
		}

		SearchTreeGroupEntry scenes = new SearchTreeGroupEntry(new GUIContent("Scenes", EditorGUIUtility.IconContent("SceneAsset Icon").image), 1);
		tree.Add(scenes);

		string[] guids = AssetDatabase.FindAssets("t:SceneAsset");
		foreach (string guid in guids) {
			string path = AssetDatabase.GUIDToAssetPath(guid);
			SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
			SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(asset.name, EditorGUIUtility.ObjectContent(asset, typeof(EditorSceneCollection)).image));
			entry.level = 2;
			entry.userData = asset;
			tree.Add(entry);
		}

		return tree;
	}


	public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context) {
		if (SearchTreeEntry.userData is EditorSceneCollection collection) {
			collection.Open();
			return true;
		}
		else if (SearchTreeEntry.userData is SceneAsset asset) {
			EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(asset));
			return true;
		}

		return false;
	}

}