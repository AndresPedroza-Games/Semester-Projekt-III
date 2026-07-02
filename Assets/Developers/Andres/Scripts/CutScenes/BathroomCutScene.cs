using UnityEngine;
using UnityEngine.Playables;

public class BathroomCutScene : MonoBehaviour
{
    [SerializeField] private PlayableDirector _Director;
    [SerializeField] private LoadSceneTrigger _LoadSceneTrigger;
    [SerializeField] private Transform _EndPosPlayer;

    private GameObject _Player;

    private void Start()
    {
        EventSystemBathroom.instance.onCutHair += PlayScene;
        _Player = FindFirstObjectByType<PlayerManager>(FindObjectsInactive.Include).gameObject;

    }

    private void PlayScene()
    {
        _Director.Play();
        FindFirstObjectByType<PlayerManager>().cinemachine.SetActive(false);
    }

    public void LoadScene()
    {
        _LoadSceneTrigger.LoadUnloadRooms();
    }

    public void EndCutScene()
    {
        PLayerSetUp();
        _Director.Stop();
        Debug.Log("End CutScene");
    }

    public void PlayerCrouch()
    {
        _Player.GetComponent<PlayerMotor>().animationPlaying = true;
        StartCoroutine(_Player.GetComponent<PlayerMotor>().playerCrouch.Crouch());
    }

    private void PLayerSetUp()
    {
        _Player.transform.position = _EndPosPlayer.position;
        _Player.GetComponent<PlayerManager>().cinemachine.SetActive(true);
    }
}
