using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class ObjectSfx : MonoBehaviour {

	[Serializable]
	public class SfxEventEntry {

		public SfxEvent trigger;
		public Sfx sfx;

	}


	[SerializeField] private List<SfxEventEntry> sounds = new();

	private Dictionary<SfxEvent, Sfx> _lookup;

	private AudioSource _source;


	private void Awake() {
		_source = GetComponent<AudioSource>();

		_lookup = new Dictionary<SfxEvent, Sfx>();

		foreach (SfxEventEntry sound in sounds) {
			_lookup[sound.trigger] = sound.sfx;
		}
	}


	public void PlaySfx(SfxEvent trigger) {
		if (!_lookup.TryGetValue(trigger, out Sfx sfx))
			return;

		Audio audio = AudioManager.audioManager.GetSfx(sfx);

		if (audio == null)
			return;

		_source.outputAudioMixerGroup = audio.group;
		_source.pitch = audio.pitch;

		_source.PlayOneShot(audio.clip, audio.volume);
	}

}