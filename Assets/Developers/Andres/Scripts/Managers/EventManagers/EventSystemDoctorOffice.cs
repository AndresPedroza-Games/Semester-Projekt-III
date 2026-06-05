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

    private DoctorOfficePuzzleController _DoctorOfficePuzzleController;

    private void Awake()
    {
        if (instace == null)
            instace = this;
    }

    private void OnEnable()
    {
        _DoctorOfficePuzzleController = FindFirstObjectByType<DoctorOfficePuzzleController>(FindObjectsInactive.Include);
        _DoctorOfficePuzzleController.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _DoctorOfficePuzzleController.gameObject.SetActive(false);
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
