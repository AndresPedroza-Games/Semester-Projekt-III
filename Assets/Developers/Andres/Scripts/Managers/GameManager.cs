using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour {

	[SerializeField] private GameObject playerSetup;
	[SerializeField] private GameObject player;
	[SerializeField] private GameObject _Camera;

	private bool _MenuOpen = false;

	public static bool miniGameActive;


    private void OnEnable() {
		InputManager.Instance.Pause.performed += PauseGame;
        EventSystemController.Instance.onStartGame += SetupGameStart;
        EventSystemController.Instance.onResumeGame += ResumeGame;
    }


	private void OnDisable() {
		InputManager.Instance.Pause.performed -= PauseGame;
		EventSystemController.Instance.onStartGame -= SetupGameStart;
        EventSystemController.Instance.onResumeGame -= ResumeGame;
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

		if (!_MenuOpen && !miniGameActive)
		{
            EventSystemController.Instance.PauseGame();
            _MenuOpen = !_MenuOpen;
            FreezeCharacter(_MenuOpen);
			return;
        }
		else if(_MenuOpen && !miniGameActive)
            EventSystemController.Instance.ResumeGame();
    }

    private void ResumeGame()
	{
        _MenuOpen = !_MenuOpen;
        FreezeCharacter(_MenuOpen);

		SetupGameStart();
    }


	private void FreezeCharacter(bool status) {
		if (status) {

			InputManager.Instance.Controls.Movement.Disable();
			InputManager.Instance.Controls.Interaction.Disable();

			ShowCursor();
			_Camera.GetComponent<CinemachineInputProvider>().enabled = !status;

        }
		else {
			HideCursor();

			InputManager.Instance.Controls.Movement.Enable();
			InputManager.Instance.Controls.Interaction.Enable();
            _Camera.GetComponent<CinemachineInputProvider>().enabled = !status;
        }
	}

}