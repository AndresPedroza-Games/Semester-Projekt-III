using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform _PieceSpawn;
    [SerializeField] private Transform _PiecesParent;
    [SerializeField] private TilePieceData tilePieceData;
    [SerializeField] private Transform _Drawer;
    [SerializeField] private SceneReference _Scene;

    [Header("Animation Settings")]
    [SerializeField] private Ease _Ease;
    [SerializeField] private float _Duration = 0.3f;

    private EventSystemChildRoom _EventSystemChildRoom;
    public List<GameObject> piecesInv = new List<GameObject>();
    public List<GameObject> createdPieces = new List<GameObject>();

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
        Debug.Log("Drawer opened");
        AnimateVisuals(_Ease, _Duration);
        LoadScene();


    }

    private async void LoadScene()
    {
        if(_Scene != null)
            await WorldSceneManager.Instance.LoadScene(_Scene);
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        Sequence seq = DOTween.Sequence();

        seq.SetEase(ease);

        seq.Join(_Drawer.DOLocalMoveX(-0.5f, duration));
    }

}
