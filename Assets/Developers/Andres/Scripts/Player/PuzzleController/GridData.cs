using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3Int, PlacementData> _PlacedPieces = new Dictionary<Vector3Int, PlacementData>();

    public void AddObjectAt(Vector3Int gridPos, Vector2Int pieceSize, int id, int pieceIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPos, pieceSize);
        PlacementData data = new PlacementData(positionToOccupy,id,pieceIndex);

        foreach (var position in positionToOccupy)
        {
            if (_PlacedPieces.ContainsKey(position))
                return;

            _PlacedPieces[position] = data;
        }
    }

    public void RemoveObjectAt(Vector3Int gridPos, Vector2Int pieceSize, int id, int pieceIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPos, pieceSize);

        foreach (var position in positionToOccupy)
        {
            if (_PlacedPieces.ContainsKey(position))
                _PlacedPieces.Remove(position);
        }
    }

    public List<Vector3Int> CalculatePositions(Vector3Int gridPos, Vector2Int pieceSize)
    {
        List<Vector3Int> returnValue = new List<Vector3Int>();

        for (int x = 0; x < pieceSize.x; x++)
        {
            for (int y = 0; y < pieceSize.y; y++)
            {
                returnValue.Add(gridPos + new Vector3Int(x, 0, y));
            }
        }

        return returnValue;
    }

    public bool PieceInsideGrid(Vector3Int gridPos, Vector2Int pieceSize, Vector2 gridSize)   
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPos, pieceSize);

        foreach (var position in positionToOccupy)
        {
            if (position.x < gridSize.x || position.x > gridSize.y || position.z > gridSize.y || position.z < gridSize.x)
                return false;
        }

        return true;
    }

    public bool PieceCorrectPosition(Vector3Int gridPos, List<Vector3Int> correctPos, int id)
    {
        if(_PlacedPieces.ContainsKey(gridPos))
            if (_PlacedPieces[gridPos].occupiedPositions == correctPos && _PlacedPieces[gridPos].ID == id)
                return true;

        return false;
    }

    public bool CanPlacePiece(Vector3Int gridPos, Vector2Int pieceSize, Vector2 gridSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPos, pieceSize);

        foreach (var position in positionToOccupy)
        {
            if (_PlacedPieces.ContainsKey(position) || !PieceInsideGrid(gridPos, pieceSize, gridSize))
                return false;
        }

        return true;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedPieceIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPos, int id, int placedObjectindx)
    {
        occupiedPositions = occupiedPos;
        ID = id;
        PlacedPieceIndex = placedObjectindx;
    }
}
