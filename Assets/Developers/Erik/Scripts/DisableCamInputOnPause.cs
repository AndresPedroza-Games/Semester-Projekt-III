using System;
using Cinemachine;
using UnityEngine;


public class DisableCamInputOnPause : MonoBehaviour {

	private CinemachineInputProvider _input;
	private bool _b = true;


	private void Awake() {
		_input = GetComponent<CinemachineInputProvider>();
	}


	private void OnEnable() {
		EventSystemController.Instance.onPauseGame += ToggleInputProvider;
	}


	private void OnDisable() {
		EventSystemController.Instance.onPauseGame -= ToggleInputProvider;

	}


	private void ToggleInputProvider() {
		_b = !_b;

		_input.enabled = _b;
	}

}