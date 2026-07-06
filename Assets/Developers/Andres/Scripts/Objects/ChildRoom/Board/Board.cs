using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform _PieceSpawn;
    [SerializeField] private Transform _PiecesParent;
    [SerializeField] private TilePieceData tilePieceData;

    private EventSystemChildRoom _EventSystemChildRoom;
    public List<GameObject> piecesInv = new List<GameObject>();
    public List<GameObject> createdPieces = new List<GameObject>();

    private InteractionDetector _InteractionDetector;

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
    }


    public CrosshairType GetCrosshairType(HoldController holdController) {
	    return CrosshairType.Interactable;
    }


    public void AddPieceToList(GameObject piece)
    {
        if (piecesInv.Contains(piece))
            return;

        piecesInv.Add(piece);
        CreatePieces();

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

            GameObject createdPiece = Instantiate(pieceData.prefab,_PieceSpawn.position,RandomRotation(), _PiecesParent);

            createdPieces.Add(createdPiece);
        }
    }

    private Quaternion RandomRotation()
    {
        int randomNumber = Random.Range(0,4) * 90;

        Quaternion result = Quaternion.Euler(-90f,randomNumber,0f);

        return result;
    }

}
