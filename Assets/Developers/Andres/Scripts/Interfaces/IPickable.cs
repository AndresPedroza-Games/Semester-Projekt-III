using UnityEngine;

public interface IPickable
{
	public void PickUp(Inventory inventory);
	public void Drop(Inventory inventory);
}
