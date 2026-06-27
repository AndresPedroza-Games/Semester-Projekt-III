using System;
using UnityEngine;

public class EventSystemBathroom : EventSystemController
{
    public static EventSystemBathroom instance;
    private BathRoomController _BathRoomController;

    public Action onInteractValve;
    public Action onTurnOnShower;
    public Action onLockDoor;
    public Action onTakeScissors;
    public Action onCutHair;

    public Door door;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        door = FindFirstObjectByType<Door>(FindObjectsInactive.Include);
        door.freeze = true;
    }

    private void OnEnable()
    {
        _BathRoomController = FindFirstObjectByType<BathRoomController>(FindObjectsInactive.Include);
        _BathRoomController.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (_BathRoomController != null)
            _BathRoomController.gameObject.SetActive(false);
    }

    public void InteractValve()
    {
        onInteractValve?.Invoke();
    }

    public void TurnOnShower()
    {
        onTurnOnShower?.Invoke();
    }

    public void LockDoor()
    {
        onLockDoor?.Invoke();
    }

    public void TakeScissors()
    {
        onTakeScissors?.Invoke();
    }

    public void CutHair()
    {
        onCutHair?.Invoke();
    }
}
