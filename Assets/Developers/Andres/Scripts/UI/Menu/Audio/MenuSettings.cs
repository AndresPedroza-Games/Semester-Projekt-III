using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;


public class MenuSettings : MenuManager {

	[Header("---Mouse---")]
	[SerializeField] private float mouseSensitivityMinValue = 0.01f;
	[SerializeField] private float mouseSensitivityMaxValue = 1f;
	[SerializeField] private MouseSensitivitySlider mouseSensitivitySlider;
	private CinemachinePOV _cinePov;

	[Header("---Audio---")]
	[SerializeField] private List<AudioSlider> audioSliders;

	[Header("---Fullscreen---")]
	[SerializeField] private Toggle fullscreenToggle;


	private void Start() {
		foreach (AudioSlider slider in audioSliders) {
			float volume = AudioManager.Instance.GetVolume(slider.mixerChannel);

			slider.slider.SetValueWithoutNotify(volume);
			UpdateVolumeToText(slider, volume);

			slider.slider.onValueChanged.AddListener(value => {
				AudioManager.Instance.SetVolume(slider.mixerChannel, value);
				UpdateVolumeToText(slider, value);
			});
			slider.inputField.onValueChanged.AddListener(value => OnInputFieldChanged(slider, value));
			slider.inputField.onEndEdit.AddListener((value) => OnInputFieldConfirmed(slider, value));
		}

		if (fullscreenToggle) {
			fullscreenToggle.isOn = Screen.fullScreen;
			fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
		}

		SetupMouseSensitivitySlider();
	}


	private void OnDisable()
	{
		foreach (var slider in audioSliders)
		{
			slider.slider.onValueChanged.RemoveAllListeners();
			slider.inputField.onValueChanged.RemoveAllListeners();
			slider.inputField.onEndEdit.RemoveAllListeners();
		}

		if (fullscreenToggle)
			fullscreenToggle.onValueChanged.RemoveListener(SetFullscreen);

		mouseSensitivitySlider.slider.onValueChanged.RemoveListener(SetMouseSensitivity);
		mouseSensitivitySlider.inputField.onEndEdit.RemoveListener(OnMouseSensitivityConfirmed);
	}


	private void UpdateVolumeToText(AudioSlider audioSlider, float linearVolume) {
		float percent = Mathf.InverseLerp(audioSlider.slider.minValue, audioSlider.slider.maxValue, linearVolume);
		int volumeInPercent = Mathf.RoundToInt(percent * 100f);
		audioSlider.inputField.SetTextWithoutNotify($"{volumeInPercent} %");
	}


	private void OnInputFieldChanged(AudioSlider audioSlider, string name) {
		string cleanedInput = new string(name.Where(char.IsDigit).ToArray());

		if (cleanedInput.Length > 3)
			cleanedInput = cleanedInput.Substring(0, 3);

		audioSlider.inputField.text = cleanedInput;
	}


	private void OnInputFieldConfirmed(AudioSlider audioSlider, string name) {
		if (int.TryParse(name, out int percent)) {
			percent = Mathf.Clamp(percent, 0, 100);
			float normalized = percent / 100f;
			float linearValue = Mathf.Lerp(audioSlider.slider.minValue, audioSlider.slider.maxValue, normalized);

			audioSlider.slider.value = linearValue;
		}
		else {
			audioSlider.slider.SetValueWithoutNotify(AudioManager.Instance.GetVolume(audioSlider.mixerChannel));

			UpdateVolumeToText(audioSlider, audioSlider.slider.value);
		}
	}


	private void SetupMouseSensitivitySlider() {
		_cinePov = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();

		mouseSensitivitySlider.slider.minValue = mouseSensitivityMinValue;
		mouseSensitivitySlider.slider.maxValue = mouseSensitivityMaxValue;

		mouseSensitivitySlider.slider.value = _cinePov.m_HorizontalAxis.m_MaxSpeed;
		UpdateMouseSensitivityText(_cinePov.m_HorizontalAxis.m_MaxSpeed);

		mouseSensitivitySlider.slider.onValueChanged.AddListener(SetMouseSensitivity);
		mouseSensitivitySlider.inputField.onEndEdit.AddListener(OnMouseSensitivityConfirmed);
	}


	private void SetMouseSensitivity(float value) {
		_cinePov.m_HorizontalAxis.m_MaxSpeed = value;
		_cinePov.m_VerticalAxis.m_MaxSpeed = value;

		UpdateMouseSensitivityText(value);
	}


	private void UpdateMouseSensitivityText(float value) {
		mouseSensitivitySlider.inputField.text = value.ToString("0.00");
	}


	private void OnMouseSensitivityConfirmed(string text) {
		if (float.TryParse(text, out float value)) {
			value = Mathf.Clamp(value, mouseSensitivitySlider.slider.minValue, mouseSensitivitySlider.slider.maxValue);

			mouseSensitivitySlider.slider.value = value;
		}
		else {
			UpdateMouseSensitivityText(_cinePov.m_HorizontalAxis.m_MaxSpeed);
		}
	}


	private void SetFullscreen(bool isFullscreen) {
		Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
	}

}