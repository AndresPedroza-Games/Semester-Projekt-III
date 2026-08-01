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

        RandomRotation();

        transform.rotation = Quaternion.Euler(0f, _Steps * 90f, 0f);
    }

    private void Start() {
		_EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
		_EventSystemChildRoom.onPiecePlaced += PlaceTile;
		_EventSystemChildRoom.onRotatePiece += RotatePiece;
        _EventSystemChildRoom.onPuzzleSolved += () => _CanInteract = false;

        ignoreNextPiece = false;
    }

    private void OnDestroy()
    {
        if (_EventSystemChildRoom == null)
            return;

        _EventSystemChildRoom.onPiecePlaced -= PlaceTile;
        _EventSystemChildRoom.onRotatePiece -= RotatePiece;
    }


    public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}

	public CrosshairType GetCrosshairType(HoldController holdController) {
		return CrosshairType.Interactable;
	}

    private float RandomRotation()
    {
        float randomStep = Random.Range(-4f,4f);

        return randomStep;
    }

	public void Interact()
    {
        if (_CanInteract && PlacementSystem.Instace._SelectedObject == null)
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

        transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
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