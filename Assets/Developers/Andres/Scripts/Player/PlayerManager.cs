using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerMotor _PlayerMotor;

    private bool _MenuOpen = false;

    private void OnEnable() {
        InputManager.Instance.Pause.performed += PauseGame;
    }


    private void OnDisable() {
        InputManager.Instance.Pause.performed -= PauseGame;
    }

    private void Start()
    {
        _PlayerMotor = GetComponent<PlayerMotor>();
    }

    private void Update() {
        _PlayerMotor.Movement();
        _PlayerMotor.HandleGravity();
    }

    private void PauseGame(InputAction.CallbackContext ctx)
    {
        _MenuOpen = !_MenuOpen;
        PauseMenu.pauseMenu.ShowMenu(_MenuOpen);
        FreezeCharacter(_MenuOpen);
    }

    private void FreezeCharacter(bool status)
    {
        //this.enabled = !status;
    }
}
