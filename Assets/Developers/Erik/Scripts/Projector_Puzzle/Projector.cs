using System;
using System.Collections.Generic;
using UnityEngine;


public class Projector : MonoBehaviour {

	[Header("---Puzzle Config---")]
	[SerializeField] private GameObject doorToEnableOnCompletion;
	[SerializeField] private List<Film> films;
	private readonly List<int> _correctAngles = new List<int>();


	private void Awake() {
		doorToEnableOnCompletion.SetActive(false);

		foreach (Film film in films) {
			_correctAngles.Add(film.CorrectAngle);
		}
	}


	private void OnEnable() {
		EventSystemPrincipalsOffice.Instance.onFilmRotated += OnFilmRotated;
	}


	private void OnDisable() {
		EventSystemPrincipalsOffice.Instance.onFilmRotated -= OnFilmRotated;

	}


	private void OnFilmRotated() {
		if (CheckCondition()) {
			EventSystemPrincipalsOffice.Instance.PuzzleCompleted();
			doorToEnableOnCompletion.SetActive(true);
		}
	}


	private bool CheckCondition() {
		for (int i = 0; i < films.Count; i++) {
			if (films[i].Angle != _correctAngles[i]) 
				return false;
		}

		Debug.Log("Correct Rotations");
		return true;
	}

}