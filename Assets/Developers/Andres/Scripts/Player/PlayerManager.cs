using Cinemachine;
using UnityEngine;


public class PlayerManager : MonoBehaviour {

	public static PlayerManager playerManager;

	public CinemachineVirtualCamera cinemachine;

	private EventSystemChildRoom _EventSystemChildRoom;

    private void Start()
    {
        if (EventSystemChildRoom.eventSystemChildRoom != null)
        {
            _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
            _EventSystemChildRoom.onInteractWithBoard += Inspect;
        }
    }

	private void Awake() {
		if (playerManager == null)
			playerManager = this;
	}

	private void Inspect(Transform cameraPos, Transform lookAt, float fov) {
		cinemachine.transform.position = cameraPos.position;
		cinemachine.LookAt = lookAt;
		cinemachine.Follow = null;
		cinemachine.m_Lens.FieldOfView = fov;
		FreezeCharacter(true);
	}


	public void FreezeCharacter(bool status) {
		if (status) {
			InputManager.Instance.Controls.Movement.Disable();
			InputManager.Instance.Zoom.Disable();
		}
		else {
			InputManager.Instance.Controls.Movement.Enable();
			InputManager.Instance.Zoom.Enable();
		}
		Debug.Log("Player Freeze");
	}


	private void OnTriggerEnter(Collider collision) {
		if (collision.CompareTag("Trigger"))
			EventSystemController.Instance.CloseDoor();

	}

}