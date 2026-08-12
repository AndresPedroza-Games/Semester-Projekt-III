using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Board : MonoBehaviour
{
    [SerializeField] private Transform _PieceSpawn;
    [SerializeField] private Transform _PiecesParent;
    [SerializeField] private TilePieceData tilePieceData;
    [SerializeField] private Transform _Drawer;
    [SerializeField] private SceneReference _Scene;

    private EventSystemChildRoom _EventSystemChildRoom;
    public List<GameObject> piecesInv = new List<GameObject>();
    public List<GameObject> createdPieces = new List<GameObject>();

    [SerializeField] [Range(1, 9)] private int ballerinaEventTriggerPieceCount;

    public GameObject currentPiece;

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onPuzzleSolved += OpenDrawer;

        currentPiece = null;
    }

    public void AddPieceToList(GameObject piece)
    {
        if (piecesInv.Contains(piece))
            return;

        piecesInv.Add(piece);

        if (piecesInv.Count == ballerinaEventTriggerPieceCount) 
	        EventSystemChildRoom.eventSystemChildRoom.PuzzlePieceEventTrigger();
        
        CreatePieces();
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

            currentPiece = Instantiate(pieceData.prefab, _PieceSpawn.position, RandomRotation(), _PiecesParent);

            createdPieces.Add(currentPiece);

            _EventSystemChildRoom.PlacePiece();
        }
    }

    private Quaternion RandomRotation()
    {
        Quaternion result = Quaternion.Euler(0f,-90f,0f);

        return result;
    }

    private void OpenDrawer()
    {
        LoadScene();
    }

    private async void LoadScene()
    {
        if(_Scene != null)
            await WorldSceneManager.Instance.LoadScene(_Scene);
    }

}
