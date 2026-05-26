using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3, PlacementData> _PlacedPieces = new Dictionary<Vector3, PlacementData>();

    public void AddObjectAt(Vector3 gridPos, Vector2Int pieceSize, int id, int pieceIndex)
    {
        List<Vector3> positionToOccupy = CalculatePositions(gridPos, pieceSize);
        PlacementData data = new PlacementData(positionToOccupy,id,pieceIndex);

        foreach (var position in positionToOccupy)
        {
            if (_PlacedPieces.ContainsKey(position))
                return;

            _PlacedPieces[position] = data;
        }
    }

    public void RemoveObjectAt(Vector3Int gridPos, Vector2Int pieceSize)
    {
        List<Vector3> positionToOccupy = CalculatePositions(gridPos, pieceSize);

        foreach (var position in positionToOccupy)
        {
            if (_PlacedPieces.ContainsKey(position))
                _PlacedPieces.Remove(position);
        }
    }

    public List<Vector3> CalculatePositions(Vector3 gridPos, Vector2Int pieceSize)
    {
        List<Vector3> returnValue = new List<Vector3>();

        for (int x = 0; x < pieceSize.x; x++)
        {
            for (int y = 0; y < pieceSize.y; y++)
            {
                returnValue.Add(gridPos + new Vector3(x, 0, y));
            }
        }

        return returnValue;
    }

    public bool PieceInsideGrid(Vector3 gridPos, Vector2Int pieceSize, Vector2 gridSize)   
    {
        List<Vector3> positionToOccupy = CalculatePositions(gridPos, pieceSize);

        foreach (var position in positionToOccupy)
        {
            if (position.x < gridSize.x || position.x > gridSize.y || position.z > gridSize.y || position.z < gridSize.x)
                return false;
        }

        return true;
    }

    public bool PieceCorrectPosition(Vector3 gridPos, Vector2Int pieceSize, List<Vector3Int> correctPos)
    {
        List<Vector3> positionToOccupy = CalculatePositions(gridPos, pieceSize);

        for (int index = 0; index < positionToOccupy.Count; index++)
        {
            if (positionToOccupy[index] == correctPos[index])
                return true;
        }

        return false;
    }

    public bool CanPlacePiece(Vector3 gridPos, Vector2Int pieceSize, Vector2 gridSize)
    {
        List<Vector3> positionToOccupy = CalculatePositions(gridPos, pieceSize);

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
    public List<Vector3> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedPieceIndex { get; private set; }

    public PlacementData(List<Vector3> occupiedPos, int id, int placedObjectindx)
    {
        occupiedPositions = occupiedPos;
        ID = id;
        PlacedPieceIndex = placedObjectindx;
    }
}
