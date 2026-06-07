using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour {

	[SerializeField] private GameObject playerSetup;
	[SerializeField] private GameObject player;

	private bool _MenuOpen = false;


	private void OnEnable() {
		InputManager.Instance.Pause.performed += PauseGame;
		EventSystemController.Instance.onStartGame += SetupGameStart;
	}


	private void OnDisable() {
		InputManager.Instance.Pause.performed -= PauseGame;
		EventSystemController.Instance.onStartGame -= SetupGameStart;
	}


	private void SetupGameStart() {
		InputManager.Instance.Pause.Enable();

		HideCursor();

		playerSetup.SetActive(true);
		player.SetActive(true);
	}


	private void HideCursor() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		InputManager.Instance.UI.Disable();
	}


	private void ShowCursor() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		InputManager.Instance.UI.Enable();
	}


	private void PauseGame(InputAction.CallbackContext ctx) {
		if (PauseMenu.pauseMenu == null) return;

		_MenuOpen = !_MenuOpen;
		PauseMenu.pauseMenu.ShowMenu(_MenuOpen);
		FreezeCharacter(_MenuOpen);

		EventSystemController.Instance.PauseGame();
	}


	private void FreezeCharacter(bool status) {
		if (status) {
			Time.timeScale = 0;

			InputManager.Instance.Controls.Movement.Disable();
			InputManager.Instance.Controls.Interaction.Disable();

			ShowCursor();
		}
		else {
			HideCursor();

			Time.timeScale = 1;

			InputManager.Instance.Controls.Movement.Enable();
			InputManager.Instance.Controls.Interaction.Enable();
		}
	}

}