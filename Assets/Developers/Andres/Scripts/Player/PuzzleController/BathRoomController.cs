using UnityEngine;
using System.Collections;

public class BathRoomController : MonoBehaviour
{
    private PlayerManager _PlayerManager;
    private EventSystemBathroom _EventSystemBathroom;

    private bool _IsActive;

    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;
        _EventSystemBathroom.onInteractValve += InteactValve;

        _PlayerManager = PlayerManager.playerManager;
    }

    private void InteactValve()
    {

        //StartCoroutine(SetActive(true));
    }

    private IEnumerator SetActive(bool status)
    {
        yield return new WaitForSeconds(0.5f);
        _IsActive = status;
        GameManager.Instance.miniGameActive = status;
    }

}
