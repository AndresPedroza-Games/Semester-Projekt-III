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


	[SerializeField] private List<SfxEventEntry> sounds = new();

	private Dictionary<SfxEvent, SfxEventEntry> _lookup;

	private AudioSource _source;


	private void Awake() {
		_source = GetComponent<AudioSource>();

		_lookup = new Dictionary<SfxEvent, SfxEventEntry>();

		foreach (SfxEventEntry sound in sounds) {
			_lookup[sound.trigger] = sound;
		}
	}


	public void PlaySfx(SfxEvent trigger) {
		if (!_lookup.TryGetValue(trigger, out SfxEventEntry entry))
			return;

		Audio audio = AudioManager.audioManager.GetSfx(entry.sfx);

		if (audio == null)
			return;

		_source.outputAudioMixerGroup = audio.group;

		ApplyOverrides(audio, entry.overrides, entry.overrides.useOverrides);

		if (_source.loop) {
			_source.clip = audio.clip;
			_source.Play();
		}
		else {
			_source.PlayOneShot(audio.clip, audio.volume);
		}

	}


	private void ApplyOverrides(Audio audio, SoundOverrides overrides, bool useOverrides) {
		if (useOverrides) {
			_source.loop = overrides.loop;
			_source.volume = overrides.volume;
			_source.pitch = Random.Range(overrides.pitchRange.x, overrides.pitchRange.y);
			_source.spatialBlend = overrides.spatialBlend;
		}
		else {
			_source.loop = audio.loop;
			_source.volume = audio.volume;
			_source.pitch = audio.pitch;
			_source.spatialBlend = audio.spatialBlend;
		}
	}

}