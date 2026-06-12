using System;


public class EventSystemPrincipalsOffice : EventSystemController {

	public new static EventSystemPrincipalsOffice Instance { get; private set; }

	public Action onFilmRotated;
	public Action onPuzzleSolved;


	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}


	public void FilmRotated() {
		onFilmRotated?.Invoke();
	}


	public void PuzzleSolved() {
		onPuzzleSolved?.Invoke();
	}

}