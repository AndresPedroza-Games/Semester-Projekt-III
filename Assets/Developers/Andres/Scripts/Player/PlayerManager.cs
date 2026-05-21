using Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager playerManager;

    public CinemachineVirtualCamera cinemachine;

    private PlayerMotor _PlayerMotor;
    private EventSystemChildRoom _EventSystemChildRoom;

    private void Awake()
    {
        if (playerManager == null)
            playerManager = this;

        _PlayerMotor = GetComponent<PlayerMotor>();
    }

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onInteractWithBoard += Inspect;
    }

    private void Inspect(Transform cameraPos, Transform lookAt, float fov)
    {
        cinemachine.transform.position = cameraPos.position;
        cinemachine.LookAt = lookAt;
        cinemachine.Follow = null;
        cinemachine.m_Lens.FieldOfView = fov;
        FreezeCharacter(true,100f);
    }

    public void FreezeCharacter(bool status, float distance)
    {
        _PlayerMotor.enabled = !status;
        GetComponentInChildren<InteractionDetector>().interactionDistance = distance;
        Debug.Log("Player Freeze");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Trigger"))
            EventSystemController.eventSystemController.CloseDoor();

    }
}
