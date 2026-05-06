using UnityEngine;

public class Door : MonoBehaviour, IOpenable, IInteractable
{
    private bool _DoorIsOpenAble;

    private void Start()
    {
        EventSystem.eventSystem.openDoor += Open;
        EventSystem.eventSystem.keyPicked += () => _DoorIsOpenAble = true;
    }

    public void Open()
    {
        if (_DoorIsOpenAble)
            Debug.Log("Opening Door");
        else
            Debug.Log("You don't have the key");
    }

    public void Close()
    {

    }

    public void Interact() {
        if (_DoorIsOpenAble)
            Debug.Log("Opening Door");
        else
            Debug.Log("You don't have the key");
    }
}
