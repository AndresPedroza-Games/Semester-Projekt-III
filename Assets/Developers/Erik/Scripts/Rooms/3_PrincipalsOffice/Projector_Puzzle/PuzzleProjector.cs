using System.Collections.Generic;
using UnityEngine;


public class PuzzleProjector : MonoBehaviour, IInteractable {

#if UNITY_EDITOR
	[Header("---Editor---")]
	public bool alwaysShowGizmos;
#endif

	[Header("---Projector---")]
	[SerializeField] private List<Light> projectorLights;
	[SerializeField] private int correctProjectorAngle;
	private bool _isOn;

	[Header("---CheckBox---")]
	[SerializeField] private Transform boxCenter;
	[SerializeField] private Vector3 boxDimensions;

	[Header("---Films---")]
	[SerializeField] private List<Transform> filmPositions;

	private Film[] _insertedFilms;
	private readonly Collider[] _results = new Collider[10];
	private int _insertedCount;
	private Film _selectedFilm;
	private int _selectedFilmIndex;

	private int _projectorAngle;
	private const int RotationIncrement = 90;

	private bool _solved;

	private HoldController _holdController;


	private void Awake() {
		_holdController = GameManager.Instance.Interactor.GetComponent<HoldController>();

		SetLights(false);

		_insertedFilms = new Film[filmPositions.Count];
	}


	private void OnEnable() {
		EventSystemPrincipalsOffice.Instance.onProjectorPowerButtonPressed += OnPowerButtonPressed;
		EventSystemPrincipalsOffice.Instance.onFilmSelectionButtonPressed += OnFilmSelectionButtonPressed;
		EventSystemPrincipalsOffice.Instance.onFilmRotationButtonPressed += RotateFilm;
	}


	private void OnDisable() {
		EventSystemPrincipalsOffice.Instance.onProjectorPowerButtonPressed -= OnPowerButtonPressed;
		EventSystemPrincipalsOffice.Instance.onFilmSelectionButtonPressed -= OnFilmSelectionButtonPressed;
		EventSystemPrincipalsOffice.Instance.onFilmRotationButtonPressed -= RotateFilm;
	}


	private void FixedUpdate() {
		if (_insertedCount >= filmPositions.Count)
			return;

		if (_holdController.HasObject)
			return;

		DetectFilms();
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject && !_solved;
	}


	public void Interact() {
		RotateProjector();
	}


	private void SetLights(bool state) {
		foreach (Light l in projectorLights)
			l.enabled = state;
	}


	private void SetAllDecals(bool state) {
		foreach (Film film in _insertedFilms) {
			if (film)
				film.Decal.SetActive(state);
		}
	}


	private void OnPowerButtonPressed(bool isOn) {
		if (_solved && _isOn)
			return;

		_isOn = isOn;

		SetLights(isOn);

		if (_selectedFilm && !_solved)
			if (isOn)
				_selectedFilm.Highlight();
			else
				_selectedFilm.RemoveHighlight();

		SetAllDecals(isOn);

		CheckCondition();
	}


	private void RotateProjector() {
		if (_solved)
			return;

		_projectorAngle = (_projectorAngle + RotationIncrement + 360) % 360;

		transform.rotation = Quaternion.Euler(0f, _projectorAngle, 0f);

		CheckCondition();
	}


	private void OnFilmSelectionButtonPressed(bool upwards) {
		if (!_isOn || _solved || _insertedCount == 0)
			return;

		_selectedFilm?.RemoveHighlight();

		int direction = upwards ? 1 : -1;

		_selectedFilmIndex = (_selectedFilmIndex + direction + _insertedCount) % _insertedCount;

		_selectedFilm = _insertedFilms[_selectedFilmIndex];

		_selectedFilm?.Highlight();
	}


	private void RotateFilm() {
		if (_solved || !_isOn || !_selectedFilm)
			return;

		_selectedFilm.Rotate();
		CheckCondition();
	}


	private void DetectFilms() {
		int count = Physics.OverlapBoxNonAlloc(boxCenter.position, boxDimensions, _results, boxCenter.rotation);

		for (int i = 0; i < count; i++) {
			
			if (_insertedCount >= _insertedFilms.Length)
				break;

			Collider hit = _results[i];

			if (hit.TryGetComponent(out Film film)) {
				if (film.IsInserted)
					continue;

				TryInsertFilm(film);
			}
		}
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

			if (_selectedFilm)
				_selectedFilm.RemoveHighlight();

			if (_isOn) {
				film.Highlight();
				film.Decal.SetActive(true);
			}

			_selectedFilm = film;
			_selectedFilmIndex = i;

			break;
		}

	}


	private void CheckCondition() {
		if (_solved || !_isOn)
			return;

		if (_projectorAngle != correctProjectorAngle)
			return;

		foreach (Film film in _insertedFilms) {
			if (!film)
				return;

			if (film.Angle != film.CorrectAngle)
				return;
		}

		Solve();
	}


	private void Solve() {
		_solved = true;
		_selectedFilm?.RemoveHighlight();
		EventSystemPrincipalsOffice.Instance.PuzzleSolved(true);
	}


	private void OnDrawGizmos() {
		if (!alwaysShowGizmos)
			return;

		if (!boxCenter || boxDimensions == Vector3.zero)
			return;

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(boxCenter.position, boxDimensions * 2);

		Gizmos.color = Color.green;
		Vector3 dimension = new Vector3(0.2f, 0.002f, 0.3f);
		foreach (Transform film in filmPositions) {
			Gizmos.DrawWireCube(film.transform.position, dimension);
		}
	}

}