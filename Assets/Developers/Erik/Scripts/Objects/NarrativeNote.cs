using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class NarrativeNote : MonoBehaviour, IInteractable {

	[Header("---Cam---")]
	[SerializeField] private CinemachineVirtualCamera noteCam;

	private CinemachineVirtualCamera _playerCam;
	private bool _isInteracting;


	private void Awake() {
		InputManager.Instance.ReadNote.Disable();

		_playerCam = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>();

		noteCam.gameObject.SetActive(false);
		noteCam.LookAt = transform;
	}


	private void OnEnable() {
		InputManager.Instance.ReadNote.performed += ExitNote;
	}


	private void OnDisable() {
		InputManager.Instance.ReadNote.performed -= ExitNote;

		InputManager.Instance.ReadNote.Disable();
	}


	public void Interact() {
		_isInteracting = !_isInteracting;

		if (!_isInteracting)
			return;

		if (_isInteracting) {
			SetInputMapsActive(false);
			InputManager.Instance.ReadNote.Enable();

			SyncSensitivity();

			noteCam.gameObject.SetActive(true);

			_playerCam.gameObject.SetActive(false);
		}
	}


	private void ExitNote(InputAction.CallbackContext ctx) {
		InputManager.Instance.ReadNote.Disable();
		SetInputMapsActive(true);
		
		noteCam.gameObject.SetActive(false);

		_playerCam.gameObject.SetActive(true);

		_isInteracting = false;
	}


	private void SyncSensitivity() {
		noteCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = _playerCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
		noteCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = _playerCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
	}


	private void SetInputMapsActive(bool activate) {
		if (activate) {
			InputManager.Instance.Controls.Movement.Enable();
			InputManager.Instance.Controls.Interaction.Enable();
			InputManager.Instance.Controls.Game.Enable();
		}
		else {
			InputManager.Instance.Controls.Movement.Disable();
			InputManager.Instance.Controls.Interaction.Disable();
			InputManager.Instance.Controls.Game.Disable();
		}
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return _isInteracting ? CrosshairType.Default : CrosshairType.Eye;
	}

}