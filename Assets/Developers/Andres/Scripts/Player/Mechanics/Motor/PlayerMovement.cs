using UnityEngine;

public class PlayerMovement
{
    private PlayerMotor _PlayerMotor;
    private Rigidbody _Rb;

    private float playerSpeed;
    private float playerCrouchSpeed;
    private float currentSpeed;

    public void Init()
    {
        _PlayerMotor = PlayerMotor.playerMotor;

        _Rb = _PlayerMotor.rb;

        playerSpeed = _PlayerMotor.playerSpeed;
        playerCrouchSpeed = _PlayerMotor.playerCrouchSpeed;
        currentSpeed = _PlayerMotor.currentSpeed;
    }

    public void Movement()
    {
        currentSpeed = !_PlayerMotor.isCrouching ? playerSpeed : playerCrouchSpeed;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        _Rb.linearVelocity = new Vector3(moveX, 0f, moveY) * currentSpeed * Time.deltaTime;
    }
}
