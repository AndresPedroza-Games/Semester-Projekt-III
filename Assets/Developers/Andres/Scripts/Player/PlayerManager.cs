using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerMotor _PlayerMotor;

    private bool _MenuOpen = false;

    private void Start()
    {
        _PlayerMotor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        _PlayerMotor.Crouch();

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    private void FixedUpdate()
    {
        _PlayerMotor.Movement();
    }

    private void PauseGame()
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
