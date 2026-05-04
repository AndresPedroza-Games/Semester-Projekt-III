using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("----Components----")]
    protected Rigidbody rb;
    protected Camera mainCamera;
    protected CharacterController characterController;

    [Header("----Crouch Settings----")]
    protected bool isCrouching;

    [Header("----Movement Settings----")]
    [SerializeField] protected float playerSpeed;
    [SerializeField] protected float playerCrouchSpeed;
    protected float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        mainCamera = FindFirstObjectByType<Camera>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Crouch();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public virtual void Movement() {}
    public virtual void Crouch() {}
}
