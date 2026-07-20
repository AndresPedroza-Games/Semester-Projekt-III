public class Hall4Manager : RoomManager {

	private void Start() {
		if (DataManager.LastScene == gameObject.scene.name) {
			PlayerMotor player = GameManager.Instance.Player.GetComponent<PlayerMotor>();
			player.ForceCrouch();
		}
	}


	public void SaveScene() {
		EventSystemController.Instance.DoorClosed(gameObject.scene.name);
	}

}