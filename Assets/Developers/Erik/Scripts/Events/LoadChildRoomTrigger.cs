using System;
using UnityEngine;

public class LoadChildRoomTrigger : MonoBehaviour
{

	private void OnEnable() {
		EventSystemController.eventSystemController.onLoadChildsRoom += LoadChildsRoom;
	}


	private void OnDisable() {
		EventSystemController.eventSystemController.onLoadChildsRoom -= LoadChildsRoom;
	}


	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			EventSystemController.eventSystemController.LoadChildsRoom();
			gameObject.SetActive(false);
		}
	}


	private async void LoadChildsRoom() {
		await WorldSceneManager.Instance.LoadScene("1_ChildsRoom");
	}

}
