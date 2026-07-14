using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PieceData", menuName = "1st Puzzle/PieceData")]
public class PieceData : ScriptableObject
{
    [field: SerializeField] public string pieceName { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Vector2Int size { get; private set; }
    [field: SerializeField] public Vector3Int currentPos;
    [field: SerializeField] public Vector3Int correctPos { get; private set;}
    [field: SerializeField] public float correctRot { get; private set;}
    [field: SerializeField] public GameObject prefab { get; private set; }
}
