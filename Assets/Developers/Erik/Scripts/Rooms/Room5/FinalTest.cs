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

	[Header("---Blocking Object Detection---")]
	[SerializeField] private Transform boxCenter;
	[SerializeField] private Vector3 halfExtends;
	private Collider[] _blockingObjects;

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
		testCam.gameObject.SetActive(false);
		if (_playerCam)
			_playerCam.gameObject.SetActive(true);
		InputManager.Instance.Controls.Movement.Enable();

		signatureToggle.onValueChanged.RemoveListener(OnSignatureToggleChanged);
		InputManager.Instance.PickUp.performed -= UiClick;
	}


	private void EnterNote() {
		InputManager.Instance.Controls.Movement.Disable();

		testCam.gameObject.SetActive(true);
		_playerCam.gameObject.SetActive(false);

		_collider.enabled = false;
		_canInteract = false;

		canvas.worldCamera = _cam;

		twin.SetActive(true);

		CheckForBlockingObjectsAndDisable();
	}


	private void ExitNote() {
		InputManager.Instance.Controls.Movement.Enable();

		testCam.gameObject.SetActive(false);
		_playerCam.gameObject.SetActive(true);

		_collider.enabled = true;

		_canInteract = false;
		canvas.worldCamera = null;

		door.OpenDoorSimple();

		EnableBlockingObjects();
	}


	private void CheckForBlockingObjectsAndDisable() {
		_blockingObjects = Physics.OverlapBox(boxCenter.position, halfExtends, Quaternion.identity);

		foreach (Collider col in _blockingObjects) {
			if (col.gameObject == this.gameObject)
				continue;

			col.gameObject.SetActive(false);
		}
	}


	private void EnableBlockingObjects() {
		foreach (Collider col in _blockingObjects) {
			col.gameObject.SetActive(true);
		}
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
		if (!canvas.worldCamera || !_cam || !_graphicRaycaster)
			return;

		Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

		PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = _cam.WorldToScreenPoint(ray.GetPoint(new Plane(canvas.transform.forward, canvas.transform.position).Raycast(ray, out float distance) ? distance : 0f)) };

		List<RaycastResult> results = new List<RaycastResult>();
		_graphicRaycaster.Raycast(pointerData, results);

		foreach (RaycastResult result in results) {
			Toggle toggle = result.gameObject.GetComponentInParent<Toggle>();

			if (toggle && toggle.interactable) {
				toggle.isOn = !toggle.isOn;
				return;
			}
		}
	}


#if UNITY_EDITOR

	private void OnDrawGizmos() {
		if (!boxCenter || halfExtends == Vector3.zero)
			return;

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(boxCenter.position, halfExtends * 2);
	}
#endif

}