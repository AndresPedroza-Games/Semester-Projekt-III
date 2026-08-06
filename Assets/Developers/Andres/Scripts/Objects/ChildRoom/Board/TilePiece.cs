using UnityEngine;


public class TilePiece : MonoBehaviour, IHighlightable, ILeftClickable, ICrosshair {

	private EventSystemChildRoom _EventSystemChildRoom;

	[Header("---Highlight Config---")]
	[SerializeField] private float borderThickness = 0.02f;

	public PieceData pieceData;
	private Renderer _Renderer;
	private readonly int _BorderThickness = Shader.PropertyToID("_BorderThickness");

	private float _Steps;

	private bool _CanInteract;
	private bool IsSelected => PlacementSystem.Instance._SelectedObject == gameObject;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");
		_Renderer = GetComponentInChildren<Renderer>();
		_CanInteract = true;

		_Steps = RandomRotation();

		transform.rotation = Quaternion.Euler(0f, _Steps * 90f, 0f);
	}


	private void Start() {
		_EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
		_EventSystemChildRoom.onRotatePiece += RotatePiece;
		_EventSystemChildRoom.onPuzzleSolved += () => _CanInteract = false;
	}


	private void OnDestroy() {
		if (!_EventSystemChildRoom)
			return;

		_EventSystemChildRoom.onRotatePiece -= RotatePiece;
	}


	public bool CanInteractWithLeftClick(HoldController holdController) {
		return !holdController.HasObject;
	}


	public void OnLeftClick() {
		if (!IsSelected)
			SelectTile();
		else 
			PlaceTile();
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return CrosshairType.Interactable;
	}


	private int RandomRotation() {
		int randomStep = Random.Range(-4, 5);

		return randomStep;
	}


	public void SelectTile() {
		if (_CanInteract && !PlacementSystem.Instance._SelectedObject) {
			_EventSystemChildRoom.PickPiece(gameObject);
			Highlight();
		}
	}


	private void PlaceTile() {

		transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
		RemoveHighlight();
		
		_EventSystemChildRoom.PlacePiece();
	}


	private void RotatePiece(Vector2 scroll) {

		if (!IsSelected)
			return;

		switch (scroll.y) {
			case > 0:
				_Steps--;
				break;

			case < 0:
				_Steps++;
				break;
		}

		_Steps = Mathf.Clamp(_Steps, -4, 4);

		if (_Steps == 4 || _Steps == -4)
			_Steps = 0;

		transform.rotation = Quaternion.Euler(0f, _Steps * 90f, 0f);
	}


	public void Highlight() {
		if (!_Renderer)
			return;

		_Renderer.material.SetFloat(_BorderThickness, borderThickness);
	}


	public void RemoveHighlight() {
		if (!_Renderer)
			return;

		_Renderer.material.SetFloat(_BorderThickness, 0);
	}

}