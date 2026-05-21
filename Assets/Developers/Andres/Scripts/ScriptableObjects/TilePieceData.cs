using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "TilePieceData", menuName = "1st Puzzle/TilePieceData")]
public class TilePieceData : ScriptableObject
{
    public List<PieceData> piecesData;
}
