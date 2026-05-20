using UnityEngine;
using UnityEngine.InputSystem;

public class Box : MonoBehaviour
{
    [SerializeField] private GameObject _Top;

    private EventSystemChildRoom _EventSystemChildRoom;

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onPuzzleSolved += OpenBox;
    }

    private void OpenBox()
    {
        Debug.Log("Box opened");
    }
}
