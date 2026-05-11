using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    private PlayerControls Controls;

    public InputAction Move => Controls.Movement.Move;
    public InputAction Interact => Controls.Interaction.Interact;
    public InputAction Crouch => Controls.Movement.Crouch;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
