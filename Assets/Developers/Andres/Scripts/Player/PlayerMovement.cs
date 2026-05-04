using UnityEngine;

public class PlayerMovement : PlayerController
{

    public override void Movement()
    {
        currentSpeed = !isCrouching ? playerSpeed : playerCrouchSpeed;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector3(moveX, 0f, moveY) * currentSpeed * Time.deltaTime;
    }
}
