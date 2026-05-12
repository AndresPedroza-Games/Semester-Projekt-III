using UnityEngine;


public class InteractableCube : MonoBehaviour, IInteractable {

	private bool moved;


	public void Interact(Interactor interactor) {
		moved = !moved;
		transform.position += moved ? Vector3.up : Vector3.down;
	}

}