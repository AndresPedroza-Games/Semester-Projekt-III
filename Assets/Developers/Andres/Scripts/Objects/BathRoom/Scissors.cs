
public class Scissors : SocketItem
{
    private EventSystemBathroom _EventSystemBathroom;

    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;
        _EventSystemBathroom.onCutHair += UseItem;
        _EventSystemBathroom.onLockDoor += () => canBeHold = true;

        canBeHold = false;
    }

    public override void Hold(HoldController holder)
    {
        if (!canBeHold)
            return;

        base.Hold(holder);
        _EventSystemBathroom.TakeScissors();

    }

    public override void UseItem()
    {
        Release();
        gameObject.SetActive(false);
    }

}
