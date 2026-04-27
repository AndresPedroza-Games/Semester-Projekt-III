using System;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem eventSystem;

    public Action openDoor;
    public Action keyPicked;

    private void Awake()
    {
        if(eventSystem == null)
            eventSystem = this;
    }

    public void OpenDoor()
    {
        if (openDoor != null)
            openDoor.Invoke();
    }

    public void KeyPicked()
    {
        if (keyPicked != null)
            keyPicked.Invoke();
    }
}
