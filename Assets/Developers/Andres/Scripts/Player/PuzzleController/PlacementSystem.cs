using System.Collections.Generic;
using UnityEngine;


public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance;

    [SerializeField] private TilePieceData _TilePieceData;
    [SerializeField] private GameObject _TilePreview;
    [SerializeField] private Grid _Grid;

    [SerializeField] private Vector2 _GridMinSize;
    [SerializeField] private Vector2 _GridMaxSize;
    [SerializeField] private LayerMask layerDetector;

    private InteractionDetector _InteractionDetector;
    private EventSystemChildRoom _EventSystemChildRoom;

    private List<PieceData> _PiecesPlaced = new List<PieceData>();

    public GameObject _SelectedObject;

    private GridData _PieceData;
    private Board _Board;

    private Vector3 _SnappedPos;
    private Vector3 _MousePos;
    private Vector3Int _GridPos;
    private Vector3 _LastPos;

    private int _SelectedObjectIndex;
    private bool _PuzzleSolved;

    private void Awake()
    {
        _PieceData = new GridData();
        _Board = GetComponentInParent<Board>();

        _PuzzleSolved = false;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _InteractionDetector = FindAnyObjectByType<InteractionDetector>(FindObjectsInactive.Include);

        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onPiecePicked += PickPiece;
        _EventSystemChildRoom.onPiecePlaced += OnPiecePlaced;
        _EventSystemChildRoom.onPuzzleSolved += () => _TilePreview.SetActive(false);
    }

    private void OnDestroy()
    {
        if (!_EventSystemChildRoom)
            return;

        _EventSystemChildRoom.onPiecePicked -= PickPiece;
        _EventSystemChildRoom.onPiecePlaced -= OnPiecePlaced;
    }

    private void Update()
    {
        MousePosition();
    }

    private void MousePosition()
    {
        if (_Board.createdPieces.Count <= 0)
            return;

        _TilePreview.SetActive(true);

        _MousePos = _InteractionDetector.GetRayPosition(layerDetector);
        _GridPos = _Grid.WorldToCell(_MousePos);

        _SnappedPos = _Grid.GetCellCenterWorld(_GridPos);

        if (_SelectedObject)
        {
            _LastPos = new Vector3(_SelectedObject.transform.position.x, _Grid.gameObject.transform.position.y, _SelectedObject.transform.position.z);
            _SelectedObject.transform.position = _PieceData.PieceInsideGrid(_GridPos, _GridMinSize, _GridMaxSize) ? new Vector3(_SnappedPos.x,_Grid.gameObject.transform.position.y + 0.05f, _SnappedPos.z) : _LastPos;
        }

        Vector3 lastPosTilePreview = new Vector3(_TilePreview.transform.position.x, _Grid.gameObject.transform.position.y, _TilePreview.transform.position.z);
        _TilePreview.transform.position = _PieceData.PieceInsideGrid(_GridPos, _GridMinSize, _GridMaxSize) ? new Vector3(_SnappedPos.x, _Grid.gameObject.transform.position.y + 0.05f, _SnappedPos.z) : lastPosTilePreview;

        _TilePreview.GetComponent<Renderer>().material.color = CanPlacePiece(_GridPos) ? Color.white : Color.lightBlue;
    }

    private void PickPiece(GameObject piece)
    {
        if (_SelectedObject)
            return;

        _SelectedObject = piece;

        TilePiece selectedPiece = _SelectedObject.GetComponent<TilePiece>();

        if (selectedPiece != null)
        {
            int targetID = selectedPiece.pieceData.ID;

            _SelectedObjectIndex = _TilePieceData.piecesData.FindIndex(data => data.ID == targetID);
        }

        if (_PiecesPlaced.Contains(_TilePieceData.piecesData[_SelectedObjectIndex]))
        {
            _PieceData.RemoveObjectAt(_TilePieceData.piecesData[_SelectedObjectIndex].currentPos);
            _PiecesPlaced.Remove(_TilePieceData.piecesData[_SelectedObjectIndex]);
        }
    }

    private void OnPiecePlaced()
    {
        AddToGrid(_GridPos);
    }

    private void AddToGrid(Vector3Int gridPos)
    {
        if (!_SelectedObject)
            return;

        GameObject piece = _PieceData.PieceInThisPosition(gridPos);

        if (piece == null)
        {
            PlaceSelected(gridPos);

            if (AllPiecesCorrectPosition() && !_PuzzleSolved)
            {
                _EventSystemChildRoom.PuzzleSolved();
                Debug.Log("Puzzle Solved");
                _PuzzleSolved = true;
            }

            return;
        }

        SwitchPieces(piece, gridPos);
    }

    private void PlaceSelected(Vector3Int gridPos)
    {
        _PieceData.AddObjectAt(_SelectedObject, gridPos, Mathf.RoundToInt(_SelectedObject.transform.localEulerAngles.y), _TilePieceData.piecesData[_SelectedObjectIndex].ID, _SelectedObjectIndex);

        _TilePieceData.piecesData[_SelectedObjectIndex].currentPos = gridPos;

        _PiecesPlaced.Add(_TilePieceData.piecesData[_SelectedObjectIndex]);

        _Board.currentPiece = null;
        _SelectedObject = null;
    }

    private void SwitchPieces(GameObject piece, Vector3Int gridPos)
    {
        PieceData oldData = piece.GetComponent<TilePiece>().pieceData;

        _PieceData.RemoveObjectAt(oldData.currentPos);
        _PiecesPlaced.Remove(oldData);

        PlaceSelected(gridPos);

        piece.GetComponent<TilePiece>().SelectTile();
    }

    private bool CanPlacePiece(Vector3 gridPos)
    {
        return _PieceData.CanPlacePiece(gridPos, _GridMinSize, _GridMaxSize);
    }

    private bool AllPiecesCorrectPosition()
    {
        if (_PiecesPlaced.Count != _TilePieceData.piecesData.Count)
            return false;

        foreach (PieceData pieceData in _PiecesPlaced)
        {
            if (!_PieceData.PieceCorrectPosition(pieceData.currentPos, pieceData.correctPos) || !_PieceData.PieceCorrectRotation(pieceData.currentPos))
                return false;
        }

        return true;
    }
}
