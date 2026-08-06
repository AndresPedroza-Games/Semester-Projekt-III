using Cinemachine;
using UnityEngine;


public class PlayerManager : MonoBehaviour {

	public static PlayerManager playerManager;

	public GameObject cinemachine;
	public GameObject designMC;

    private void Awake()
    {
        if (playerManager == null)
            playerManager = this;

        cinemachine = FindFirstObjectByType<CinemachineVirtualCamera>().gameObject;

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
	}

}