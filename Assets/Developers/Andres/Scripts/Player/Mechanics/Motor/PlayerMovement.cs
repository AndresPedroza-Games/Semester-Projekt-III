using UnityEngine;

public class PlayerMovement
{
    private PlayerMotor _PlayerMotor;

    private Rigidbody rb;

    private float playerSpeed;
    private float playerCrouchSpeed;
    private float currentSpeed;

    public void Init()
    {
        _PlayerMotor = PlayerMotor.playerMotor;

        playerSpeed = _PlayerMotor.playerSpeed;
        playerCrouchSpeed = _PlayerMotor.playerCrouchSpeed;
        currentSpeed = _PlayerMotor.currentSpeed;

        rb = _PlayerMotor.rb;
    }

    public void Movement()
    {
        currentSpeed = !_PlayerMotor.isCrouching ? playerSpeed : playerCrouchSpeed;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector3(moveX, 0f, moveY) * currentSpeed * Time.deltaTime;
    }
}
