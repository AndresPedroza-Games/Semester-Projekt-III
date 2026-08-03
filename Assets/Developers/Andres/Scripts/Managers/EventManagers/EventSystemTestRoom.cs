using System;
using UnityEngine;

public class EventSystemTestRoom : EventSystemController
{
    public static EventSystemTestRoom instance;

    public Action onInteractionWithPaper;
    public Action<bool> onCrossAnswer;
    public Action onPuzzleSolved;
    public Action onRestartPuzzle;

    public GameObject twinShadow;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void InteractPaper()
    {
        onInteractionWithPaper?.Invoke();
        twinShadow.SetActive(true);
    }

    public void CrossAnswer(bool answer)
    {
        onCrossAnswer?.Invoke(answer);
    }

    public void PuzzleSolved()
    {
        onPuzzleSolved?.Invoke();
    }

    public void RestartPuzzle()
    {
        onRestartPuzzle?.Invoke();
    }
}
