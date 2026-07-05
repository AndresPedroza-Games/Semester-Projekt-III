using UnityEngine;

public class Screwdriver : SocketItem
{
    public override void UseItem()
    {
        gameObject.SetActive(false);
        base.UseItem();
    }
}
