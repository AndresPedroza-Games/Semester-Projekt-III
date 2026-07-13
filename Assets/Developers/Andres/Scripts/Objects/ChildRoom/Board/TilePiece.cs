using UnityEngine;


public class TilePiece : MonoBehaviour, IInteractable, IHighlightable {

	private EventSystemChildRoom _EventSystemChildRoom;

    [Header("---Highlight Config---")]
    [SerializeField] private float borderThickness = 0.02f;

    public PieceData pieceData;
    private Renderer _Renderer;
    private readonly int _BorderThickness = Shader.PropertyToID("_BorderThickness");

    private float _Steps;

    private bool _CanInteract;
    public bool piecePicked;
    public bool ignoreNextPiece;

    private void Awake()
    {
		_Renderer = GetComponentInChildren<Renderer>();
        _CanInteract = true;
    }

    private void Start() {
		_EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
		_EventSystemChildRoom.onPiecePlaced += PlaceTile;
		_EventSystemChildRoom.onRotatePiece += RotatePiece;
        _EventSystemChildRoom.onPuzzleSolved += () => _CanInteract = false;

    }


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}

	public CrosshairType GetCrosshairType(HoldController holdController) {
		return CrosshairType.Interactable;
	}

	public void Interact()
    {
        if (_CanInteract)
        {
            _EventSystemChildRoom.PickPiece(this.gameObject);
            Highlight();

            piecePicked = true;
        }
    }


	private void PlaceTile()
    {
        if (!piecePicked)
            return;

        if (ignoreNextPiece)
        {
            ignoreNextPiece = false;
            return;
        }

        transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        RemoveHighlight();
        piecePicked = false;
        Debug.Log("Piece Placed");

    }

    private void RotatePiece(Vector2 scroll) {

        if (!piecePicked)
            return;

        switch (scroll.y)
        {
            case > 0:
                _Steps++;
                break;

            case < 0:
                _Steps++;
                break;
        }

        _Steps = Mathf.Clamp(_Steps, -4, 4);

        if (_Steps == 4 || _Steps == -4)
            _Steps = 0;

        transform.rotation = Quaternion.Euler(-90f, 0f, _Steps * 90f);


        Debug.Log("Rotate");
    }

    public void Highlight()
    {
        if (!_Renderer)
            return;

        _Renderer.material.SetFloat(_BorderThickness, borderThickness);
    }

    public void RemoveHighlight()
    {
        if (!_Renderer)
            return;

        _Renderer.material.SetFloat(_BorderThickness, 0);
    }
}