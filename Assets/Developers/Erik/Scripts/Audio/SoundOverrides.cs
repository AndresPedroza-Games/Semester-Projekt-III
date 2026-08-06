using System;
using UnityEngine;


[Serializable]
public class SoundOverrides {

	[Header("---Yes or No---")]
	public bool useOverrides = true;

	[Header("---Overrides---")]
	public bool loop;
	[Range(0f, 1f)] public float volume = 1f;
	public Vector2 pitchRange = Vector2.one;
	[Range(0f, 1f)] public float spatialBlend = 1f;

}