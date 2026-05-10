using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public static PlayerMotor playerMotor;

    [Header("Mechanics")]
    private PlayerMovement _PlayerMovement;
    private PlayerCrouch _PlayerCrouch;

    [Header("---Components---")]
    public CharacterController characterController { get; set; }
    public Camera mainCamera { get; set; }
    public Rigidbody rb { get; set; }

    [Header("----Movement Settings----")]
    public float playerSpeed;
    public float playerCrouchSpeed;
    public float currentSpeed { get; set;}

    [Header("----Crouch Settings----")]
    public float crouchHeight { get; set; }
    public float standHeight { get; set; }

    public Vector3 crouchCenter { get; set; }
    public Vector3 standCenter { get; set; }
    public float transitionSpeed;

    public float radius { get; set;}
    public LayerMask headCollision;
    public KeyCode crouchKey = KeyCode.LeftShift;

    public bool isCrouching { get; set;}

    private void Awake()
    {
        if (playerMotor == null)
            playerMotor = this;

        GetComponents();

        standHeight = characterController.height;
        crouchHeight = standHeight / 2f;

        standCenter = characterController.center;
        crouchCenter = standCenter / 2f;

        _PlayerMovement = new PlayerMovement();
        _PlayerMovement.Init();

        _PlayerCrouch = new PlayerCrouch();
        _PlayerCrouch.Init();
    }

    public void Movement()
    {
        _PlayerMovement.Movement();
    }

    public void Crouch()
    {
        if (Input.GetKeyDown(crouchKey))
            StartCoroutine(_PlayerCrouch.Crouch());
    }

    private void GetComponents()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = FindFirstObjectByType<Camera>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * (crouchHeight - radius), radius);
    }
}
