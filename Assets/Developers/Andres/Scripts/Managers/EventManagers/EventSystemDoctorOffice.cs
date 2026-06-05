using System;
using UnityEngine;

public class EventSystemDoctorOffice : EventSystemController
{
    public static EventSystemDoctorOffice instace;

    public Action onInteractWithLock;
    public Action onEndInteractionWithLock;
    public Action onPuzzleCompleted;

    public Action onReleasePiece;
    public Action<Vector2> onRotateLock;

    private void Awake()
    {
        if (instace == null)
            instace = this;
    }

    public void InteractWithLock()
    {
        onInteractWithLock?.Invoke();
    }

    public void ExitLock()
    {
        onEndInteractionWithLock?.Invoke();
    }

    public void RotateLock(Vector2 direction)
    {
        onRotateLock?.Invoke(direction);
    }

    public void ReleasePiece()
    {
        onReleasePiece?.Invoke();
    }

    public void PuzzleCompleted()
    {
        onPuzzleCompleted?.Invoke();
    }
}
