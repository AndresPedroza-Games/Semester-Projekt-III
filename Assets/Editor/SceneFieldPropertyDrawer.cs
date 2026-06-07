using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneReference))]
public class SceneFieldPropertyDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);

		var sceneAsset = property.FindPropertyRelative("sceneAsset");

		EditorGUI.BeginChangeCheck();

		var newAsset = EditorGUI.ObjectField(position, label, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);

		if (EditorGUI.EndChangeCheck()) {
			sceneAsset.objectReferenceValue = newAsset;
		}

		EditorGUI.EndProperty();
	}

}
#endif