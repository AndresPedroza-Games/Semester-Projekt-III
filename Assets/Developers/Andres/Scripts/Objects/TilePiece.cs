using UnityEngine;

public class TilePiece : MonoBehaviour, IInteractable
{
    private EventSystemChildRoom _EventSystemChildRoom;

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onPiecePlaced += PlaceTile;
        _EventSystemChildRoom.onRotatePiece += RotatePiece;
    }

    public bool CanInteract(HoldController holdController)
    {
        return true;
    }

    public void Interact()
    {
        _EventSystemChildRoom.PickPiece(this.gameObject);
        Debug.Log("Piece picked");
    }

    private void PlaceTile()
    {
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    private void RotatePiece()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90f, transform.rotation.eulerAngles.z);
        Debug.Log("Rotate");
    }
}
