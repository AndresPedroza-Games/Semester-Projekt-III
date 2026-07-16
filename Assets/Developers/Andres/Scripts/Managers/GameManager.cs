using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }
	public string LastScene { get; set; }

	[SerializeField] private GameObject playerSetup;
	[SerializeField] private GameObject player;
	public GameObject Player => player;
	[field: SerializeField] public GameObject Camera { get; private set; }

	[field: SerializeField] public Interactor Interactor { get; private set; }

	private bool _MenuOpen = false;

	public bool miniGameActive;


	private void Awake() {
		if (Instance && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
		miniGameActive = false;
	}


	private void OnEnable() {
		InputManager.Instance.Pause.performed += PauseGame;
		EventSystemController.Instance.onStartGame += SetupGameStart;
		EventSystemController.Instance.onResumeGame += ResumeGame;
		EventSystemController.Instance.onMainMenuEntered += OnMainMenuEntered;
	}


	private void OnDisable() {
		InputManager.Instance.Pause.performed -= PauseGame;
		EventSystemController.Instance.onStartGame -= SetupGameStart;
		EventSystemController.Instance.onResumeGame -= ResumeGame;
		EventSystemController.Instance.onMainMenuEntered -= OnMainMenuEntered;
	}


	private void SetupGameStart() {
		InputManager.Instance.Pause.Enable();
		Time.timeScale = 1f;

		HideCursor();

		playerSetup.SetActive(true);
		player.SetActive(true);
		
		FreezeCharacter(false);
		_MenuOpen = false;
	}


	private void OnMainMenuEntered() {
		InputManager.Instance.Pause.Disable();
		
		playerSetup.SetActive(false);
		player.SetActive(false);
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

		if (!_MenuOpen) {
			EventSystemController.Instance.PauseGame();
			_MenuOpen = !_MenuOpen;
			FreezeCharacter(_MenuOpen);
			Time.timeScale = 0f;
			return;
		}
		else
			EventSystemController.Instance.ResumeGame();
	}


	private void ResumeGame() {
		_MenuOpen = !_MenuOpen;
		FreezeCharacter(_MenuOpen);
		Time.timeScale = 1f;

		InputManager.Instance.Pause.Enable();
		HideCursor();
	}


	private void FreezeCharacter(bool status) {
		if (status) {

			InputManager.Instance.Controls.Movement.Disable();
			InputManager.Instance.Controls.Interaction.Disable();

			ShowCursor();
			Camera.GetComponent<CinemachineInputProvider>().enabled = !status;

		}
		else {
			HideCursor();

			InputManager.Instance.Controls.Movement.Enable();
			InputManager.Instance.Controls.Interaction.Enable();
			Camera.GetComponent<CinemachineInputProvider>().enabled = !status;
		}
	}

}