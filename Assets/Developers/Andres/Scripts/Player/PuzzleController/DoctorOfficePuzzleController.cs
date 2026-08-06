using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class DoctorOfficePuzzleController : MonoBehaviour
{
    private EventSystemDoctorOffice _EventSystemDoctorOffice;
    private GameObject _Camera;
    private PlayerManager _PlayerManager;

    private Vector2 direction;
    private bool _IsActive;

    private void Start()
    {
        _EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
        _EventSystemDoctorOffice.onInteractWithLock += InteractLock;
        _EventSystemDoctorOffice.onPuzzleCompleted += ExitLockOnComplete;

        _PlayerManager = PlayerManager.playerManager;

        _Camera = _PlayerManager.cinemachine.gameObject;
    }

    private void OnEnable()
    {
        InputManager.Instance.ExitLock.performed += ExitLock;

        InputManager.Instance.RotateLock.performed += RotateLock;
        InputManager.Instance.RotateLock.canceled += RotateLock;
    }

    private void OnDisable()
    {
        InputManager.Instance.ExitLock.performed -= ExitLock;

        InputManager.Instance.RotateLock.performed -= RotateLock;
        InputManager.Instance.RotateLock.canceled -= RotateLock;
    }

    private void InteractLock()
    {
        _Camera.SetActive(false);
        _PlayerManager.FreezeCharacter(true);
        
        StartCoroutine(SetActive(true));
    }

    private void ExitLock(InputAction.CallbackContext ctx)
    {
        if (!_IsActive)
            return;

        _EventSystemDoctorOffice.ExitLock();
        _Camera.SetActive(true);
        _PlayerManager.FreezeCharacter(false);

        StartCoroutine(SetActive(false));
    }

    private void RotateLock(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
        _EventSystemDoctorOffice.RotateLock(direction);
    }

    
    private void ExitLockOnComplete()
    {
	    _EventSystemDoctorOffice.ExitLock();
	    _Camera.SetActive(true);
	    _PlayerManager.FreezeCharacter(false);
        
	    InputManager.Instance.Pause.Enable();
    }

    private IEnumerator SetActive(bool status)
    {
        yield return new WaitForSeconds(0.5f);
        _IsActive = status;
        GameManager.Instance.miniGameActive = status;
    }
}
