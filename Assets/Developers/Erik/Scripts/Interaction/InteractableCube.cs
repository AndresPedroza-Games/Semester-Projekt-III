using UnityEngine;

public class InteractableCube : MonoBehaviour, IInteractable
{
    bool moved = false;
    public void Interact() {
        moved = !moved;
        transform.position += moved ? Vector3.up : Vector3.down; ;
    }

    
}
