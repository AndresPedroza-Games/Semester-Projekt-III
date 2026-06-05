using UnityEngine;
using UnityEngine.InputSystem;

public class DoctorOfficePuzzleController : MonoBehaviour
{
    private EventSystemDoctorOffice _EventSystemDoctorOffice;
    private GameObject _Camera;
    private PlayerManager _PlayerManager;

    private Vector2 direction;

    private void Start()
    {
        _EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
        _EventSystemDoctorOffice.onInteractWithLock += InteractLock;

        _PlayerManager = PlayerManager.playerManager;

        _Camera = _PlayerManager.cinemachine.gameObject;
    }

    private void OnEnable()
    {
        InputManager.Instance.ExitLock.performed += ExitLock;

        InputManager.Instance.RotateLock.performed += RotateLock;
        InputManager.Instance.RotateLock.canceled += RotateLock;

        InputManager.Instance.ReleasePiece.performed += RelasePiece;
    }

    private void OnDisable()
    {
        InputManager.Instance.ExitLock.performed -= ExitLock;

        InputManager.Instance.RotateLock.performed -= RotateLock;
        InputManager.Instance.RotateLock.canceled -= RotateLock;

        InputManager.Instance.ReleasePiece.performed -= RelasePiece;
    }

    private void InteractLock()
    {
        _Camera.SetActive(false);
        _PlayerManager.FreezeCharacter(true);
    }

    private void ExitLock(InputAction.CallbackContext ctx)
    {
        _EventSystemDoctorOffice.ExitLock();
        _Camera.SetActive(true);
        _PlayerManager.FreezeCharacter(false);
    }

    private void RotateLock(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
        _EventSystemDoctorOffice.RotateLock(direction);
    }

    private void RelasePiece(InputAction.CallbackContext ctx)
    {
        _EventSystemDoctorOffice.ReleasePiece();
    }
}
