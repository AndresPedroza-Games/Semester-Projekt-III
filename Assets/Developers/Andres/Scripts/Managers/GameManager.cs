using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour {

	private bool _MenuOpen = false;


	private void OnEnable() {
		InputManager.Instance.Pause.performed += PauseGame;
	}


	private void OnDisable() {
		InputManager.Instance.Pause.performed -= PauseGame;
	}


	private void Start() {
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