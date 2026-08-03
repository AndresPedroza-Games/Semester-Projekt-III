using System.Collections.Generic;
using UnityEngine;


public class DataManager : MonoBehaviour {

	public static string LastScene;
    // public DataContainer dataContainer;

    //public List<GameObject> itemsPicked;
    // public List<GameObject> doorsOpened;

    // public GameObject currentGameObject;
    // public Transform playerPosition;

    private EventSystemController eventSystemController;

    private void Start() {
	    LastScene = PlayerPrefs.GetString("LastScene");
	    
        eventSystemController = EventSystemController.Instance;

        // eventSystemController.onItemPicked += (item) => currentGameObject = item;
        // eventSystemController.onItemDropped += (item) => currentGameObject = null;

        //eventSystemController.onOpenDoor += (item) => AddData(doorsOpened, item);
        eventSystemController.onDoorClosed += SaveGame;

        // eventSystemController.onSaveGame += SaveGame;
    }

    private void AddData<t>(List<t> itemList, t itemPicked)
    {
        if (itemList.Contains(itemPicked))
            return;

        itemList.Add(itemPicked);
        Debug.Log($"Data has been saved: {itemPicked.GetType()}");
    }

    private void SaveGame(string scene) {
	    LastScene = scene;
	    
	    PlayerPrefs.SetString("LastScene", scene);
	    PlayerPrefs.Save();
	    
	    //dataContainer.playerPosition = this.playerPosition;
	    //dataContainer.currentGameObject = this.currentGameObject;

	    //foreach (GameObject item in itemsPicked)
	    //{
	    //    AddData(dataContainer.itemsPicked, item);
	    //}

	    // foreach (GameObject item in doorsOpened)
	    // {
	    //     AddData(dataContainer.doorsOpened, item);
	    // }
    }

    public static void ClearSavedScene()
    {
	    LastScene = null;
	    
	    PlayerPrefs.DeleteKey("LastScene");
	    PlayerPrefs.Save();
	    
        //itemsPicked.Clear();
        // doorsOpened.Clear();
        // currentGameObject = null;
    }
}   
