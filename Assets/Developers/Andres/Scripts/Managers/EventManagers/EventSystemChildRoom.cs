using System;
using UnityEngine;

public class EventSystemChildRoom : EventSystemController
{
    public static EventSystemChildRoom eventSystemChildRoom;

    public Action onPiecePlaced;
    public Action<GameObject> onPiecePicked;
    public Action onInteractWithBoard;
    public Action onExitBoard;
    public Action<Vector2> onRotatePiece;
    public Action onPuzzleSolved;

    private PuzzleController _PuzzleController;

    private void Awake()
    {
        if (eventSystemChildRoom == null)
            eventSystemChildRoom = this;
    }

    private void OnEnable()
    {
        _PuzzleController = FindFirstObjectByType<PuzzleController>(FindObjectsInactive.Include);
        _PuzzleController.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if(_PuzzleController != null)
            _PuzzleController.gameObject.SetActive(false);
    }

    public void PlacePiece()
    {
        if (onPiecePlaced != null)
            onPiecePlaced.Invoke();
    }

    public void InteractWithBoard()
    {
        if (onInteractWithBoard != null)
            onInteractWithBoard.Invoke();
    }

    public void PickPiece(GameObject piece)
    {
        if (onPiecePicked != null)
            onPiecePicked.Invoke(piece);
    }

    public void RotatePiece(Vector2 scroll)
    {
        if (onRotatePiece != null)
            onRotatePiece.Invoke(scroll);
    }

    public void ExitBoard()
    {
        if (onExitBoard != null)
            onExitBoard.Invoke();
    }

    public void PuzzleSolved()
    {
        if (onPuzzleSolved != null)
            onPuzzleSolved.Invoke();
    }
}
