using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private Transform _Head;

    private CinemachineVirtualCamera _CineMachine;
    private EventSystemChildRoom _EventSystemChildRoom;
    private PlayerManager _PlayerManager;

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _PlayerManager = PlayerManager.playerManager;
        _CineMachine = _PlayerManager.cinemachine;
    }

    private void OnEnable()
    {
        InputManager.Instance.PlacePiece.performed += PlacePiece;
        InputManager.Instance.ExitPuzzle.performed += ExitBoard;
        InputManager.Instance.RotatePiece.performed += RotatePiece;
    }

    private void OnDisable()
    {
        InputManager.Instance.PlacePiece.performed -= PlacePiece;
        InputManager.Instance.ExitPuzzle.performed -= ExitBoard;
        InputManager.Instance.RotatePiece.performed -= RotatePiece;
    }


    private void PlacePiece(InputAction.CallbackContext ctx)
    {
        _EventSystemChildRoom.PlacePiece();
    }

    private void RotatePiece(InputAction.CallbackContext ctx)
    {
        _EventSystemChildRoom.RotatePiece();
    }

    private void ExitBoard(InputAction.CallbackContext ctx)
    {
        if (FindFirstObjectByType<PlacementSystem>().isInteracting)
        {
            _EventSystemChildRoom.ExitBoard();
            _CineMachine.Follow = _Head;
            _CineMachine.LookAt = null;
            _CineMachine.m_Lens.FieldOfView = 50f;
            _PlayerManager.FreezeCharacter(false, 2f);
            Debug.Log("Exit");
        }
    }
}
