using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private float _PlayerSpeed;
    [SerializeField] private float _CameraSpeed;

    private Rigidbody rb;
    private Camera _MainCamera;
    private EventSystem eventSystem;
    private MainMenu mainMenu;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _MainCamera = FindFirstObjectByType<Camera>();
        mainMenu = FindFirstObjectByType<MainMenu>(FindObjectsInactive.Include);
    }

    private void Start()
    {
        eventSystem = EventSystem.eventSystem;
    }

    private void Update()
    {
        Movement();
        CameraRotation();

        PauseGame();
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.mainMenuHUD.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        mainMenu.mainMenuHUD.SetActive(false);
    }

    private void Movement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector3(moveX, 0f, moveY) * _PlayerSpeed * Time.deltaTime;
    }

    private void CameraRotation()
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        _MainCamera.transform.localEulerAngles = new Vector3(moveX, moveY, 0f) * _CameraSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IOpenable isOpenAble = collision.gameObject.GetComponent<IOpenable>();

        if (isOpenAble != null)
            isOpenAble.Open();
    }

    private void OnTriggerEnter(Collider collision)
    {
        IPickable isPickable = collision.GetComponent<IPickable>();

        if (isPickable != null)
            isPickable.PickUp();

    }
}
