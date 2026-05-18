using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class DataManager : MonoBehaviour
{
    public DataContainer dataContainer;

    //public List<GameObject> itemsPicked;
    public List<GameObject> doorsOpened;

    public GameObject currentGameObject;
    public Transform playerPosition;

    private EventSystemController eventSystemController;

    private void Start()
    {
        eventSystemController = EventSystemController.eventSystemController;

        eventSystemController.onItemPicked += (item) => currentGameObject = item;
        eventSystemController.onItemDropped += (item) => currentGameObject = null;

        eventSystemController.onOpenDoor += (item) => AddData(doorsOpened, item);
        eventSystemController.onCloseDoor += SaveGame;

        eventSystemController.onSaveGame += SaveGame;
        eventSystemController.onExitGame += ClearData;
    }

    private void AddData<t>(List<t> itemList, t itemPicked)
    {
        if (itemList.Contains(itemPicked))
            return;

        itemList.Add(itemPicked);
        Debug.Log($"Data has been saved: {itemPicked.GetType()}");
    }

    private void SaveGame()
    {
        dataContainer.playerPosition = this.playerPosition;
        dataContainer.currentGameObject = this.currentGameObject;

        //foreach (GameObject item in itemsPicked)
        //{
        //    AddData(dataContainer.itemsPicked, item);
        //}

        foreach (GameObject item in doorsOpened)
        {
            AddData(dataContainer.doorsOpened, item);
        }
    }

    private void ClearData()
    {
        //itemsPicked.Clear();
        doorsOpened.Clear();
        currentGameObject = null;
    }
}   
