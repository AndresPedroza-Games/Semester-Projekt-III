using System;

public class EventSystemTestRoom : EventSystemController
{
    public static EventSystemTestRoom instance;

    public Action onInteractionWithPaper;
    public Action onCrossAnswer;
    public Action onPuzzleSolved;
    public Action onRestartPuzzle;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void InteractPaper()
    {
        onInteractionWithPaper?.Invoke();
    }

    public void CrossAnswer()
    {
        onCrossAnswer?.Invoke();
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
