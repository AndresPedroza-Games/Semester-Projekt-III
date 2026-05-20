using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private TilePieceData _TilePieceData;
    [SerializeField] private GameObject _TilePreview;
    [SerializeField] private Grid _Grid;
    [SerializeField] private LayerMask layerDetector;

    private InteractionDetector _InteractionDetector;
    private EventSystemChildRoom _EventSystemChildRoom;

    private GameObject _SelectedObject;
    public bool isInteracting;

    private GridData _CubeData;
    private GridData _CubeYellowData;
    private GridData _SelectedData;

    private Vector3 _SnappedPos;
    private Vector3 _MousePos;
    private Vector3Int _GridPos;

    private void Awake()
    {
        _CubeData = new GridData();
        _CubeYellowData = new GridData();


    }

    private void Start()
    {
        _InteractionDetector = FindAnyObjectByType<InteractionDetector>();

        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onPiecePicked += PickPiece;
        _EventSystemChildRoom.onPiecePlaced += AddToGrid;
    }

    private void Update()
    {
       if (isInteracting)
            MousePosition();
    }

    private void MousePosition()
    {
        _MousePos = _InteractionDetector.GetRayPosition(layerDetector);
        _GridPos = _Grid.WorldToCell(_MousePos);
        _SnappedPos = _Grid.GetCellCenterWorld(_GridPos);
        
        if(_SelectedObject != null)
            _SelectedObject.transform.position = new Vector3(_SnappedPos.x,1f, _SnappedPos.z);

        _TilePreview.transform.position = _SnappedPos;

        _TilePreview.GetComponent<Renderer>().material.color = CanPlacePiece(_GridPos) ? Color.white : Color.red;
    }

    private void PickPiece(GameObject piece)
    {
        _SelectedObject = piece;
    }

    private void AddToGrid()
    {
        if(_SelectedObject != null && CanPlacePiece(_GridPos))
        {
            _SelectedObject = null;
            _SelectedData.AddObjectAt(_GridPos, new Vector2Int(1, 1),1,1);
            Debug.Log("Piece placed");
        }
        else
            Debug.Log("Can't place");
    }

    private bool CanPlacePiece(Vector3Int gridPos)
    {
        _SelectedData = _CubeData;

        return _SelectedData.CanPlacePiece(gridPos, new Vector2Int(1,1));
    }
}
