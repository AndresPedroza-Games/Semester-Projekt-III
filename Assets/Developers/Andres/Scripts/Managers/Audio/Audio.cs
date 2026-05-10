using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Audio
{
    public string audioName;

    public AudioClip clip;

    public AudioMixerGroup group;

    public bool loop;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public AudioSource source;

    public float originalVolume;

}
