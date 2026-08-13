using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMotor : MonoBehaviour
{
    public static PlayerMotor playerMotor;

    [Header("Mechanics")]
    private PlayerMovement _PlayerMovement;
    public PlayerCrouch playerCrouch;

    [Header("---Components---")]
    public CharacterController characterController { get; set; }
    public Camera mainCamera { get; private set; }
    public Transform head;

    [Header("----Movement Settings----")]
    public float playerSpeed;
    public float playerCrouchSpeed;
    public float currentSpeed { get; set;}

    private Vector2 moveInput;
    public Vector3 externalForce;
    private float _TransitionSpeed = 0.1f;

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
    [field: SerializeField] public float coolDownCrouch { get; set; }

    public float radius { get; set;}
    public LayerMask headCollision { get; private set; }
    public bool isCrouching { get; set;}
    public bool animationPlaying;

    [Header("---Footstep Config---")]
    [SerializeField] private float walkStepDistance = 1.0f;
    [SerializeField] private AudioClip footstepSfx;
    [SerializeField] private Vector2 pitchRange = Vector2.one;
    private Vector3 _lastStepPosition;
    private AudioSource _audioSource;

    
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
        isCrouching = true;

        _PlayerMovement = new PlayerMovement();
        _PlayerMovement.Init();

        playerCrouch = new PlayerCrouch();
        playerCrouch.Init();

        externalForce = Vector3.zero;
    }


    private void Start() {
	    _lastStepPosition = transform.position;
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


    private void Movement()
    {
        _PlayerMovement.Movement(characterController, moveInput, externalForce);

        if (externalForce.magnitude > 0)
            StartCoroutine(StopForce());
    }

    private void Update()
    {
        Movement();
        HandleFootstepSound();
        HandleGravity();
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
        if (!animationPlaying)
        {
            animationPlaying = true;
            StartCoroutine(playerCrouch.Crouch());
        }
    }


    public void ForceCrouch() {
	    isCrouching = false;
	    
	    characterController.height = crouchHeight;
	    characterController.center = crouchCenter;

	    head.localPosition = new Vector3(head.localPosition.x, crouchHeight, head.localPosition.z);
    }
    

    private void GetComponents()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        _audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator StopForce()
    {
        float timeElapsed = 0f;

        while (timeElapsed < _TransitionSpeed)
        {
            float t = timeElapsed / _TransitionSpeed;
            t = Mathf.SmoothStep(0f, 1f, t);

            externalForce = Vector3.Lerp(externalForce, Vector3.zero, Time.deltaTime * t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }


    private void HandleFootstepSound() {
	    if (!characterController.isGrounded || !isCrouching)
		    return;

	    Vector3 velocity = characterController.velocity;
	    velocity.y = 0f;

	    if (velocity.magnitude < 0.1f)
		    return;

	    float distance = Vector3.Distance( transform.position, _lastStepPosition );

	    if (distance >= walkStepDistance) {
		    _audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
		    _audioSource?.PlayOneShot(footstepSfx);
		    _lastStepPosition = transform.position;
	    }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * (standHeight - radius), radius);
    }
}
