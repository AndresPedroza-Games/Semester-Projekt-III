using UnityEngine;

public class BoardDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<MiniPiece>(out var miniPiece) && GetComponentInParent<Board>().currentPiece == null)
        {
            GetComponentInParent<Board>().AddPieceToList(collision.gameObject);
            miniPiece.Deactivate();
        }
    }
}
