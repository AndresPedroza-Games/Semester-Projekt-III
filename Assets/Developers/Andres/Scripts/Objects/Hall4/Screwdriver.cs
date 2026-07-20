public class Screwdriver : SocketItem
{
    public override void UseItem()
    {
	    Release();
	    gameObject.SetActive(false);
    }
}
