using UnityEngine;


[CreateAssetMenu(fileName = "BuildSettings", menuName = "Scriptable Objects/BuildSettings")]
public class BuildSettings : ScriptableObject {

	public string startupScene;
	public bool isTestBuild;

}