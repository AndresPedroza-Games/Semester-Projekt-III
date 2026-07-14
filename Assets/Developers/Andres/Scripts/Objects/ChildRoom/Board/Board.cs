using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform _PieceSpawn;
    [SerializeField] private Transform _PiecesParent;
    [SerializeField] private TilePieceData tilePieceData;
    [SerializeField] private Transform _Drawer;

    [Header("Animation Settings")]
    [SerializeField] private Ease _Ease;
    [SerializeField] private float _Duration = 0.3f;
    private float _Angle;

    private EventSystemChildRoom _EventSystemChildRoom;
    public List<GameObject> piecesInv = new List<GameObject>();
    public List<GameObject> createdPieces = new List<GameObject>();

    private InteractionDetector _InteractionDetector;
    private GameObject _CurrentPiece;
    private PlacementSystem _PlacementSystem;

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onPuzzleSolved += OpenDrawer;

        _PlacementSystem = PlacementSystem.Instace;
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

            _CurrentPiece = Instantiate(pieceData.prefab, _PieceSpawn.position, RandomRotation(), _PiecesParent);

            createdPieces.Add(_CurrentPiece);

            _EventSystemChildRoom.PlacePiece();
        }
    }

    private Quaternion RandomRotation()
    {
        Quaternion result = Quaternion.Euler(-90f,0f,0f);

        return result;
    }

    private void OpenDrawer()
    {
        Debug.Log("Drawer opened");
        AnimateVisuals(_Ease, _Duration);
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        Sequence seq = DOTween.Sequence();

        seq.SetEase(ease);

        seq.Join(_Drawer.DOLocalMoveX(0.5f, duration));
    }

}
