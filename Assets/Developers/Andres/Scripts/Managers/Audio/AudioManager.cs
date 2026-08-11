using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;


public enum MixerChannel {

	Master,
	Music,
	Sfx

}

[DefaultExecutionOrder(-1)]
public class AudioManager : MonoBehaviour {

	public static AudioManager Instance { get; private set; }

	[Header("---Audio---")]
	[SerializeField] private AudioMixer audioMixer;
	private readonly Dictionary<MixerChannel, string> _mixerParameters = new() { { MixerChannel.Master, "Master" }, { MixerChannel.Music, "Music" }, { MixerChannel.Sfx, "SFX" }, };

	[Header("---SFX---")]
	[SerializeField] private SFXDataSO sfxData;

	public List<Audio> audioList = new List<Audio>();

	private const string FirstLaunchKey = "FirstLaunchDone";


	private void Awake() {
		if (Instance && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;

		SetComponentsAudio();
	}


	private void Start() {
		if (!PlayerPrefs.HasKey(FirstLaunchKey)) {
			SetDefaultVolume();

			PlayerPrefs.SetInt(FirstLaunchKey, 1);
			PlayerPrefs.Save();
		}
		else {
			GetAndSetSavedVolume();
		}
	}


	private void OnEnable() {
		EventSystemController.Instance.onPauseGame += PauseListener;
		EventSystemController.Instance.onResumeGame += ResumeListener;
		EventSystemController.Instance.onMainMenuEntered += ResumeListener;
		EventSystemController.Instance.onStartGame += ResumeListener;
	}


	private void OnDisable() {
		EventSystemController.Instance.onPauseGame -= PauseListener;
		EventSystemController.Instance.onResumeGame -= ResumeListener;
		EventSystemController.Instance.onMainMenuEntered -= ResumeListener;
		EventSystemController.Instance.onStartGame -= ResumeListener;
	}


	private void SetDefaultVolume() {
		foreach (var mixer in _mixerParameters) {
			const float defaultVolume = 0.3f;

			float dB = Mathf.Log10(Mathf.Clamp(defaultVolume, 0.0001f, 1f)) * 20f;

			audioMixer.SetFloat(mixer.Value, dB);
			PlayerPrefs.SetFloat(mixer.Value, defaultVolume);
		}
	}


	private void GetAndSetSavedVolume() {
		foreach (var mixer in _mixerParameters) {
			if (!PlayerPrefs.HasKey(mixer.Value))
				continue;

			float volume = PlayerPrefs.GetFloat(mixer.Value);

			float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;

			audioMixer.SetFloat(mixer.Value, dB);
		}

	}


	public void SetVolume(MixerChannel channel, float linearVolume) {
		linearVolume = Mathf.Clamp(linearVolume, 0.0001f, 1f);

		float dB = Mathf.Log10(linearVolume) * 20f;

		audioMixer.SetFloat(_mixerParameters[channel], dB);
		PlayerPrefs.SetFloat(_mixerParameters[channel], linearVolume);
		PlayerPrefs.Save();
	}


	public float GetVolume(MixerChannel channel) {
		return PlayerPrefs.GetFloat(_mixerParameters[channel], 0.3f);
	}


	private void PauseListener() {
		AudioListener.pause = true;
	}


	private void ResumeListener() {
		AudioListener.pause = false;
	}


	private void SetComponentsAudio() {
		foreach (Audio audio in audioList) {
			audio.source = gameObject.AddComponent<AudioSource>();
			audio.source.clip = audio.clip;
			audio.source.playOnAwake = false;
			audio.source.volume = audio.volume;
			audio.originalVolume = audio.volume;
			audio.source.pitch = audio.pitch;
			audio.source.loop = audio.loop;
			audio.source.outputAudioMixerGroup = audio.group;
		}
	}


	public void Play(string audioName) {
		foreach (Audio audio in audioList) {
			if (audio.audioName == audioName) {
				audio.source.Play();
				audio.source.pitch = SetRandomPitch();
				Debug.Log("Audio is played");
			}

		}
	}


	public void StopPlay(string audioName) {
		foreach (Audio audio in audioList) {
			if (audio.audioName == audioName)
				audio.source.Stop();
		}
	}


	public void Mute(string name, bool muteStatus) {
		foreach (Audio audio in audioList) {
			if (audio.group.ToString() == name)
				audio.source.mute = muteStatus;
		}
	}


	private float SetRandomPitch() {
		float pitch = Random.Range(0.8f, 1.2f);

		return pitch;
	}


	public Audio GetSfx(Sfx sfx) {
		return sfxData.GetSfx(sfx);
	}

}