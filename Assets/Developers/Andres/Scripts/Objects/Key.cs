using UnityEngine;

public class Key : SocketItem
{
    public override void UseItem()
    {
        Release();
        gameObject.SetActive(false);
    }
}
