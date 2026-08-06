using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;


public class NarrativeNote : MonoBehaviour, IInteractable, ICrosshair {

	[Header("---Cam---")]
	[SerializeField] private CinemachineVirtualCamera noteCam;
	[SerializeField] private GameObject _Questions;

	[Header("---Animation---")]
	[SerializeField] private Ease _Transition;
	[SerializeField] private float _Duration;
	[SerializeField] private Transform _Hinge;


    private CinemachineVirtualCamera _playerCam;
	public bool isInteracting;
	public bool _CanExitInteraction;

	private bool _IsFlip;
	private Vector3 _Angle;

	private void Awake() {
		InputManager.Instance.ReadNote.Disable();
		
		gameObject.layer = LayerMask.NameToLayer("Interactable");

		_playerCam = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>();

		noteCam.gameObject.SetActive(false);
		noteCam.LookAt = transform;

		_CanExitInteraction = true;
    }


	private void OnEnable() {
		InputManager.Instance.ReadNote.performed += ExitNote;
	}


	private void OnDisable() {
		InputManager.Instance.ReadNote.performed -= ExitNote;
        InputManager.Instance.FlipNote.performed -= Flip;

        InputManager.Instance.ReadNote.Disable();
        GameManager.Instance.miniGameActive = false;
	}


	public void Interact() {

        if (!_CanExitInteraction)
            return;

        isInteracting = !isInteracting;

		if (!isInteracting)
			return;

		if (isInteracting) {
			SetInputMapsActive(false);
			InputManager.Instance.ReadNote.Enable();

			SyncSensitivity();

			noteCam.gameObject.SetActive(true);

			_playerCam.gameObject.SetActive(false);

			if (EventSystemTestRoom.instance != null)
			{
				TestNote();
            }
        }
	}


	private void ExitNote(InputAction.CallbackContext ctx) {

		if (!_CanExitInteraction)
			return;

		InputManager.Instance.ReadNote.Disable();
		SetInputMapsActive(true);
		
		noteCam.gameObject.SetActive(false);

		_playerCam.gameObject.SetActive(true);

        isInteracting = false;
	}

	private void Flip(InputAction.CallbackContext ctx)
	{
		_IsFlip = !_IsFlip;

        _Angle = _IsFlip ? new Vector3(90f,0f,0f) : new Vector3(-90f, 90f, 90f);

        _Hinge.DOLocalRotateQuaternion(Quaternion.Euler(_Angle), _Duration).SetEase(_Transition).SetLink(gameObject);
	}

	private void TestNote()
	{
        InputManager.Instance.Controls.Interaction.Enable();
        _CanExitInteraction = false;
        GameManager.Instance.miniGameActive = true;
        EventSystemTestRoom.instance.InteractPaper();
        _Questions.SetActive(true);
        EventSystemTestRoom.instance.onPuzzleSolved += PuzzleSolved;

        InputManager.Instance.FlipNote.performed += Flip;
    }

    private void PuzzleSolved()
	{
        InputManager.Instance.ReadNote.Disable();
        SetInputMapsActive(true);

        noteCam.gameObject.SetActive(false);

        _playerCam.gameObject.SetActive(true);

        isInteracting = false;

        GameManager.Instance.miniGameActive = false;

		InputManager.Instance.FlipNote.performed -= Flip;
    }

    private void SyncSensitivity() {
		noteCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = _playerCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
		noteCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = _playerCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
	}


	private void SetInputMapsActive(bool activate) {
		if (activate) {
			InputManager.Instance.Controls.Movement.Enable();
			InputManager.Instance.Controls.Interaction.Enable();
		}
		else {
			InputManager.Instance.Controls.Movement.Disable();
			InputManager.Instance.Controls.Interaction.Disable();
		}
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return isInteracting ? CrosshairType.Default : CrosshairType.Eye;
	}

}