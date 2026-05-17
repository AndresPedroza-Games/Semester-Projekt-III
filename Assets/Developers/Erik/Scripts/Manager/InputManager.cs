using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour {

	public static InputManager Instance;

	private PlayerControls controls;

	public InputAction Move => controls.Movement.Move;
	public InputAction Interact => controls.Interaction.Interact;
	public InputAction PickUp => controls.Interaction.PickUp;
	public InputAction Crouch => controls.Movement.Crouch;
	public InputAction Zoom => controls.Interaction.Zoom;

	public InputAction Pause => controls.Game.Pause;


	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}
		else {
			Instance = this;
		}

		controls ??= new PlayerControls();
		controls.Enable();
	}

}