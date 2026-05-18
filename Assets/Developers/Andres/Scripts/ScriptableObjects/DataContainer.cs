using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DataContainer", menuName = "Data Manager/DataContainer")]
public class DataContainer : ScriptableObject
{
    public List<GameObject> itemsPicked;
    public List<GameObject> doorsOpened;

    public GameObject currentGameObject;
    public Transform playerPosition;
}
