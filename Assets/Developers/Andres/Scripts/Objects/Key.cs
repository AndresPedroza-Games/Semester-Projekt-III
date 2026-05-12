using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public bool keyIsPicked;

    public void PickUp()
    {
        keyIsPicked = true;
        EventSystem.eventSystem.KeyPicked();
        Debug.Log("Key Picked");
    }

    public void Interact(Interactor interactor) {
        keyIsPicked = true;
        EventSystem.eventSystem.KeyPicked();
    }
}
