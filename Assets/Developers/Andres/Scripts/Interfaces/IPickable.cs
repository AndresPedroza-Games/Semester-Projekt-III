using UnityEngine;


public interface IPickable {

	public void PickUp(Interactor interactor);
	public void Drop();
	public GameObject GetGameObject();

}