using UnityEngine;

public class Scissors : SocketItem
{
    private EventSystemBathroom _EventSystemBathroom;

    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;
        _EventSystemBathroom.onCutHair += UseItem;
        _EventSystemBathroom.onLockDoor += () => CanBeHold = true;

        CanBeHold = false;
    }

    public override void Interact()
    {
        _EventSystemBathroom.TakeScissors();
    }

    public override void UseItem()
    {
        Release();
        gameObject.SetActive(false);
    }

}
