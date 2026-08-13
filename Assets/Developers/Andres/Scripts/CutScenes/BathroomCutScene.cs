using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;


public class BathroomCutScene : MonoBehaviour
{
    [SerializeField] private PlayableDirector _Director;
    [SerializeField] private LoadSceneTrigger _LoadSceneTrigger;
    [SerializeField] private Transform PlayerStartPos;
    [SerializeField] private Transform _StartPosPlayer;
    [SerializeField] private Transform _EndPosPlayer;

    private GameObject _Player;
    private Transform playerCam;
    private CinemachinePOV _cinePovComp;

    private void Start()
    {
        EventSystemBathroom.instance.onCutHair += PlayScene;
        _Player = FindFirstObjectByType<PlayerManager>(FindObjectsInactive.Include).gameObject;
        playerCam = GameManager.Instance.Camera.transform;
        _cinePovComp = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();
    }

    private void PlayScene() {
	    EventSystemController.Instance.CutSceneStart();
	    SetPlayerPositionAndRotation();
	    
        _Director.Play();

        GameManager.Instance.Camera.transform.position = new Vector3(_StartPosPlayer.position.x, playerCam.position.y, _StartPosPlayer.position.z);
        
        InputManager.Instance.Controls.Movement.Disable();
        InputManager.Instance.Controls.Interaction.Disable();
        InputManager.Instance.Controls.Game.Disable();
        // FindFirstObjectByType<PlayerManager>().cinemachine.SetActive(false);
	    _Player.SetActive(false);
    }
    
    private void SetPlayerPositionAndRotation() {
        _Player.transform.position = _StartPosPlayer.position;
	    _cinePovComp.m_HorizontalAxis.Value = _StartPosPlayer.eulerAngles.y;
	    _cinePovComp.m_VerticalAxis.Value = 0f;
    }

    public void LoadScene()
    {
        _LoadSceneTrigger.LoadUnloadRooms();
    }

    public void EndCutScene()
    {
        PLayerSetUp();
        _Director.Stop();
    }

    public void PlayerCrouch() {
	    if (!_Player.GetComponent<PlayerMotor>().isCrouching)
		    return;
        _Player.GetComponent<PlayerMotor>().animationPlaying = true;
        StartCoroutine(_Player.GetComponent<PlayerMotor>().playerCrouch.Crouch());
    }

    private void PLayerSetUp()
    {
	    _Player.SetActive(true);
        _Player.transform.position = _EndPosPlayer.position;
        
	    EventSystemController.Instance.CutsceneEnd();
        
        InputManager.Instance.Controls.Movement.Enable();
        InputManager.Instance.Controls.Interaction.Enable();
        InputManager.Instance.Controls.Game.Enable();
        PlayerCrouch();
        // _Player.GetComponent<PlayerManager>().cinemachine.SetActive(true);
    }
}
