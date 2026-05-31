using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour {

	[SerializeField] private GameObject playerSetup;
	[SerializeField] private GameObject player;
	
	
	private bool _MenuOpen = false;


	private void OnEnable() {
		InputManager.Instance.Pause.performed += PauseGame;
		EventSystemController.eventSystemController.onStartGame += HideCursor;
		EventSystemController.eventSystemController.onStartGame += ActivatePlayer;
	}


	private void OnDisable() {
		InputManager.Instance.Pause.performed -= PauseGame;
		EventSystemController.eventSystemController.onStartGame -= HideCursor;
		EventSystemController.eventSystemController.onStartGame -= ActivatePlayer;
	}


	private void ActivatePlayer() {
		playerSetup.SetActive(true);
		player.SetActive(true);
	}


	private void HideCursor() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}



	private void PauseGame(InputAction.CallbackContext ctx) {
		if (PauseMenu.pauseMenu == null) return;

		_MenuOpen = !_MenuOpen;
		PauseMenu.pauseMenu.ShowMenu(_MenuOpen);
		FreezeCharacter(_MenuOpen);
	}


	private void FreezeCharacter(bool status) {
		//this.enabled = !status;
	}

}