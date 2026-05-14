using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class Zoom : MonoBehaviour {

	[Header("---Zoom Config---")]
	[SerializeField] private float zoomFOV = 20f;
	[SerializeField] private float zoomDuration = 0.2f;

	private float defaultFOV;

	private CinemachineVirtualCamera cineCam;
	private Coroutine zoomRoutine;


	private void Awake() {
		cineCam = FindAnyObjectByType<CinemachineVirtualCamera>();
		defaultFOV = cineCam.m_Lens.FieldOfView;
	}


	private void OnEnable() {
		InputManager.Instance.Zoom.performed += ZoomPerformed;
		InputManager.Instance.Zoom.canceled += ZoomCanceled;
	}


	private void OnDisable() {
		InputManager.Instance.Zoom.performed -= ZoomPerformed;
		InputManager.Instance.Zoom.canceled -= ZoomCanceled;
	}


	private void ZoomPerformed(InputAction.CallbackContext ctx) {
		if (zoomRoutine != null) {
			StopCoroutine(zoomRoutine);
			zoomRoutine = null;
		}
		
		zoomRoutine = StartCoroutine(ZoomCoroutine(zoomFOV));
	}


	private void ZoomCanceled(InputAction.CallbackContext ctx) {
		if (zoomRoutine != null) {
			StopCoroutine(zoomRoutine);
			zoomRoutine = null;
		}

		zoomRoutine = StartCoroutine(ZoomCoroutine(defaultFOV));
	}


	private IEnumerator ZoomCoroutine(float targetFOV) {

		float startFOV = cineCam.m_Lens.FieldOfView;
		float time = 0f;

		while (time < zoomDuration) {
			time += Time.deltaTime;

			cineCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, time / zoomDuration);
			yield return null;
		}

		cineCam.m_Lens.FieldOfView = targetFOV;
		zoomRoutine = null;
	}

}