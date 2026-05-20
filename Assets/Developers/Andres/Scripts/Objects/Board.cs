using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _CameraPosition;
    [SerializeField] private Transform _PuzzleBoard;
    [SerializeField] private Transform _PieceSpawn;
    [SerializeField] private TilePieceData tilePieceData;

    private EventSystemChildRoom _EventSystemChildRoom;
    private List<GameObject> _PiecesInv = new List<GameObject>();
    private List<GameObject> _CreatedPieces = new List<GameObject>();

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onExitBoard += () => GetComponentInChildren<PlacementSystem>().isInteracting = false;
    }

    public void Interact()
    {
       _EventSystemChildRoom.InteractWithBoard(_CameraPosition, _PuzzleBoard);
        GetComponentInChildren<PlacementSystem>().isInteracting = true;
        CreatePieces();
    }

    public bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }

    private void AddPieceToList(GameObject piece)
    {
        if (_PiecesInv.Contains(piece))
            return;

        _PiecesInv.Add(piece);
        piece.GetComponent<Holdable>().enabled = false;

        Debug.Log("Piece added to list");
    }

    private void CreatePieces()
    {
        if (_PiecesInv.Count == 0)
            return;

        foreach (var pieceInv in _PiecesInv)
        {
            MiniPiece miniPiece = pieceInv.GetComponent<MiniPiece>();

            if (miniPiece == null)
                continue;

            var pieceData = tilePieceData.piecesData
                .FirstOrDefault(p => p.prefab == miniPiece.pieceData.prefab);

            if (pieceData == null)
                continue;

            bool alreadyCreated = _CreatedPieces
                .Any(p => p.name.Contains(pieceData.prefab.name));

            if (alreadyCreated)
                continue;

            GameObject createdPiece = Instantiate(pieceData.prefab,_PieceSpawn.position,Quaternion.identity);

            _CreatedPieces.Add(createdPiece);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IInteractable isInteractable = collision.gameObject.GetComponent<IInteractable>();

        if (isInteractable != null)
            AddPieceToList(collision.gameObject);
    }
}
