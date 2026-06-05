using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour {

	public static InputManager Instance;

	public PlayerControls Controls { get; private set; }

    [Header("Mechanics")]
    public InputAction Move => Controls.Movement.Move;
	public InputAction Interact => Controls.Interaction.Interact;
	public InputAction PickUp => Controls.Interaction.PickUp;
	public InputAction Crouch => Controls.Movement.Crouch;
	public InputAction Zoom => Controls.Interaction.Zoom;

	[Header("Board Puzzle")]
	public InputAction PlacePiece => Controls.BoardPuzzle.Place;
	public InputAction ExitPuzzle => Controls.BoardPuzzle.Exit;
	public InputAction RotatePiece => Controls.BoardPuzzle.Rotate;

	[Header("Doctor Office Puzzle")]

	public InputAction ExitLock => controls.DoctorOfficePuzzle.Exit;
    public InputAction RotateLock => controls.DoctorOfficePuzzle.RotatePiece;
    public InputAction ReleasePiece => controls.DoctorOfficePuzzle.ReleasePiece;

    [Header("Settings")]
    public InputAction Pause => Controls.Game.Pause;


	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}
		else {
			Instance = this;
		}

		Controls ??= new PlayerControls();
		Controls.Enable();
	}

}