using System;
using UnityEngine;


public class EventSystemController : MonoBehaviour {

	public static EventSystemController Instance { get; private set; }

	[Header("Events")]
	//We can use to add sounds or to change the UI
	public Action<string> onDoorClosed; //for saving the last scene the player was in
	public Action<GameObject> onItemPicked;
	public Action<GameObject> onItemDropped;


	public Action onStartGame;
	public Action onSaveGame;
	public Action onExitGame;

	public Action onMainMenuEntered;

	public Action onPauseGame;
	public Action onResumeGame;


	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}


	public void StartGame() {
		onStartGame?.Invoke();
	}


	public void PauseGame() {
		onPauseGame?.Invoke();
	}


	public void MainMenuEntered() {
		onMainMenuEntered?.Invoke();
	}


	public void ResumeGame() {
		onResumeGame?.Invoke();
	}


	public void DoorClosed(string scene) {
		onDoorClosed?.Invoke(scene);
	}


	public void PickItem(GameObject item) {
		if (onItemPicked != null)
			onItemPicked.Invoke(item);
	}


	public void DropItem(GameObject item) {
		if (onItemDropped != null)
			onItemDropped.Invoke(item);
	}


	public void SaveGame() {
		if (onSaveGame != null)
			onSaveGame.Invoke();
	}


	public void ExitGame() {
		onExitGame?.Invoke();
	}

}