using System;
using UnityEngine;

public class EventSystemTestRoom : EventSystemController
{
    public static EventSystemTestRoom instance;

    public Action onInteractionWithPaper;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void InteractPaper()
    {
        onInteractionWithPaper?.Invoke();
    }
}
