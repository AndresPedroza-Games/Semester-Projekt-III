using System;
using UnityEngine;

public class EventSystemController : MonoBehaviour
{
    public static EventSystemController eventSystemController;

    [Header("Events")]
    //We can use to add sounds or to change the UI
    public Action onCloseDoor;
    public Action<GameObject> onOpenDoor;
    public Action<GameObject> onItemPicked;
    public Action<GameObject> onItemDropped;

    public Action onSaveGame;
    public Action onExitGame;

    private void Awake()
    {
        if(eventSystemController == null)
            eventSystemController = this;

    }

    private void Start()
    {
        
    }

    public void OpenDoor(GameObject item)
    {
        if (onOpenDoor != null)
            onOpenDoor.Invoke(item);
    }

    public void CloseDoor()
    {
        if (onCloseDoor != null)
            onCloseDoor.Invoke();
    }

    public void PickItem(GameObject item)
    {
        if (onItemPicked != null)
            onItemPicked.Invoke(item);
    }

    public void DropItem(GameObject item)
    {
        if (onItemDropped != null)
            onItemDropped.Invoke(item);
    }

    public void SaveGame()
    {
        if (onSaveGame != null)
            onSaveGame.Invoke();
    }
}
