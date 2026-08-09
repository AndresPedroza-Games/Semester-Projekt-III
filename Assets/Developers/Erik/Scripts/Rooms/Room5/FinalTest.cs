using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class FinalTest : MonoBehaviour, IInteractable, ICrosshair {

	[Header("---Ending Scenes---")]
	[SerializeField] private SceneReference endingScene1;
	[SerializeField] private SceneReference endingScene2;

	[Header("---Cam---")]
	[SerializeField] private CinemachineVirtualCamera testCam;
	private CinemachineVirtualCamera _playerCam;

	[Header("---Canvas---")]
	[SerializeField] private Canvas canvas;
	[SerializeField] private Toggle question5ToggleYes;
	[SerializeField] private Toggle signatureToggle;
	private GraphicRaycaster _graphicRaycaster;

	private Collider _collider;
	private Camera _cam;

	private bool _canInteract = true;


	private void Awake() {
		_playerCam = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>();
		_collider = GetComponent<Collider>();

		testCam.gameObject.SetActive(false);

		_cam = GameManager.Instance.MainCamera.GetComponent<Camera>();
		_graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
	}


	private void OnEnable() {
		signatureToggle.onValueChanged.AddListener(OnSignatureToggleChanged);
		InputManager.Instance.PickUp.performed += UiClick;
	}


	private void OnDisable() {
		signatureToggle.onValueChanged.RemoveListener(OnSignatureToggleChanged);
		InputManager.Instance.PickUp.performed -= UiClick;
	}


	public void Interact() {
		if (!_canInteract)
			return;

		testCam.gameObject.SetActive(true);
		_playerCam.gameObject.SetActive(false);

		_collider.enabled = false;
		_canInteract = false;

		canvas.worldCamera = _cam;
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject && _canInteract;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return _canInteract ? CrosshairType.Eye : CrosshairType.Default;
	}


	private void OnSignatureToggleChanged(bool isOn) {
		if (!isOn)
			return;

		//LoadEnding();
		ExitTest();
	}


	private void ExitTest() {
		testCam.gameObject.SetActive(false);
		_playerCam.gameObject.SetActive(true);

		_canInteract = false;
		canvas.worldCamera = null;
	}


	private async void LoadEnding() {
		if (question5ToggleYes.isOn)
			await WorldSceneManager.Instance.LoadScene(endingScene1);
		else
			await WorldSceneManager.Instance.LoadScene(endingScene2);
	}


	private void UiClick(InputAction.CallbackContext ctx) {
		if (!canvas.worldCamera)
			return;

		if (!_cam)
			return;

		if (!_graphicRaycaster)
			return;

		Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

		Plane canvasPlane = new Plane(canvas.transform.forward, canvas.transform.position);

		if (!canvasPlane.Raycast(ray, out float distance)) {
			return;
		}

		Vector3 worldPoint = ray.GetPoint(distance);

		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_cam, worldPoint);

		PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = screenPoint };

		List<RaycastResult> results = new List<RaycastResult>();

		_graphicRaycaster.Raycast(pointerData, results);

		foreach (RaycastResult result in results) {
			Toggle toggle = result.gameObject.GetComponentInParent<Toggle>();

			if (toggle && toggle.interactable) {

				ExecuteEvents.Execute(toggle.gameObject, pointerData, ExecuteEvents.pointerClickHandler);

				return;
			}
		}
	}


}