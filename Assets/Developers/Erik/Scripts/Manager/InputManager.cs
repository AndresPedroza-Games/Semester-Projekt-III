using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour {

	public static InputManager Instance;
	public PlayerControls Controls { get; private set; }

	
	[Header("Mechanics")]
	public InputAction Move => Controls.Movement.Move;
	public InputAction Look => Controls.Movement.Look;
	public InputAction Interact => Controls.Interaction.Interact;
	public InputAction InteractElementPuzzle => Controls.Interaction.InteractElementPuzzle;
	public InputAction PickUp => Controls.Interaction.PickUp;
	public InputAction Crouch => Controls.Movement.Crouch;
	public InputAction Zoom => Controls.Interaction.Zoom;

	
	[Header("UI")]
	public InputActionMap UI => Controls.UI;

	
	[Header("Board Puzzle")]
	public InputAction PlacePiece => Controls.BoardPuzzle.Place;
	public InputAction RotatePiece => Controls.BoardPuzzle.Rotate;
	
	[Header("Doctor Office Puzzle")]

	public InputAction ExitLock => Controls.DoctorOfficePuzzle.Exit;
	public InputAction RotateLock => Controls.DoctorOfficePuzzle.RotatePiece;
	public InputAction ReleasePiece => Controls.DoctorOfficePuzzle.ReleasePiece;
	
	[Header("Narrative Notes")]
	public InputAction ReadNote => Controls.NarrativeNotes.ReadNote;
	public InputAction FlipNote => Controls.NarrativeNotes.Flip;

	
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