using Cinemachine;
using UnityEngine;


public class PlayerManager : MonoBehaviour {

	public static PlayerManager playerManager;

	public GameObject cinemachine;
	public GameObject designMC;

	private EventSystemChildRoom _EventSystemChildRoom;

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
            InputManager.Instance.Pause.Disable();
			InputManager.Instance.Controls.DoctorOfficePuzzle.Disable();
			InputManager.Instance.Controls.BoardPuzzle.Disable();

        }
        else {
			InputManager.Instance.Controls.Movement.Enable();
			InputManager.Instance.Zoom.Enable();
            InputManager.Instance.Pause.Enable();
            InputManager.Instance.Controls.DoctorOfficePuzzle.Enable();
            InputManager.Instance.Controls.BoardPuzzle.Enable();

        }

        //designMC.SetActive(!status);

        Debug.Log("Player Freeze");
	}


	// private void OnTriggerEnter(Collider collision) {
	// 	if (collision.CompareTag("Trigger"))
	// 		EventSystemController.Instance.CloseDoor();
	//
	// }

}