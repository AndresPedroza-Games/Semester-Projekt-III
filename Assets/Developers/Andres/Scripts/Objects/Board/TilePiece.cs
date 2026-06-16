using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class TilePiece : MonoBehaviour, IInteractable, IHighlightable {

	private EventSystemChildRoom _EventSystemChildRoom;

    [Header("---Highlight Config---")]
    [SerializeField] private float borderThickness = 0.02f;

    public PieceData pieceData;
    private Renderer _Renderer;
    private readonly int _BorderThickness = Shader.PropertyToID("_BorderThickness");

    private float _Steps;

    private void Awake()
    {
		_Renderer = GetComponentInChildren<Renderer>();
    }

    private void Start() {
		_EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
		_EventSystemChildRoom.onPiecePlaced += PlaceTile;
		_EventSystemChildRoom.onRotatePiece += RotatePiece;
	}


	public bool CanInteract(HoldController holdController) {
		return true;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return CrosshairType.Default;
	}


	public void Interact()
    {
        _EventSystemChildRoom.PickPiece(this.gameObject);
        Highlight();

        Debug.Log("Piece picked");
    }


	private void PlaceTile()
    {
        transform.position = new Vector3(transform.position.x, 1.15f, transform.position.z);
        RemoveHighlight();
    }


    private void RotatePiece(Vector2 scroll) {
        
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