using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _Camera;
    [SerializeField] private Transform _PieceSpawn;
    [SerializeField] private TilePieceData tilePieceData;

    private EventSystemChildRoom _EventSystemChildRoom;
    public List<GameObject> piecesInv = new List<GameObject>();
    public List<GameObject> createdPieces = new List<GameObject>();

    private InteractionDetector _InteractionDetector;

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onExitBoard += ExitBoard;
    }

    public void Interact()
    {
       _EventSystemChildRoom.InteractWithBoard();
        PlacementSystem.isInteracting = true;
        _Camera.SetActive(true);

        _InteractionDetector = FindAnyObjectByType<InteractionDetector>(FindObjectsInactive.Include);
        _InteractionDetector.interactionDistance = 4f;
        
        InputManager.Instance.Pause.Disable();
        GameManager.miniGameActive = true;
    }

    public bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }

    public void AddPieceToList(GameObject piece)
    {
        if (piecesInv.Contains(piece))
            return;

        piecesInv.Add(piece);
        CreatePieces();
        //piece.GetComponent<Holdable>().

        Debug.Log("Piece added to list");
    }

    private void CreatePieces()
    {
        if (piecesInv.Count == 0)
            return;

        foreach (var pieceInv in piecesInv)
        {
            MiniPiece miniPiece = pieceInv.GetComponent<MiniPiece>();

            if (miniPiece == null)
                continue;

            var pieceData = tilePieceData.piecesData
                .FirstOrDefault(p => p.prefab == miniPiece.pieceData.prefab);

            if (pieceData == null)
                continue;

            bool alreadyCreated = createdPieces
                .Any(p => p.name.Contains(pieceData.prefab.name));

            if (alreadyCreated)
                continue;

            GameObject createdPiece = Instantiate(pieceData.prefab,_PieceSpawn.position,RandomRotation());

            createdPieces.Add(createdPiece);
        }
    }

    private void ExitBoard()
    {
        _Camera.SetActive(false);
        PlacementSystem.isInteracting = false;
        
        InputManager.Instance.Pause.Enable();
        GameManager.miniGameActive = false;
    }

    private Quaternion RandomRotation()
    {
        int randomNumber = Random.Range(0,4) * 90;

        Quaternion result = Quaternion.Euler(0f,randomNumber,0f);

        return result;
    }

}
