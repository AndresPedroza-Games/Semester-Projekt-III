using System;
using System.Collections.Generic;
using UnityEngine;


public enum SfxEvent {

	OnCollision,
	OnInteract,
	OnPickup,
	OnDoorClose,

}


public enum Sfx {

	None,
	KeyCollect,

	DefaultInteract,

	DefaultPickup,
	DefaultCollision,

	DoorOpen,
	DoorClose,
	DoorLocked,

	ProjectorButton,
	ProjectorPowerButton,

	LightSwitch,
	WallClock,
	FlashLightToggle

}


[CreateAssetMenu(fileName = "SFXData", menuName = "Scriptable Objects/SFXData")]
public class SFXDataSO : ScriptableObject {

	[Serializable]
	public class Entry {

		public Sfx sfx;
		public Audio audio;

	}


	[SerializeField] private List<Entry> entries = new();

	private Dictionary<Sfx, Audio> _lookup;


	private void OnEnable() {
		_lookup = new Dictionary<Sfx, Audio>();

		foreach (Entry entry in entries)
			_lookup[entry.sfx] = entry.audio;
	}


	public Audio GetSfx(Sfx sfx) {
		return _lookup.GetValueOrDefault(sfx);
	}

}