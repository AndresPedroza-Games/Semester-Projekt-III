using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    public Dictionary<Vector3, PlacementData> _PlacedPieces = new Dictionary<Vector3, PlacementData>();

    public void AddObjectAt(Vector3 gridPos, float rot, int id, int pieceIndex)
    {
        Vector3 positionToOccupy = gridPos;
        PlacementData data = new PlacementData(positionToOccupy,rot,id,pieceIndex);

        if (_PlacedPieces.ContainsKey(positionToOccupy)){
            return;
        }

        _PlacedPieces[positionToOccupy] = data;
    }

        public bool PieceCorrectPosition(Vector3 gridPos, Vector3Int correctPos)
        {
            Vector3 positionToOccupy = _PlacedPieces[gridPos].occupiedPositions;

            if (positionToOccupy == correctPos)
                return true;

            return false;
        }

    public void RemoveObjectAt(Vector3Int gridPos)
    {
        Vector3 positionToOccupy = gridPos;

        if (_PlacedPieces.ContainsKey(positionToOccupy)){
            _PlacedPieces.Remove(positionToOccupy);
        }

    }


    public bool PieceInsideGrid(Vector3 gridPos, Vector2 gridMin, Vector2 gridMax)   
    {
        Vector3 positionToOccupy = gridPos;

        if (positionToOccupy.x < gridMin.x || positionToOccupy.x >= gridMax.x || positionToOccupy.z >= gridMax.y || positionToOccupy.z < gridMin.y)
            return false;

        return true;
    }

    public bool PieceCorrectRotation(Vector3 gridPos, float correctRot)
    {
        if (!_PlacedPieces.TryGetValue(gridPos, out var data))
            return false;

        float positionToOccupy = _PlacedPieces[gridPos].rotation;

        if (positionToOccupy == correctRot)
            return true;

        return false;
    }

    public bool CanPlacePiece(Vector3 gridPos, Vector2 gridMinSize, Vector3 gridMaxSize)
    {
        Vector3 positionToOccupy = gridPos;

        if (_PlacedPieces.ContainsKey(positionToOccupy) || !PieceInsideGrid(gridPos, gridMinSize, gridMaxSize))
            return false;

        return true;
    }
}

public class PlacementData
{
    public Vector3 occupiedPositions;

    public float rotation;
    public int ID { get; private set; }
    public int PlacedPieceIndex { get; private set; }

    public PlacementData(Vector3 occupiedPos, float rot, int id, int placedObjectindex)
    {
        occupiedPositions = occupiedPos;
        rotation = rot;
        ID = id;
        PlacedPieceIndex = placedObjectindex;
    }
}
