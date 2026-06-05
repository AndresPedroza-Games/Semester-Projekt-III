using UnityEngine;


public class Key : Holdable, IKey {

	public void UseKey() {
		Release();
		gameObject.SetActive(false);
	}

}