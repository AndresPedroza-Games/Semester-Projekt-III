using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;

    public List<Audio> audioList = new List<Audio>();

    private void Awake()
    {
        if (audioManager == null)
            audioManager = this;

        SetComponentsAudio();
    }

    private void SetComponentsAudio()
    {
        foreach (Audio audio in audioList)
        {
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

    public void Play(string audioName)
    {
        foreach (Audio audio in audioList)
        {
            if (audio.audioName == audioName)
            {
                audio.source.Play();
                audio.source.pitch = SetRandomPitch();
                Debug.Log("Audio is played");
            }

        }
    }

    public void StopPlay(string audioName)
    {
        foreach (Audio audio in audioList)
        {
            if (audio.audioName == audioName)
                audio.source.Stop();
        }
    }

    public void Mute(string name, bool muteStatus)
    {
        foreach (Audio audio in audioList)
        {
            if (audio.group.ToString() == name)
                audio.source.mute = muteStatus;
        }
    }

    private float SetRandomPitch()
    {
        float pitch = Random.Range(0.8f, 1.2f);

        return pitch;
    }
}
