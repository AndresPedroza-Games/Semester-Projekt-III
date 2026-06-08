using System;
using UnityEngine;


public class LoadWallTrigger : MonoBehaviour {

	[SerializeField] private GameObject wallToEnable;


	private void Awake() {
		if (wallToEnable.activeSelf)
			wallToEnable.SetActive(false);
	}


	private void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		wallToEnable.SetActive(true);

		gameObject.SetActive(false);
	}

}