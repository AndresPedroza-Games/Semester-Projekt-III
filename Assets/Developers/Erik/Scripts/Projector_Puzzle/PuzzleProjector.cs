using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class PuzzleProjector : MonoBehaviour, IInteractable {

	[Header("---Camera---")]
	[SerializeField] private Transform puzzleCamTarget;

	[Header("---CheckBox---")]
	[SerializeField] private Transform boxCenter;
	[SerializeField] private Vector3 boxDimensions;

	[Header("---Puzzle---")]
	[SerializeField] private GameObject doorToEnableOnCompletion;
	[SerializeField] private List<Transform> filmPositions;

	private bool _solved;
	private Film[] _insertedFilms;
	private readonly Collider[] _results = new Collider[10];
	private int _insertedCount;

	private Film _selectedFilm;

	private HoldController _holdController;
	private InteractionDetector _interactionDetector;

	private CinemachineVirtualCamera _cineCam;
	private Transform _previousCamTarget;


	private void Awake() {
		_holdController = GameManager.Instance.Interactor.GetComponent<HoldController>();
		_interactionDetector = GameManager.Instance.Interactor.GetComponent<InteractionDetector>();

		InputManager.Instance.Controls.ProjectorPuzzle.Disable();

		_cineCam = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>();

		doorToEnableOnCompletion.SetActive(false);

		_insertedFilms = new Film[filmPositions.Count];
	}


	private void OnEnable() {
		InputManager.Instance.ExitProjector.performed += ExitProjector;
		InputManager.Instance.SelectFilm.performed += SelectFilm;
		InputManager.Instance.RotateFilm.performed += RotateFilm;

		EventSystemPrincipalsOffice.Instance.onFilmRotated += CheckCondition;
	}


	private void OnDisable() {
		InputManager.Instance.ExitProjector.performed -= ExitProjector;
		InputManager.Instance.SelectFilm.performed -= SelectFilm;
		InputManager.Instance.RotateFilm.performed -= RotateFilm;

		EventSystemPrincipalsOffice.Instance.onFilmRotated -= CheckCondition;
	}


	private void FixedUpdate() {
		if (_insertedCount >= filmPositions.Count)
			return;

		if (_holdController.HasObject)
			return;

		DetectFilms();
	}


	public bool CanInteract(HoldController holdController) {
		return true;
	}


	public void Interact() {
		PlayerManager.playerManager.FreezeCharacter(true);
		InputManager.Instance.Pause.Disable();
		EnterProjector();
	}


	private void EnterProjector() {
		PlayerManager.playerManager.FreezeCharacter(true);
		InputManager.Instance.Pause.Disable();
		InputManager.Instance.Controls.Interaction.Disable();

		InputManager.Instance.Controls.ProjectorPuzzle.Enable();

		_previousCamTarget = _cineCam.Follow;
		_cineCam.Follow = puzzleCamTarget;
	}


	private void ExitProjector(InputAction.CallbackContext ctx) {
		PlayerManager.playerManager.FreezeCharacter(false);
		InputManager.Instance.Pause.Enable();
		InputManager.Instance.Controls.Interaction.Enable();

		InputManager.Instance.Controls.ProjectorPuzzle.Disable();

		if (_selectedFilm) {
			_selectedFilm.RemoveHighlight();
			_selectedFilm = null;
		}

		_cineCam.Follow = _previousCamTarget;
	}


	private void ExitProjectorOnSolved() {
		PlayerManager.playerManager.FreezeCharacter(false);
		InputManager.Instance.Pause.Enable();
		InputManager.Instance.Controls.Interaction.Enable();

		InputManager.Instance.Controls.ProjectorPuzzle.Disable();

		if (_selectedFilm) {
			_selectedFilm.RemoveHighlight();
			_selectedFilm = null;
		}

		_cineCam.Follow = _previousCamTarget;
	}


	private void SelectFilm(InputAction.CallbackContext ctx) {
		if (_solved)
			return;

		if (_selectedFilm) {
			_selectedFilm.RemoveHighlight();
			_selectedFilm = null;
			return;
		}

		if (_interactionDetector.CurrentTarget == null)
			return;

		if (_interactionDetector.CurrentTarget.GetType() == typeof(Film)) {
			_selectedFilm = _interactionDetector.CurrentTarget as Film;

			if (_selectedFilm)
				_selectedFilm.Highlight();
		}
	}


	private void RotateFilm(InputAction.CallbackContext ctx) {
		if (!_selectedFilm)
			return;

		_selectedFilm.Rotate(ctx.ReadValue<Vector2>().y);
	}


	private void DetectFilms() {
		int count = Physics.OverlapBoxNonAlloc(boxCenter.position, boxDimensions, _results, boxCenter.rotation);

		for (int i = 0; i < count; i++) {

			Collider hit = _results[i];

			if (hit.TryGetComponent(out Film film)) {
				if (film.IsInserted)
					continue;

				TryInsertFilm(film);
			}
		}
	}


	private void CheckCondition() {
		if (_solved)
			return;

		foreach (Film film in _insertedFilms) {
			if (!film)
				return;

			if (film.Angle != film.CorrectAngle)
				return;
		}

		ExitProjectorOnSolved();
		EventSystemPrincipalsOffice.Instance.PuzzleSolved();
		doorToEnableOnCompletion.SetActive(true);
		_solved = true;
	}


	private void TryInsertFilm(Film film) {
		if (film.IsInserted)
			return;

		for (int i = 0; i < _insertedFilms.Length; i++) {
			if (_insertedFilms[i])
				continue;

			_insertedFilms[i] = film;

			film.Insert(filmPositions[i]);
			_insertedCount++;
			break;
		}

	}


	private void OnDrawGizmosSelected() {
		if (!boxCenter || boxDimensions == Vector3.zero)
			return;

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(boxCenter.position, boxDimensions * 2);
	}


}