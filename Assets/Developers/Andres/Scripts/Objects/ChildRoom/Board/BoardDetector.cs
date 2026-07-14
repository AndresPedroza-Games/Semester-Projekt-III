using UnityEngine;

public class BoardDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        IInteractable isInteractable = collision.gameObject.GetComponent<IInteractable>();

        if (isInteractable != null && GetComponentInParent<Board>().currentPiece == null)
        {
            GetComponentInParent<Board>().AddPieceToList(collision.gameObject);
            collision.gameObject.SetActive(false);
        }
    }
}
