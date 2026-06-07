using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleController : MonoBehaviour
{
    private GameObject _CineMachine;
    private EventSystemChildRoom _EventSystemChildRoom;
    private PlayerManager _PlayerManager;
    private InteractionDetector _InteractionDetector;

    private float _StartInteractionDistance;

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

        _InteractionDetector = FindAnyObjectByType<InteractionDetector>(FindObjectsInactive.Include);
        _StartInteractionDistance = _InteractionDetector.interactionDistance;
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
        if (PlacementSystem.isInteracting)
        {
            _EventSystemChildRoom.ExitBoard();
            _PlayerManager.FreezeCharacter(false);

            _CineMachine.SetActive(true);
            _InteractionDetector.interactionDistance = _StartInteractionDistance;

            Debug.Log("Exit");
        }
    }
}
