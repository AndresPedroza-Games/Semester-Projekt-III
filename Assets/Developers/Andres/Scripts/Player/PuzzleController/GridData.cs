using System.Collections.Generic;
using UnityEngine;


public class GridData {

	private readonly Dictionary<Vector2Int, Piece> _pieces = new();
	public IEnumerable<Piece> Pieces => _pieces.Values;
	public int Count => _pieces.Count;


	public Piece GetPiece(Vector2Int position) {
		_pieces.TryGetValue(position, out Piece piece);
		return piece;
	}


	public void SetPiece(Vector2Int position, Piece piece) {
		_pieces[position] = piece;
	}


	public void RemovePiece(Piece piece) {
		Vector2Int? positionToRemove = null;

		foreach (var pair in _pieces) {
			if (pair.Value == piece) {
				positionToRemove = pair.Key;
				break;
			}
		}

		if (positionToRemove.HasValue)
			_pieces.Remove(positionToRemove.Value);
	}


	public bool IsInside(Vector2Int position) {
		return position.x is >= 0 and < 3 && position.y is >= 0 and < 3;
	}

}