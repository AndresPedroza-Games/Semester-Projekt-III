using Cinemachine;
using UnityEngine;


public class DisableCamInputOnPause : MonoBehaviour {

	private CinemachineInputProvider _input;


	private void Awake() {
		_input = GetComponent<CinemachineInputProvider>();
	}


	private void OnEnable() {
		EventSystemController.Instance.onPauseGame += DisableInput;
		EventSystemController.Instance.onStartGame += ActivateInput;
		EventSystemController.Instance.onResumeGame += ActivateInput;
	}


	private void OnDisable() {
		EventSystemController.Instance.onPauseGame -= DisableInput;
		EventSystemController.Instance.onStartGame -= ActivateInput;
		EventSystemController.Instance.onResumeGame -= ActivateInput;
	}


	private void DisableInput() {
		_input.enabled = false;
	}


	private void ActivateInput() {
		_input.enabled = true;
	}

}