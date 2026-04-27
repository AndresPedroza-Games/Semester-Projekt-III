using UnityEngine;

public class Key : MonoBehaviour, IPickable
{
    public bool keyIsPicked;

    public void PickUp()
    {
        keyIsPicked = true;
        EventSystem.eventSystem.KeyPicked();
        Debug.Log("Key Picked");
    }
}
