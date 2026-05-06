using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Linq;

public class AudioSettings : MenuManager
{
    [Header("---Audio---")]
    [SerializeField] private AudioMixer _AudioMixer;
    [SerializeField] private List<AudioSlider> _AudioSliders;

    private void Start()
    {
        foreach (AudioSlider audioSlider in _AudioSliders)
        {
            if (_AudioMixer.GetFloat(audioSlider.name, out float dbValue))
            {
                float linearVolume = Mathf.Pow(10f, dbValue / 20f);
                linearVolume = Mathf.Clamp(linearVolume, audioSlider.slider.minValue, audioSlider.slider.maxValue);
                audioSlider.slider.value = linearVolume;

                UpdateVolumeToText(audioSlider, linearVolume);

                audioSlider.slider.onValueChanged.AddListener((volume) => SetVolume(audioSlider, volume));
                audioSlider.inputField.onValueChanged.AddListener((name) => OnInputFieldChanged(audioSlider, name));
                audioSlider.inputField.onEndEdit.AddListener((name) => OnInputFieldConfirmed(audioSlider, name));
            }
        }
    }

    private void SetVolume(AudioSlider audioSlider,float volume)
    {
        _AudioMixer.SetFloat(audioSlider.name, Mathf.Log10(volume) * 20f);
        UpdateVolumeToText(audioSlider, volume);
    }

    private void UpdateVolumeToText(AudioSlider audioSlider, float linearVolume)
    {
        float percent = Mathf.InverseLerp(audioSlider.slider.minValue, audioSlider.slider.maxValue, linearVolume);
        int volumeInPercent = Mathf.RoundToInt(percent * 100f);
        audioSlider.inputField.text = $"{volumeInPercent.ToString()} %";
    }

    private void OnInputFieldChanged(AudioSlider audioSlider, string name)
    {
        string cleanedInput = new string(name.Where(char.IsDigit).ToArray());

        if (cleanedInput.Length > 3)
            cleanedInput = cleanedInput.Substring(0, 3);

        audioSlider.inputField.text = cleanedInput;
    }

    private void OnInputFieldConfirmed(AudioSlider audioSlider, string name)
    {
        if (int.TryParse(name, out int percent))
        {
            percent = Mathf.Clamp(percent, 0, 100);
            float normalized = percent / 100f;
            float linearValue = Mathf.Lerp(audioSlider.slider.minValue, audioSlider.slider.maxValue, normalized);

            audioSlider.slider.value = linearValue;
            SetVolume(audioSlider, linearValue);
        }
        else
        {
            if (_AudioMixer.GetFloat(audioSlider.name, out float dB))
            {
                float linear = Mathf.Pow(10f, dB / 20f);
                UpdateVolumeToText(audioSlider, linear);

            }
        }
    }
}
