using UnityEngine;


public interface IHoldable {
	public GameObject GameObject();
	public void Hold(HoldController holder, Vector3 hitPoint);
	public void Release();

}