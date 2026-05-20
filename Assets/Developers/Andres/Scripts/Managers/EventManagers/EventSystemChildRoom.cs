using System;
using UnityEngine;

public class EventSystemChildRoom : EventSystemController
{
    public static EventSystemChildRoom eventSystemChildRoom;

    public Action onPiecePlaced;
    public Action<GameObject> onPiecePicked;
    public Action<Transform, Transform> onInteractWithBoard;
    public Action onExitBoard;
    public Action onRotatePiece;
    public Action onPuzzleSolved;

    private void Awake()
    {
        if (eventSystemChildRoom == null)
            eventSystemChildRoom = this;
    }

    public void PlacePiece()
    {
        if (onPiecePlaced != null)
            onPiecePlaced.Invoke();
    }

    public void InteractWithBoard(Transform cameraPos, Transform board)
    {
        if (onInteractWithBoard != null)
            onInteractWithBoard.Invoke(cameraPos, board);
    }

    public void PickPiece(GameObject piece)
    {
        if (onPiecePicked != null)
            onPiecePicked.Invoke(piece);
    }

    public void RotatePiece()
    {
        if (onRotatePiece != null)
            onRotatePiece.Invoke();
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
