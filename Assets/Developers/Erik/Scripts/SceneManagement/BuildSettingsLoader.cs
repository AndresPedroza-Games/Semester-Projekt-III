using UnityEngine;


public class BuildSettingsLoader {

	private static BuildSettings _settings;

	public static BuildSettings Settings {
		get {
			if (!_settings)
				_settings = Resources.Load<BuildSettings>("BuildSettings");

			return _settings;
		}
	}

	public static string StartupScene {
		get {
			if (Settings)
				return Settings.startupScene;

			return "MainMenu";
		}
	}

}