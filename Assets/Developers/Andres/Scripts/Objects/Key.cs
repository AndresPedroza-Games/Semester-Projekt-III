using UnityEngine;


public class Key : Holdable, IKey {

	public void UseKey() {
		transform.SetParent(null);
		gameObject.SetActive(false);
	}

}