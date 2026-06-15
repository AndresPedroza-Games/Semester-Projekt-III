using System;
using UnityEngine;


[DefaultExecutionOrder(-10)]
public class EventSystemPrincipalsOffice : EventSystemController {

	public new static EventSystemPrincipalsOffice Instance { get; private set; }

	public Action<bool> onProjectorPowerButtonPressed;
	public Action<bool> onFilmSelectionButtonPressed;
	public Action onFilmRotationButtonPressed;

	public Action<bool> onPuzzleSolved;


	private void Awake() {
		if (Instance && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}


	public void PuzzleSolved(bool solved) {
		onPuzzleSolved?.Invoke(solved);
	}


	public void ProjectorPowerButtonPressed(bool state) {
		onProjectorPowerButtonPressed?.Invoke(state);
	}


	public void FilmSelectionButtonPressed(bool direction) {
		onFilmSelectionButtonPressed?.Invoke(direction);
	}


	public void FilmRotationButtonPressed() {
		onFilmRotationButtonPressed?.Invoke();
	}

}