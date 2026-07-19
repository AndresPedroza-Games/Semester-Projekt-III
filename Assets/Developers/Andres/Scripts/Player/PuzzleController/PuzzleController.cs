using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleController : MonoBehaviour
{
    private PlayerManager _PlayerManager;
    private InteractionDetector _InteractionDetector;

    private float _StartInteractionDistance;

    private void Start()
    {
        _PlayerManager = PlayerManager.playerManager;
    }

    private void OnEnable()
    {
        InputManager.Instance.PlacePiece.performed += PlacePiece;
        InputManager.Instance.RotatePiece.performed += RotatePiece;

        _InteractionDetector = FindAnyObjectByType<InteractionDetector>(FindObjectsInactive.Include);
        _StartInteractionDistance = _InteractionDetector.interactionDistance;
    }

    private void OnDisable()
    {
        InputManager.Instance.PlacePiece.performed -= PlacePiece;
        InputManager.Instance.RotatePiece.performed -= RotatePiece;
    }


    private void PlacePiece(InputAction.CallbackContext ctx)
    {
        EventSystemChildRoom.eventSystemChildRoom?.PlacePiece();
    }

    private void RotatePiece(InputAction.CallbackContext ctx)
    {
        Vector2 scroll = ctx.ReadValue<Vector2>();

        EventSystemChildRoom.eventSystemChildRoom?.RotatePiece(scroll);
    }
}
