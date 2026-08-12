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
	[field: SerializeField] public AudioSource AudioSource { get; private set; }

	[SerializeField] private List<SfxEventEntry> sounds = new();

	private Dictionary<SfxEvent, SfxEventEntry> _lookup;


	private void Awake() {
		if (!AudioSource)
			AudioSource = GetComponent<AudioSource>();

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

		AudioSource.outputAudioMixerGroup = a.group;

		ApplyOverrides(a, entry.overrides, entry.overrides.useOverrides);

		if (AudioSource.loop) {
			AudioSource.clip = a.clip;
			AudioSource?.Play();
		}
		else {
			AudioSource?.PlayOneShot(a.clip, a.volume);
		}

	}


	private void ApplyOverrides(Audio a, SoundOverrides overrides, bool useOverrides) {
		if (useOverrides) {
			AudioSource.loop = overrides.loop;
			AudioSource.volume = overrides.volume;
			AudioSource.pitch = Random.Range(overrides.pitchRange.x, overrides.pitchRange.y);
			AudioSource.spatialBlend = overrides.spatialBlend;
		}
		else {
			AudioSource.loop = a.loop;
			AudioSource.volume = a.volume;
			AudioSource.pitch = a.pitch;
			AudioSource.spatialBlend = a.spatialBlend;
		}
	}

}