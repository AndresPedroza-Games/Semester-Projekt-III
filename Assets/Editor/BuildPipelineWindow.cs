using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;


public class BuildPipelineWindow : EditorWindow {

	private string buildName = "Vanishing Twin";
	private string version = "1.0.0";
	private bool developmentBuild = false;

	private BuildSettings _buildSettings;

	private SceneAsset sceneToAdd;
	private Vector2 sceneScroll;


	[MenuItem("Tools/Build/Open Window")]
	public static void Open() {
		GetWindow<BuildPipelineWindow>("Build Pipeline");
	}


	private void OnEnable() {
		version = PlayerSettings.bundleVersion;
	}


	private void OnGUI() {
		GUILayout.Label("Build Pipeline", EditorStyles.boldLabel);

		EditorGUILayout.Space();

		buildName = EditorGUILayout.TextField("Build Name", buildName);
		version = EditorGUILayout.TextField("Version", version);
		developmentBuild = EditorGUILayout.Toggle("Development Build", developmentBuild);
		
		_buildSettings = (BuildSettings)EditorGUILayout.ObjectField("Build Settings", _buildSettings, typeof(BuildSettings), false);

		EditorGUILayout.Space();

		if (_buildSettings) {
			EditorGUILayout.HelpBox($"Startup Scene: {_buildSettings.startupScene}\n" + $"Test Build: {_buildSettings.isTestBuild}", MessageType.Info);
		}
		else {
			EditorGUILayout.HelpBox("No Build Profile selected. A default MainMenu build will be created.", MessageType.Warning);
		}

		EditorGUILayout.Space(10);

		DrawSceneList();

		EditorGUILayout.Space(10);

		if (GUILayout.Button("Build for Windows", GUILayout.Height(35))) {
			BuildWindows();
		}
	}


	private void DrawSceneList() {
		EditorGUILayout.LabelField("Build Scenes", EditorStyles.boldLabel);

		var scenes = EditorBuildSettings.scenes.ToList();

		sceneScroll = EditorGUILayout.BeginScrollView(sceneScroll, GUILayout.Height(250));

		for (int i = 0; i < scenes.Count; i++) {
			EditorGUILayout.BeginHorizontal("box");

			var scene = scenes[i];

			scene.enabled = EditorGUILayout.Toggle(scene.enabled, GUILayout.Width(20));

			string sceneName = Path.GetFileNameWithoutExtension(scene.path);

			EditorGUILayout.LabelField($"[{i}] {sceneName}", GUILayout.ExpandWidth(true));

			if (GUILayout.Button("↑", GUILayout.Width(30))) {
				if (i > 0) {
					var temp = scenes[i];
					scenes[i] = scenes[i - 1];
					scenes[i - 1] = temp;

					SaveScenes(scenes);
					GUIUtility.ExitGUI();
				}
			}

			if (GUILayout.Button("↓", GUILayout.Width(30))) {
				if (i < scenes.Count - 1) {
					var temp = scenes[i];
					scenes[i] = scenes[i + 1];
					scenes[i + 1] = temp;

					SaveScenes(scenes);
					GUIUtility.ExitGUI();
				}
			}

			if (GUILayout.Button("X", GUILayout.Width(30))) {
				scenes.RemoveAt(i);

				SaveScenes(scenes);
				GUIUtility.ExitGUI();
			}

			scenes[i] = scene;

			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndScrollView();

		SaveScenes(scenes);

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();

		sceneToAdd = (SceneAsset)EditorGUILayout.ObjectField("Add Scene", sceneToAdd, typeof(SceneAsset), false);

		if (GUILayout.Button("Add", GUILayout.Width(60))) {
			AddScene();
		}

		EditorGUILayout.EndHorizontal();

		if (sceneToAdd != null) {
			AddScene();
		}
	}


	private void AddScene() {
		if (!sceneToAdd)
			return;

		string path = AssetDatabase.GetAssetPath(sceneToAdd);

		List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();

		bool alreadyExists = scenes.Any(s => s.path == path);

		if (!alreadyExists) {
			scenes.Add(new EditorBuildSettingsScene(path, true));
			SaveScenes(scenes);
		}

		sceneToAdd = null;
	}


	private void SaveScenes(List<EditorBuildSettingsScene> scenes) {
		EditorBuildSettings.scenes = scenes.ToArray();
	}


	public void BuildWindows() {
		BuildSettings runtimeSettings = Resources.Load<BuildSettings>("BuildSettings");

		if (!runtimeSettings) {
			Debug.LogError("No Runtime BuildSettings found in Resources!");
			return;
		}

		if (_buildSettings) {
			EditorUtility.CopySerialized(_buildSettings, runtimeSettings);
		}
		else {
			runtimeSettings.startupScene = "MainMenu";
			runtimeSettings.isTestBuild = false;
		}

		EditorUtility.SetDirty(runtimeSettings);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		string[] scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();

		if (scenes.Length == 0) {
			Debug.LogError("No scenes enabled.");
			return;
		}

		PlayerSettings.bundleVersion = version;

		string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

		string buildFolder = $"Builds/{buildName}_{version}_{timestamp}";

		if (!Directory.Exists(buildFolder)) {
			Directory.CreateDirectory(buildFolder);
		}

		string buildPath = Path.Combine(buildFolder, buildName + ".exe");

		BuildOptions options = developmentBuild ? BuildOptions.Development : BuildOptions.None;

		BuildPlayerOptions buildOptions = new BuildPlayerOptions { scenes = scenes, locationPathName = buildPath, target = BuildTarget.StandaloneWindows64, options = options };

		BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

		Debug.Log("Result: " + report.summary.result);
		Debug.Log("Size: " + report.summary.totalSize);
		Debug.Log("Errors: " + report.summary.totalErrors);
		Debug.Log("Warnings: " + report.summary.totalWarnings);

		if (report.summary.result == BuildResult.Succeeded) {
			version = IncrementVersion(version);

			PlayerSettings.bundleVersion = version;

			AssetDatabase.SaveAssets();

			Debug.Log($"Version increased to {version}");
		}

		Debug.Log($"Build finished at {buildPath}");
	}


	private string IncrementVersion(string currentVersion) {
		string[] parts = currentVersion.Split('.');

		if (parts.Length != 3) {
			Debug.LogWarning("Version format should be Major.Minor.Patch");
			return currentVersion;
		}

		if (!int.TryParse(parts[0], out int major))
			return currentVersion;

		if (!int.TryParse(parts[1], out int minor))
			return currentVersion;

		if (!int.TryParse(parts[2], out int patch))
			return currentVersion;

		patch++;

		return $"{major}.{minor}.{patch}";
	}

}