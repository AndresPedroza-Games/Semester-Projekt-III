using UnityEngine;
using System.Collections.Generic;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private TilePieceData _TilePieceData;
    [SerializeField] private GameObject _TilePreview;
    [SerializeField] private Grid _Grid;

    [SerializeField] private Vector2 _GridSize;
    [SerializeField] private LayerMask layerDetector;

    private InteractionDetector _InteractionDetector;
    private EventSystemChildRoom _EventSystemChildRoom;

    private List<PieceData> _PiecesPlaced = new List<PieceData>();

    private GameObject _SelectedObject;
    public static bool isInteracting;

    private GridData _PieceData;

    private Vector3 _SnappedPos;
    private Vector3 _MousePos;
    private Vector3Int _GridPos;

    private int _SelectedObjectIndex;

    private void Awake()
    {
        _PieceData = new GridData();
    }

    private void Start()
    {
        _InteractionDetector = FindAnyObjectByType<InteractionDetector>(FindObjectsInactive.Include);

        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onPiecePicked += PickPiece;
        _EventSystemChildRoom.onPiecePlaced += AddToGrid;
        _EventSystemChildRoom.onExitBoard += () => _TilePreview.SetActive(false);
    }

    private void Update()
    {
       if (isInteracting)
            MousePosition();
    }

    private void MousePosition()
    {
        _TilePreview.SetActive(true);

        _MousePos = _InteractionDetector.GetRayPosition(layerDetector);
        _GridPos = _Grid.WorldToCell(_MousePos);
        _SnappedPos = _Grid.GetCellCenterWorld(_GridPos);

        if (_SelectedObject != null)
        {
            Vector3 lastPos = new Vector3(_SelectedObject.transform.position.x, _Grid.gameObject.transform.position.y, _SelectedObject.transform.position.z);
            _SelectedObject.transform.position = _PieceData.PieceInsideGrid(_GridPos, _TilePieceData.piecesData[_SelectedObjectIndex].size, _GridSize) ? new Vector3(_SnappedPos.x,_Grid.gameObject.transform.position.y + 0.05f, _SnappedPos.z) : lastPos;
        }

        _TilePreview.transform.position = new Vector3(_SnappedPos.x, _Grid.gameObject.transform.position.y + 0.05f, _SnappedPos.z);

        _TilePreview.GetComponent<Renderer>().material.color = CanPlacePiece(_GridPos) ? Color.white : Color.red;
    }

    private void PickPiece(GameObject piece)
    {
        _SelectedObject = piece;
        _PieceData.RemoveObjectAt(_GridPos, _TilePieceData.piecesData[_SelectedObjectIndex].size);

        TilePiece selectedPiece = _SelectedObject.GetComponent<TilePiece>();

        if (selectedPiece != null)
        {
            int targetID = selectedPiece.pieceData.ID;

            _SelectedObjectIndex = _TilePieceData.piecesData.FindIndex(data => data.ID == targetID);
        }

        if (_PiecesPlaced.Contains(_TilePieceData.piecesData[_SelectedObjectIndex]))
            _PiecesPlaced.Remove(_TilePieceData.piecesData[_SelectedObjectIndex]);
    }

    private void AddToGrid()
    {
        if(_SelectedObject != null && CanPlacePiece(_GridPos))
        {
            _PieceData.AddObjectAt(_GridPos, _TilePieceData.piecesData[_SelectedObjectIndex].size, _TilePieceData.piecesData[_SelectedObjectIndex].ID, _SelectedObjectIndex);
            _TilePieceData.piecesData[_SelectedObjectIndex].currentPos = _GridPos;
            
            if(!_PiecesPlaced.Contains(_TilePieceData.piecesData[_SelectedObjectIndex]))
                _PiecesPlaced.Add(_TilePieceData.piecesData[_SelectedObjectIndex]);

            Debug.Log("Piece placed");
            Debug.Log(_GridPos);

            if (AllPiecesCorrectPosition())
            {
                _EventSystemChildRoom.PuzzleSolved();
                Debug.Log("Puzzle Solved");
            }

            _SelectedObject = null;
        }
        else
            Debug.Log("Can't place");
    }

    private bool CanPlacePiece(Vector3 gridPos)
    {
        return _PieceData.CanPlacePiece(gridPos, _TilePieceData.piecesData[_SelectedObjectIndex].size, _GridSize);
    }

    private bool AllPiecesCorrectPosition()
    {
        if (_PiecesPlaced.Count != _TilePieceData.piecesData.Count)
            return false;

        foreach (PieceData pieceData in _PiecesPlaced)
        {
            if (!_PieceData.PieceCorrectPosition(pieceData.currentPos, pieceData.correctPos))
                return false;
        }

        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IInteractable isInteractable = collision.gameObject.GetComponent<IInteractable>();

        if (isInteractable != null)
        {
            GetComponentInParent<Board>().AddPieceToList(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }
}
