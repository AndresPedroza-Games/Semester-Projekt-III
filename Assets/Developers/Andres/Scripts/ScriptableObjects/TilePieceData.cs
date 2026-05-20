using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "TilePieceData", menuName = "1st Puzzle/TilePieceData")]
public class TilePieceData : ScriptableObject
{
    public List<PieceData> piecesData;
}

[Serializable]
public class PieceData
{
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public string ID { get; private set; }
    [field: SerializeField] public Vector2Int size { get; private set; }
    [field: SerializeField] public GameObject prefab { get; private set; }
}
