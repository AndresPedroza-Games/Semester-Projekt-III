using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    private PlayerControls Controls;

    public Vector2 MoveInput => Controls.Movement.Move.ReadValue<Vector2>();
    public InputAction Interact => Controls.Interaction.Interact;
    public InputAction Crouch => Controls.Movement.Crouch;



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
