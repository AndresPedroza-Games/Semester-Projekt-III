public class Key : SocketItem
{
    public override void UseItem()
    {
        Release();
        gameObject.SetActive(false);
    }


    public override bool CanInteract(HoldController holdController) {
	    return false;
    }

}
