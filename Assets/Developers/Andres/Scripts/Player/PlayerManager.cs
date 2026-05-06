using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerMotor _PlayerMotor;

    private bool _GameStatus;

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
        _GameStatus = !_GameStatus;
        FreezeCharacter(_GameStatus);
    }

    private void FreezeCharacter(bool status)
    {
        this.enabled = !status;
    }
}
