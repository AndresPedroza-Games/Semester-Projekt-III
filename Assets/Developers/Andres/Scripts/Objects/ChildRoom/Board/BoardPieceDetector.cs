using UnityEngine;


public class BoardPieceDetector : MonoBehaviour {

	[SerializeField] private Transform boxCenter;
	[SerializeField] private Vector3 boxHalfExtends = Vector3.zero;

	private readonly Collider[] _results = new Collider[15];

	private HoldController _holdController;
	private PuzzleBoard _board;


	private void Start() {
		_board = PuzzleBoard.Instance;
		_holdController = GameManager.Instance.Interactor.GetComponent<HoldController>();
	}


	private void FixedUpdate() {
		if (_board.PieceCount >= 9) {
			enabled = false;
			return;
		}

		if (_holdController.HasObject)
			return;

		DetectPieces();
	}


	private void DetectPieces() {
		int count = Physics.OverlapBoxNonAlloc(boxCenter.position, boxHalfExtends, _results);

		if (count == 0)
			return;

		for (int i = 0; i < count; i++) {

			if (_board.PieceCount >= 9)
				break;

			Collider hit = _results[i];

			if (hit.TryGetComponent(out Piece piece)) {
				if (piece.IsOnBoard)
					continue;

				_board.AddPiece(piece);
				EventSystemChildRoom.Instance.AddedPieceToBoard(piece);
			}
		}
	}


#if UNITY_EDITOR
	private void OnDrawGizmosSelected() {
		if (!boxCenter)
			return;

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(boxCenter.position, boxHalfExtends * 2);
	}
#endif

}