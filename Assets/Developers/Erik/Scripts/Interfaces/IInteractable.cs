public interface IInteractable {

	public void Interact();

	public bool CanInteract(HoldController holdController);

	public CrosshairType GetCrosshairType(HoldController holdController);

}