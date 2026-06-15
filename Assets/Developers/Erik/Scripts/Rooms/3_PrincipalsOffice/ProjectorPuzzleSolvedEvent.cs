using System;
using UnityEngine;

public class ProjectorPuzzleSolvedEvent : MonoBehaviour {

	[Header("---On Event---")]
	[SerializeField] private GameObject doorToEnableOnSolved;
	

	private void OnEnable() {
		EventSystemPrincipalsOffice.Instance.onPuzzleSolved += EnableDoor;
	}


	private void OnDisable() {
		EventSystemPrincipalsOffice.Instance.onPuzzleSolved -= EnableDoor;
	}


	private void EnableDoor(bool solved) {
		doorToEnableOnSolved.SetActive(solved);
	}

}
