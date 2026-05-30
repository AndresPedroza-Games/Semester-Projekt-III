using UnityEngine;


public interface IHoldable {
	public GameObject GameObject();
	public void Hold(HoldController holder);
	public void Release();

}