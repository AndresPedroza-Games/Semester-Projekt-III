using UnityEngine;

public class PlayerMovement
{
    private PlayerMotor _PlayerMotor;
    private Camera cam;

    private float playerSpeed;
    private float playerCrouchSpeed;
    private float currentSpeed;
    private const float gravity = -9.81f;

    public void Init()
    {
        _PlayerMotor = PlayerMotor.playerMotor;
        cam = _PlayerMotor.mainCamera;

        playerSpeed = _PlayerMotor.playerSpeed;
        playerCrouchSpeed = _PlayerMotor.playerCrouchSpeed;
        currentSpeed = _PlayerMotor.currentSpeed;
    }

    public void Movement(CharacterController controller, Vector2 moveInput)
    {
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        currentSpeed = !_PlayerMotor.isCrouching ? playerSpeed : playerCrouchSpeed;
        Vector3 horizontal = (forward * moveInput.y + right * moveInput.x).normalized;
        Vector3 vertical = Vector3.up * _PlayerMotor.yVelocity;

        controller.Move(currentSpeed * Time.deltaTime * (horizontal + vertical));
    }

    public void HandleGravity() {
        if (_PlayerMotor.characterController.isGrounded && _PlayerMotor.yVelocity <= 0) {
            _PlayerMotor.yVelocity = -2.0f;
        }
        else {
            _PlayerMotor.yVelocity += gravity * _PlayerMotor.gravityMultiplier * Time.deltaTime;
            _PlayerMotor.yVelocity = Mathf.Clamp(_PlayerMotor.yVelocity, _PlayerMotor.maxFallSpeed, 0f);
        }
    }
}
