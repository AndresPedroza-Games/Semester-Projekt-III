using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(AudioSource))]
public class ObjectSfx : MonoBehaviour {

	[Serializable]
	public class SfxEventEntry {

		public SfxEvent trigger;
		public Sfx sfx;

		public SoundOverrides overrides;

	}


	[Header("---AudioSource---")]
	[SerializeField] private AudioSource audioSource;

	[SerializeField] private List<SfxEventEntry> sounds = new();

	private Dictionary<SfxEvent, SfxEventEntry> _lookup;


	private void Awake() {
		if (!audioSource)
			audioSource = GetComponent<AudioSource>();

		_lookup = new Dictionary<SfxEvent, SfxEventEntry>();

		foreach (SfxEventEntry sound in sounds) {
			_lookup[sound.trigger] = sound;
		}
	}


	public void PlaySfx(SfxEvent trigger) {
		if (!_lookup.TryGetValue(trigger, out SfxEventEntry entry))
			return;

		Audio a = AudioManager.Instance.GetSfx(entry.sfx);

		if (a == null)
			return;

		audioSource.outputAudioMixerGroup = a.group;

		ApplyOverrides(a, entry.overrides, entry.overrides.useOverrides);

		if (audioSource.loop) {
			audioSource.clip = a.clip;
			audioSource?.Play();
		}
		else {
			audioSource?.PlayOneShot(a.clip, a.volume);
		}

	}


	private void ApplyOverrides(Audio a, SoundOverrides overrides, bool useOverrides) {
		if (useOverrides) {
			audioSource.loop = overrides.loop;
			audioSource.volume = overrides.volume;
			audioSource.pitch = Random.Range(overrides.pitchRange.x, overrides.pitchRange.y);
			audioSource.spatialBlend = overrides.spatialBlend;
		}
		else {
			audioSource.loop = a.loop;
			audioSource.volume = a.volume;
			audioSource.pitch = a.pitch;
			audioSource.spatialBlend = a.spatialBlend;
		}
	}

}