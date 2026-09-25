using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;


public class Lock : MonoBehaviour {

	[Header("---Scene To Load----")]
	[SerializeField] private SceneReference _Scene;

	[Header("---Door---")]
	[SerializeField] private GameObject door;
	[SerializeField] private Vector3 openedRotation;
	[SerializeField] private Ease ease;
	[SerializeField] private float _DurationDoor = 1f;
	private Tween _rotationTween;
	private Collider _doorCollider;

	[Header("---Puzzle Config---")]
	[SerializeField] private List<int> _Password = new List<int>();
	public List<LockPiece> _LockPiecesList;

	[Header("---Top---")]
	[SerializeField] private Transform _Bar;

	[Header("---Lock Body---")]
	[SerializeField] private GameObject lockBody;

	private EventSystemDoctorOffice _EventSystemDoctorOffice;

	private List<int> _CurrentCombination = new List<int>();
	private Dictionary<int, int> _AngleToPassword = new Dictionary<int, int>();

	public static GameObject SelectedPiece;

	private CinemachineVirtualCamera _puzzleCam;
	private CinemachineVirtualCamera _playerCam;

	private Animator _animator;
	private Holdable _lockBodyHoldable;
	private Rigidbody _lockBodyRigidBody;
	private readonly List<Collider> _lockBodyColliders = new();


	private void Awake() {
		_animator = GetComponent<Animator>();

		if (lockBody.TryGetComponent(out Holdable holdable)) {
			_lockBodyHoldable = holdable;
			_lockBodyHoldable.enabled = false;
		}

		if (lockBody.TryGetComponent(out Rigidbody rb)) {
			_lockBodyRigidBody = rb;
			_lockBodyRigidBody.useGravity = false;
			_lockBodyRigidBody.isKinematic = true;
		}

		if (lockBody) {
			Component[] components = lockBody.GetComponents<Component>();

			foreach (Component comp in components) {
				if (comp is Collider col) {
					_lockBodyColliders.Add(col);
					col.enabled = false;
				}
			}
		}

		_puzzleCam = GetComponentInChildren<CinemachineVirtualCamera>(true);
		_playerCam = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>();

		_doorCollider = door.GetComponent<Collider>();
	}


	private void Start() {
		_EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
		_EventSystemDoctorOffice.onReleasePiece += PuzzleCompleted;
		_EventSystemDoctorOffice.onPuzzleCompleted += PuzzleSolved;
		_EventSystemDoctorOffice.onPieceSelected += OnPieceSelected;
		_EventSystemDoctorOffice.onReleasePiece += OnPieceRelease;
		_EventSystemDoctorOffice.onInteractWithLock += SyncSensitivity;

		_AngleToPassword = new() {
			{ 0, 5 },
			{ 36, 6 },
			{ 72, 7 },
			{ 108, 8 },
			{ 144, 9 },
			{ 180, 0 },
			{ -144, 1 },
			{ -108, 2 },
			{ -72, 3 },
			{ -36, 4 }
		};
	}


	private void OnDisable() {
		_EventSystemDoctorOffice.onReleasePiece -= PuzzleCompleted;
		_EventSystemDoctorOffice.onPuzzleCompleted -= PuzzleSolved;
		_EventSystemDoctorOffice.onPieceSelected -= OnPieceSelected;
		_EventSystemDoctorOffice.onReleasePiece -= OnPieceRelease;
		_EventSystemDoctorOffice.onInteractWithLock -= SyncSensitivity;
	}


	private void OnPieceSelected(GameObject obj) {
		SelectedPiece = obj;
	}


	private void OnPieceRelease() {
		SelectedPiece = null;
	}


	private void PuzzleCompleted() {
		if (CheckIfPuzzleCompleted()) {
			_animator?.Play("Lock_Solved_Anim");
		}
	}


	private void OnSolvedAnimationEnd() {
		ActivatePhysics();
		_EventSystemDoctorOffice.PuzzleCompleted();
	}


	private bool CheckIfPuzzleCompleted() {
		_CurrentCombination.Clear();

		foreach (LockPiece piece in _LockPiecesList) {
			float angle = piece._Steps * 36f;
			int currentAngle = Mathf.RoundToInt(Mathf.DeltaAngle(0f, angle));
			int number = _AngleToPassword[currentAngle];
			_CurrentCombination.Add(number);
		}

		for (int number = 0; number < _Password.Count; number++) {
			if (_CurrentCombination[number] != _Password[number])
				return false;
		}

		return true;
	}


	private void PuzzleSolved() {
		SelectedPiece = null;

		if (_Scene != null)
			LoadScene();

		OpenDoor();
	}


	private void OpenDoor() {
		_doorCollider.enabled = false;
		_rotationTween = door.transform.DOLocalRotateQuaternion(Quaternion.Euler(openedRotation), _DurationDoor).SetEase(ease).SetLink(gameObject);

		_rotationTween.OnComplete(() => { _doorCollider.enabled = true; });
	}


	private async void LoadScene() {
		await WorldSceneManager.Instance.LoadScene(_Scene);
	}


	private void SyncSensitivity() {
		_puzzleCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed =
			_playerCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
		_puzzleCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed =
			_playerCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
	}


	private void ActivatePhysics() {
		_animator.enabled = false;

		foreach (Collider col in _lockBodyColliders) {
			col.enabled = true;
		}

		if (lockBody.TryGetComponent(out Rigidbody rb)) {
			rb.useGravity = true;
			rb.isKinematic = false;
		}

		if (_lockBodyHoldable)
			_lockBodyHoldable.enabled = true;
	}

}