using System;
using UnityEngine;

public class EventSystemHall4 : EventSystemController
{
    public static EventSystemHall4 instance;

    public Action onStart;
    public Action onPressPlate;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        OnStart();
    }

    public void OnStart()
    {
        onStart?.Invoke();
    }

    public void PressPlate()
    {
        onPressPlate?.Invoke();
    }
}
