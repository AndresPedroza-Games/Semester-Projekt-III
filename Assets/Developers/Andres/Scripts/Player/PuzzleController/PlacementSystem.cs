using UnityEngine;
using System.Collections.Generic;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private TilePieceData _TilePieceData;
    [SerializeField] private GameObject _TilePreview;
    [SerializeField] private Grid _Grid;

    [SerializeField] private Vector2Int _GridSize = new Vector2Int(5,5);
    [SerializeField] private LayerMask layerDetector;

    private InteractionDetector _InteractionDetector;
    private EventSystemChildRoom _EventSystemChildRoom;

    private GameObject _SelectedObject;
    public bool isInteracting;

    private GridData _CubeData;
    private GridData _CubeYellowData;
    private GridData _SelectedData;
    private Board _Board;

    private Vector3Int _SnappedPos;
    private Vector3 _MousePos;
    private Vector3Int _GridPos;

    private void Awake()
    {
        _CubeData = new GridData();
        _CubeYellowData = new GridData();

        _SelectedData = _CubeData;
    }

    private void Start()
    {
        _InteractionDetector = FindAnyObjectByType<InteractionDetector>();

        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onPiecePicked += PickPiece;
        _EventSystemChildRoom.onPiecePlaced += AddToGrid;

        _Board = GetComponentInParent<Board>();
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
        _SnappedPos = Vector3Int.RoundToInt(_Grid.GetCellCenterWorld(_GridPos));

        if (_SelectedObject != null)
        {
            Vector3 lastPos = new Vector3(_SelectedObject.transform.position.x,1f, _SelectedObject.transform.position.z);
            _SelectedObject.transform.position = _SelectedData.PieceInsideGrid(_GridPos, new Vector2Int(1, 1), _GridSize) ? new Vector3(_SnappedPos.x,1f, _SnappedPos.z) : lastPos;
        }

        _TilePreview.transform.position = new Vector3(_SnappedPos.x,0f, _SnappedPos.z);

        _TilePreview.GetComponent<Renderer>().material.color = CanPlacePiece(_GridPos) ? Color.white : Color.red;
    }

    private void PickPiece(GameObject piece)
    {
        _SelectedObject = piece;
        _SelectedData.RemoveObjectAt(_GridPos, new Vector2Int(1, 1),1,1);
    }

    private void AddToGrid()
    {
        if(_SelectedObject != null && CanPlacePiece(_GridPos))
        {
            _SelectedData.AddObjectAt(_GridPos, new Vector2Int(1, 1), _SelectedObject.GetComponent<TilePiece>().pieceData.ID, 1);
            _SelectedObject = null;
            Debug.Log("Piece placed");
            Debug.Log(_GridPos);

            if (AllPiecesCorrectPosition())
                _EventSystemChildRoom.PuzzleSolved();
        }
        else
            Debug.Log("Can't place");
    }

    private bool CanPlacePiece(Vector3Int gridPos)
    {
        return _SelectedData.CanPlacePiece(gridPos, new Vector2Int(1,1), _GridSize);
    }

    private bool AllPiecesCorrectPosition()
    {
        foreach (var position in _Board.createdPieces)
        {
            Vector3Int gridPos = _Grid.WorldToCell(position.transform.position);

            if (_SelectedData.PieceCorrectPosition(gridPos, position.GetComponent<TilePiece>().pieceData.finalPos, position.GetComponent<TilePiece>().pieceData.ID))
               return true;
        }

        return false;
    }
}
