using System;
using UnityEngine;

public class EventSystemController : MonoBehaviour
{
    public static EventSystemController eventSystemController;

    [Header("Events")]
    //We can use to add sounds or to change the UI
    public Action onCloseDoor;
    public Action onItemPicked;
    public Action onItemDropped;

    public Action onSaveGame;

    private void Awake()
    {
        if(eventSystemController == null)
            eventSystemController = this;

    }

    private void Start()
    {
        
    }

    public void CloseDoor()
    {
        if (onCloseDoor != null)
            onCloseDoor.Invoke();
    }

    public void PickItem()
    {
        if (onItemPicked != null)
            onItemPicked.Invoke();
    }

    public void DropItem()
    {
        if (onItemDropped != null)
            onItemDropped.Invoke();
    }

    public void SaveGame()
    {
        if (onSaveGame != null)
            onSaveGame.Invoke();
    }
}
