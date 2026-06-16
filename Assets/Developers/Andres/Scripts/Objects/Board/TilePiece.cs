using UnityEngine;


public class TilePiece : MonoBehaviour, IInteractable {

	private EventSystemChildRoom _EventSystemChildRoom;

	public PieceData pieceData;


	private void Start() {
		_EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
		_EventSystemChildRoom.onPiecePlaced += PlaceTile;
		_EventSystemChildRoom.onRotatePiece += RotatePiece;
	}


	public bool CanInteract(HoldController holdController) {
		return true;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return CrosshairType.Default;
	}


	public void Interact() {
		if (PlacementSystem.isInteracting) {
			_EventSystemChildRoom.PickPiece(this.gameObject);
			Debug.Log("Piece picked");
		}
	}


	private void PlaceTile() {
		if (PlacementSystem.isInteracting)
			transform.position = new Vector3(transform.position.x, 1.15f, transform.position.z);
	}


	private void RotatePiece() {
		if (PlacementSystem.isInteracting) {
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90f, transform.rotation.eulerAngles.z);
			Debug.Log("Rotate");
		}
	}

}