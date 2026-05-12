using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    public static PlayerMotor playerMotor;

    [Header("Mechanics")]
    private PlayerMovement _PlayerMovement;
    private PlayerCrouch _PlayerCrouch;

    [Header("---Components---")]
    public CharacterController characterController { get; set; }
    public Camera mainCamera { get; private set; }
    public Transform head;

    [Header("----Movement Settings----")]
    public float playerSpeed;
    public float playerCrouchSpeed;
    public float currentSpeed { get; set;}

    private Vector2 moveInput;

    [Header("----Gravity Settings----")]
    public float gravityMultiplier = 1f;
    [Range(-50f, -1f)] public float maxFallSpeed = -50f;
    public float yVelocity { get; set; }


    [Header("----Crouch Settings----")]
    public float crouchHeight { get; set; }
    public float standHeight { get; set; }

    public Vector3 crouchCenter { get; set; }
    public Vector3 standCenter { get; set; }
    public float transitionSpeed;

    public float radius { get; set;}
    public LayerMask headCollision { get; private set; }
    public bool isCrouching { get; set;}

    private void Awake()
    {
        if (playerMotor == null)
            playerMotor = this;

        GetComponents();

        headCollision = ~LayerMask.GetMask("Player");

        standHeight = characterController.height;
        crouchHeight = standHeight / 2f;

        standCenter = characterController.center;
        crouchCenter = standCenter / 2f;

        radius = characterController.radius;

        _PlayerMovement = new PlayerMovement();
        _PlayerMovement.Init();

        _PlayerCrouch = new PlayerCrouch();
        _PlayerCrouch.Init();
    }

    private void OnEnable() {
        InputManager.Instance.Move.performed += OnMoveInputPerformed;
        InputManager.Instance.Move.canceled += OnMoveInputCanceled;

        InputManager.Instance.Crouch.performed += Crouch;
    }

    private void OnDisable() {
        InputManager.Instance.Move.performed -= OnMoveInputPerformed;
        InputManager.Instance.Move.canceled -= OnMoveInputCanceled;

        InputManager.Instance.Crouch.performed -= Crouch;
    }

    private void OnMoveInputPerformed(InputAction.CallbackContext ctx) {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveInputCanceled(InputAction.CallbackContext ctx) {
        moveInput = Vector2.zero;
    }

    public void Movement()
    {
        _PlayerMovement.Movement(characterController, moveInput);
    }


    private void LateUpdate() {
	    RotatePlayer();
    }


    private void RotatePlayer() {
	    Vector3 camEuler = mainCamera.transform.eulerAngles;
	    transform.rotation = Quaternion.Euler(0f, camEuler.y, 0f);
    }


    public void HandleGravity() {
        _PlayerMovement.HandleGravity();
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        StartCoroutine(_PlayerCrouch.Crouch());
    }

    private void GetComponents()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * (standHeight - radius), radius);
    }
}
