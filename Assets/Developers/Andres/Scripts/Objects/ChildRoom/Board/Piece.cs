using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public class Piece : Holdable, ILeftClickable {

	[field: SerializeField]
	public PieceData PieceData { get; private set; }

	public bool IsOnBoard { get; private set; }
	public bool IsLocked { get; set; }
	public Vector2Int GridPosition { get; private set; }
	public int RotationSteps { get; private set; }

	private bool _isRotating;
	public Tween MoveTween { get; private set; }


	private void OnEnable() {
		EventSystemChildRoom.Instance.OnPieceAddedToBoard += OnPieceAddedToBoard;
		EventSystemChildRoom.Instance.OnPuzzleSolved += OnPuzzleSolved;
	}


	protected override void OnDisable() {
		base.OnDisable();

		EventSystemChildRoom.Instance.OnPieceAddedToBoard -= OnPieceAddedToBoard;
		EventSystemChildRoom.Instance.OnPuzzleSolved -= OnPuzzleSolved;
	}


	private void OnPuzzleSolved() {
		IsOnBoard = false;
	}


	public void SetGridPosition(Vector2Int position) {
		GridPosition = position;
	}


	public void MoveTo(Vector3 target, float duration, Ease ease) {
		MoveTween?.Kill();
		MoveTween = transform.DOMove(target, duration).SetEase(ease);
	}


	public void Rotate(int direction, float duration, Ease ease) {
		if (_isRotating)
			return;

		RotationSteps = (RotationSteps + direction + 4) % 4;
		Quaternion targetRotation = Quaternion.Euler(0f, RotationSteps * 90f, 0f);
		_isRotating = true;

		transform.DORotateQuaternion(targetRotation, duration).SetEase(ease).OnComplete(() => _isRotating = false);
	}


	private void OnPieceAddedToBoard(Piece piece) {
		if (piece != this)
			return;

		canBeHold = false;
		Rigidbody.isKinematic = true;
		Rigidbody.useGravity = false;

		IsOnBoard = true;
	}


	public void SetRandomRotation() {
		RotationSteps = Random.Range(0, 4);
		transform.rotation = Quaternion.Euler(0f, RotationSteps * 90f, 0f);
	}


	public bool CanInteractWithLeftClick(HoldController holdController) {
		return !holdController.HasObject && IsOnBoard;
	}


	public void OnLeftClick() {
		if (IsLocked)
			return;

		if (!IsOnBoard)
			return;

		PuzzleBoard.Instance.SelectOrPlace(this);
	}


	public override CrosshairType GetCrosshairType(HoldController holdController) {
		if (PuzzleBoard.Instance.HeldPiece == this)
			return CrosshairType.HandClosed;

		return IsLocked ? CrosshairType.Default : CrosshairType.HandOpen;
	}

}