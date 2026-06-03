using UnityEngine;


public class Key : MonoBehaviour, IKey {

	public void UseKey() {
		transform.SetParent(null);
		gameObject.SetActive(false);
	}

}