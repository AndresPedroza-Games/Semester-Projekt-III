using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class FinalTest : MonoBehaviour, IInteractable, ICrosshair {

	[Header("---Twin---")]
	[SerializeField] private GameObject twin;
	[SerializeField] private float baseAlpha = 0.5f;
	[SerializeField] private float transparencyStep = 0.1f;
	private Material _twinMaterial;

	[Header("---Ending Scenes---")]
	[SerializeField] private SceneReference endingScene1;
	[SerializeField] private SceneReference endingScene2;

	[Header("---Door To Endings---")]
	[SerializeField] private Door door;

	[Header("---Cam---")]
	[SerializeField] private CinemachineVirtualCamera testCam;
	private CinemachineVirtualCamera _playerCam;

	[Header("---Canvas---")]
	[SerializeField] private Canvas canvas;
	[SerializeField] private Toggle question5ToggleYes;
	[SerializeField] private Toggle signatureToggle;
	private GraphicRaycaster _graphicRaycaster;

	[Header("---Toggles---")]
	[SerializeField] private List<ToggleGroup> toggleGroups;
	[SerializeField] private List<Toggle> correctToggles;

	private Collider _collider;
	private Camera _cam;

	private bool _canInteract = true;


	private void Awake() {
		_playerCam = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>();
		_collider = GetComponent<Collider>();

		testCam.gameObject.SetActive(false);

		_cam = GameManager.Instance.MainCamera.GetComponent<Camera>();
		_graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();

		_twinMaterial = twin.GetComponent<Renderer>().material;
		Color color = _twinMaterial.color;
		color.a = baseAlpha;
		_twinMaterial.color = color;

		signatureToggle.interactable = false;

		twin.SetActive(false);
	}


	private void OnEnable() {
		signatureToggle.onValueChanged.AddListener(OnSignatureToggleChanged);
		InputManager.Instance.PickUp.performed += UiClick;
	}


	private void OnDisable() {
		signatureToggle.onValueChanged.RemoveListener(OnSignatureToggleChanged);
		InputManager.Instance.PickUp.performed -= UiClick;
	}


	private void EnterNote() {
		testCam.gameObject.SetActive(true);
		_playerCam.gameObject.SetActive(false);

		_collider.enabled = false;
		_canInteract = false;

		canvas.worldCamera = _cam;

		twin.SetActive(true);
	}


	private void ExitNote() {
		testCam.gameObject.SetActive(false);
		_playerCam.gameObject.SetActive(true);

		_canInteract = false;
		canvas.worldCamera = null;

		door.OpenDoorSimple();
	}


	public void UpdateTwinMaterial() {
		float alpha = baseAlpha;

		foreach (ToggleGroup toggleGroup in toggleGroups) {
			Toggle selectedToggle = toggleGroup.GetFirstActiveToggle();

			if (!selectedToggle)
				continue;

			if (correctToggles.Contains(selectedToggle)) {
				alpha += transparencyStep;
			}
			else {
				alpha -= transparencyStep;
			}
		}

		alpha = Mathf.Clamp01(alpha);

		Color color = _twinMaterial.color;
		color.a = alpha;
		_twinMaterial.color = color;
	}


	public void Interact() {
		if (!_canInteract)
			return;

		EnterNote();
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

		if (!AllTogglesSet())
			return;

		LoadEnding(question5ToggleYes.isOn ? endingScene1 : endingScene2);

		ExitNote();
		twin.SetActive(false);
	}


	private bool AllTogglesSet() {
		foreach (ToggleGroup toggleGroup in toggleGroups) {
			if (!toggleGroup.AnyTogglesOn()) {
				return false;
			}
		}

		return true;
	}


	public void CheckIfAllTogglesAreSet() {
		if (!AllTogglesSet()) {
			if (signatureToggle.interactable)
				signatureToggle.interactable = false;
			return;
		}

		signatureToggle.interactable = true;
	}


	private async void LoadEnding(SceneReference endingScene) {
		await WorldSceneManager.Instance.LoadScene(endingScene);
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