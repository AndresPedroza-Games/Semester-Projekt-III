using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private GameObject _Hinge;
    [SerializeField] private GameObject _Key;

    private EventSystemChildRoom _EventSystemChildRoom;

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onPuzzleSolved += OpenBox;
    }

    private void OpenBox()
    {
        Debug.Log("Box opened");
        RotateHinge();
        _Key.gameObject.SetActive(true);
    }

    private void RotateHinge()
    {
        _Hinge.transform.Rotate(-45f,0f,0f);
    }
}
