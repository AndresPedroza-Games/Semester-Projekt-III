using System;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class EventSystemChildRoom : MonoBehaviour {

	public static EventSystemChildRoom Instance;

	public event Action OnPuzzleSolved;
	public event Action OnPuzzlePieceEventTriggered;
	public event Action<Piece> OnPieceAddedToBoard;


	private void Awake() {
		if (Instance && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}


	public void PuzzlePieceEventTrigger() {
		OnPuzzlePieceEventTriggered?.Invoke();
	}


	public void PuzzleSolved() {
		OnPuzzleSolved?.Invoke();
	}


	public void AddedPieceToBoard(Piece piece) {
		OnPieceAddedToBoard?.Invoke(piece);
	}

}