using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager playerManager;

    private PlayerMotor _PlayerMotor;

    private void Awake()
    {
        if (playerManager == null)
            playerManager = this;
    }

    private void Start()
    {
        _PlayerMotor = GetComponent<PlayerMotor>();

    }

    private void Update() {
        _PlayerMotor.Movement();
        _PlayerMotor.HandleGravity();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Trigger"))
            EventSystemController.eventSystemController.CloseDoor();

    }
}
