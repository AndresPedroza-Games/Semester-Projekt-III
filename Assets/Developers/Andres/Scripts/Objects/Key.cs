using UnityEngine;

public class Key : MonoBehaviour, IPickable, IInteractable
{
    public bool keyIsPicked;

    public void PickUp()
    {
        keyIsPicked = true;
        EventSystem.eventSystem.KeyPicked();
        Debug.Log("Key Picked");
    }

    public void Interact() {
        keyIsPicked = true;
        EventSystem.eventSystem.KeyPicked();
    }
}
