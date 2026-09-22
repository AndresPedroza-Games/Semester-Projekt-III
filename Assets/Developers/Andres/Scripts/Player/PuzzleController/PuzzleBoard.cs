using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;


public class PuzzleBoard : MonoBehaviour {

	public static PuzzleBoard Instance;

	[Header("---Scene To Load---")]
	[SerializeField] private SceneReference scene;

	[Header("---Game Event---")]
	[SerializeField] [Range(1, 9)] private int ballerinaEventTriggerPieceCount;

	[Header("---Animation Config---")]
	[SerializeField] private float placementDuration = 0.1f;
	[SerializeField] private Ease placementEase;
	[Space(5)]
	[SerializeField] private float rotationDuration = 0.1f;
	[SerializeField] private Ease rotationEase;
	[Space(5)]
	[SerializeField] private float hoverSmoothTime = 0.03f;

	[Header("---Grid---")]
	[SerializeField] private Grid grid;
	[SerializeField] private float pieceHoverHeight = 0.03f;
	[SerializeField] private LayerMask layerDetector;


	public int PieceCount { get; private set; }
	public Piece HeldPiece { get; private set; }

	private InteractionDetector _interactionDetector;

	private GridData _gridData;
	private Vector3 _hoverVelocity;
	private Vector2Int _gridPos;

	private readonly Vector2Int[] _flashOrder = { new(0, 2), new(1, 2), new(2, 2), new(2, 1), new(2, 0), new(1, 0), new(0, 0), new(0, 1), new(1, 1) };

	private bool _isValidGridPosition;
	private bool _puzzleSolved;


	private void Awake() {
		_gridData = new GridData();
		_puzzleSolved = false;

		if (Instance && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}


	private void OnEnable() {
		InputManager.Instance.RotatePiece.performed += RotatePiece;
	}


	private void OnDisable() {
		InputManager.Instance.RotatePiece.performed -= RotatePiece;
	}


	private void Start() {
		_interactionDetector = GameManager.Instance.Interactor.GetComponent<InteractionDetector>();
	}


	private void Update() {
		if (!HeldPiece)
			return;

		UpdateHeldPiecePosition();
	}


	private void RotatePiece(InputAction.CallbackContext ctx) {
		if (!HeldPiece)
			return;

		int scroll = Mathf.RoundToInt(ctx.ReadValue<Vector2>().y);

		HeldPiece.Rotate(-scroll, rotationDuration, rotationEase);
	}


	public void SelectOrPlace(Piece piece) {
		if (!HeldPiece) {
			SelectPiece(piece);
			return;
		}

		PlaceHeldPiece();
	}


	private void SelectPiece(Piece piece) {
		if (HeldPiece)
			return;

		_gridData.RemovePiece(piece);

		HeldPiece = piece;

		_hoverVelocity = Vector3.zero;

		MovePieceToHoverPosition(piece, piece.GridPosition);

		_isValidGridPosition = false;
	}


	private void PlaceHeldPiece() {
		if (!HeldPiece)
			return;

		if (!_isValidGridPosition)
			return;

		Piece occupyingPiece = _gridData.GetPiece(_gridPos);

		if (occupyingPiece && occupyingPiece.IsLocked)
			return;

		_gridData.SetPiece(_gridPos, HeldPiece);
		HeldPiece.SetGridPosition(_gridPos);

		Tween placementTween = MovePieceToGridPosition(HeldPiece, _gridPos);

		if (!occupyingPiece) {
			HeldPiece = null;
		}
		else {
			HeldPiece = occupyingPiece;
			_hoverVelocity = Vector3.zero;
			MovePieceToHoverPosition(HeldPiece, _gridPos);
		}

		CheckPuzzleWinCondition(placementTween);
	}


	private void UpdateHeldPiecePosition() {
		if (HeldPiece.MoveTween.IsActive())
			return;

		Vector3 mousePos = _interactionDetector.GetRayPosition(layerDetector);

		Vector3Int unityCell = grid.WorldToCell(mousePos);
		_gridPos = new Vector2Int(unityCell.x, unityCell.y);

		_isValidGridPosition = _gridData.IsInside(_gridPos);

		if (!_isValidGridPosition)
			return;

		Vector3 worldPosition = GetWorldPosition(_gridPos);
		Vector3 targetPosition = new(worldPosition.x, grid.transform.position.y + pieceHoverHeight, worldPosition.z);
		HeldPiece.transform.position = Vector3.SmoothDamp(HeldPiece.transform.position, targetPosition, ref _hoverVelocity, hoverSmoothTime);
	}


	private void MovePieceToHoverPosition(Piece piece, Vector2Int position) {
		Vector3 worldPosition = GetWorldPosition(position);
		Vector3 target = new(worldPosition.x, grid.transform.position.y + pieceHoverHeight, worldPosition.z);
		piece.MoveTo(target, placementDuration, placementEase);
	}


	private Tween MovePieceToGridPosition(Piece piece, Vector2Int position) {
		Vector3 worldPosition = GetWorldPosition(position);
		Vector3 target = new(worldPosition.x, grid.transform.position.y + 0.02f, worldPosition.z);

		return piece.MoveTo(target, placementDuration, placementEase, true);
	}


	public void AddPiece(Piece piece) {
		if (PieceCount >= 9)
			return;

		// Vector2Int position = GetRandomFreePosition();
		Vector2Int position = GetFreePosition();

		_gridData.SetPiece(position, piece);
		piece.SetGridPosition(position);

		piece.SetRandomRotation();
		MovePieceToGridPosition(piece, position);

		PieceCount++;

		if (PieceCount == ballerinaEventTriggerPieceCount)
			EventSystemChildRoom.Instance.PuzzlePieceEventTrigger();

		CheckPuzzleWinCondition();
	}


	// private Vector2Int GetRandomFreePosition() {
	// 	List<Vector2Int> freePositions = new();
	//
	// 	for (int x = 0; x < 3; x++) {
	// 		for (int y = 0; y < 3; y++) {
	// 			Vector2Int position = new Vector2Int(x, y);
	//
	// 			if (!_gridData.GetPiece(position))
	// 				freePositions.Add(position);
	// 		}
	// 	}
	//
	// 	return freePositions[Random.Range(0, freePositions.Count)];
	// }


	private Vector2Int GetFreePosition() {

		for (int y = 2; y >= 0; y--) {
			for (int x = 0; x < 3; x++) {
				Vector2Int pos = new Vector2Int(x, y);

				if (!_gridData.GetPiece(pos)) {
					return pos;
				}
			}
		}

		return default;
	}


	private async void LoadScene() {
		if (scene != null)
			await WorldSceneManager.Instance.LoadScene(scene);
	}


	private bool IsPuzzleSolved() {
		if (_gridData.Count != 9)
			return false;

		foreach (Piece piece in _gridData.Pieces) {
			if (!piece.IsLocked)
				return false;
		}

		return true;
	}


	private void UpdateLockedPieces() {
		foreach (Piece piece in _gridData.Pieces) {
			if (piece.GridPosition == piece.PieceData.CorrectPosition && piece.RotationSteps == piece.PieceData.CorrectRotationStep) {
				if (piece.IsLocked)
					continue;

				piece.IsLocked = true;
				_flashOrder[_gridData.Count - 1] = piece.GridPosition;
			}
		}
	}


	private void CheckPuzzleWinCondition(Tween placementTween = null) {
		if (_puzzleSolved)
			return;

		UpdateLockedPieces();

		if (!IsPuzzleSolved())
			return;

		_puzzleSolved = true;

		if (placementTween != null && placementTween.IsActive())
			placementTween.OnComplete(OnPuzzleSolved);
		else
			OnPuzzleSolved();
	}


	private void OnPuzzleSolved() {
		Sequence sequence = DOTween.Sequence();

		const float waveDelay = 0.08f;
		float time = 0f;

		foreach (Vector2Int position in _flashOrder) {
			Piece piece = _gridData.GetPiece(position);

			if (!piece)
				continue;

			sequence.Insert(time, piece.Flash());
			time += waveDelay;
		}

		sequence.OnComplete(() => {
			foreach (Piece p in _gridData.Pieces) {
				if (!p)
					continue;
				p.Flash();
			}

			EventSystemChildRoom.Instance.PuzzleSolved();
			LoadScene();
		});
	}


	private Vector3Int ToUnityCell(Vector2Int position) {
		return new Vector3Int(position.x, position.y, 0);
	}


	private Vector3 GetWorldPosition(Vector2Int position) {
		Vector3Int cellPosition = ToUnityCell(position);
		return grid.GetCellCenterWorld(cellPosition);
	}

}