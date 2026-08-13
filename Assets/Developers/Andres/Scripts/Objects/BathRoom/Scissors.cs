
using UnityEngine;


public class Scissors : SocketItem
{
    private EventSystemBathroom _EventSystemBathroom;

    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;
        _EventSystemBathroom.onLockDoor += () => canBeHold = true;

        canBeHold = false;
    }

    public override void Hold(HoldController holder, Vector3 hitPoint)
    {
        if (!canBeHold)
            return;

        base.Hold(holder, hitPoint);
        _EventSystemBathroom.TakeScissors();

    }

    public override void UseItem()
    {
        Release();
        gameObject.SetActive(false);
    }

}
