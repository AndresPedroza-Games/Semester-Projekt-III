using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public class Piece : Holdable, ILeftClickable {

	[Header("PIECE DATA")]
	[field: SerializeField] public PieceData PieceData { get; private set; }

	public bool IsOnBoard { get; private set; }
	public bool IsLocked { get; set; }
	public Vector2Int GridPosition { get; private set; }
	public int RotationSteps { get; private set; }

	private bool _isRotating;
	public Tween MoveTween { get; private set; }


	[Header("FLASH CONFIG")]
	[SerializeField] private float flashDuration = 0.5f;
	[SerializeField] private float flashIntensity = 0.5f;
	[SerializeField] private Color flashColor = Color.white;

	[Header("PARTICLES")]
	[SerializeField] private ParticleSystem particles;

	private readonly int _emissionColor = Shader.PropertyToID("_EmissionColor");


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


	public Tween MoveTo(Vector3 target, float duration, Ease ease, bool juicy = false) {
		MoveTween?.Kill();
		MoveTween = transform.DOMove(target, duration).SetEase(ease).OnComplete(() => {
			if (IsLocked)
				Flash();

			if (juicy) {
				sfx?.PlaySfx(SfxEvent.OnCollision);
				PlayParticles();
			}
		});
		return MoveTween;
	}


	public void Rotate(int direction, float duration, Ease ease) {
		if (_isRotating)
			return;

		RotationSteps = (RotationSteps + direction + 4) % 4;
		Quaternion targetRotation = Quaternion.Euler(0f, RotationSteps * 90f, 0f);
		_isRotating = true;

		transform.DORotateQuaternion(targetRotation, duration).SetEase(ease).OnComplete(() => _isRotating = false);
	}


	public void PlayParticles() {
		particles?.Play();
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
		if (RotationSteps == PieceData.CorrectRotationStep)
			RotationSteps = (RotationSteps + 1) % 4;

		transform.rotation = Quaternion.Euler(0f, RotationSteps * 90f, 0f);
	}


	public bool CanInteractWithLeftClick(HoldController holdController) {
		return !holdController.HasObject && IsOnBoard;
	}


	public void OnLeftClick() {
		if (IsLocked || !IsOnBoard)
			return;

		if (!PuzzleBoard.Instance.HeldPiece)
			sfx?.PlaySfx(SfxEvent.OnPickup);

		PuzzleBoard.Instance.SelectOrPlace(this);
	}


	public override CrosshairType GetCrosshairType(HoldController holdController) {
		if (PuzzleBoard.Instance.HeldPiece == this)
			return CrosshairType.HandClosed;

		return IsLocked ? CrosshairType.Default : CrosshairType.HandOpen;
	}


	public Tween Flash() {
		MaterialPropertyBlock props = new MaterialPropertyBlock();
		ren.GetPropertyBlock(props);
		Color originalEmission = props.GetColor(_emissionColor);

		Sequence sequence = DOTween.Sequence();

		sequence.Append(transform.DOScale(1.1f, flashDuration * 0.5f).SetEase(Ease.OutQuad));

		sequence.Join(DOTween.To(() => 0f, value => {
			props.SetColor(_emissionColor, flashColor * (value * flashIntensity));
			ren.SetPropertyBlock(props);
		}, 1f, flashDuration * 0.5f).SetEase(Ease.OutQuad));

		sequence.Append(transform.DOScale(1f, flashDuration * 0.5f).SetEase(Ease.InQuad));

		sequence.Join(DOTween.To(() => 1f, value => {
			props.SetColor(_emissionColor, flashColor * (value * flashIntensity));

			ren.SetPropertyBlock(props);
		}, 0f, flashDuration * 0.5f).SetEase(Ease.InQuad));

		sequence.OnComplete(() => {
			props.SetColor(_emissionColor, originalEmission);
			ren.SetPropertyBlock(props);
		});

		return sequence;
	}

}