using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;


public class Lock : MonoBehaviour
{
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
    [SerializeField] private float _DurationTop = 0.1f;

    private EventSystemDoctorOffice _EventSystemDoctorOffice;

    private List<int> _CurrentCombination = new List<int>();
    private Dictionary<int, int> _AngleToPassword = new Dictionary<int, int>();

    public static GameObject SelectedPiece;
    
    private CinemachineVirtualCamera _puzzleCam;
    private CinemachineVirtualCamera _playerCam;

    private float _MoveUpAnim = 0.006f;

    private void Awake() 
    {
        _puzzleCam = GetComponentInChildren<CinemachineVirtualCamera>(true);
        _playerCam = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>();

        _doorCollider = door.GetComponent<Collider>();
    }


    private void Start()
    {
        _EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
        _EventSystemDoctorOffice.onReleasePiece += PuzzleCompleted;
        _EventSystemDoctorOffice.onPuzzleCompleted += PuzzleSolved;
        _EventSystemDoctorOffice.onPieceSelected += OnPieceSelected;
        _EventSystemDoctorOffice.onReleasePiece += OnPieceRelease;
        _EventSystemDoctorOffice.onInteractWithLock += SyncSensitivity;

        _AngleToPassword = new()
        {
            {0,5},
            {36,6},
            {72,7},
            {108,8},
            {144,9},
            {180,0},
            {-144,1},
            {-108,2},
            {-72,3},
            {-36,4}
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

    
    private void PuzzleCompleted()
    {
        if (CheckIfPuzzleCompleted())
        {
            MoveUpAnim(ease, _DurationTop);
            _EventSystemDoctorOffice.PuzzleCompleted();
            ActivatePhysics();
        }
    }

    private bool CheckIfPuzzleCompleted()
    {
        _CurrentCombination.Clear();

        foreach (LockPiece piece in _LockPiecesList)
        {
            float angle = piece._Steps * 36f;
            int currentAngle =Mathf.RoundToInt(Mathf.DeltaAngle(0f, angle));
            int number = _AngleToPassword[currentAngle];
            _CurrentCombination.Add(number);
        }

        for (int number = 0; number < _Password.Count; number++)
        {
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

	    _rotationTween.OnComplete(() => {
		    _doorCollider.enabled = true;
	    });
    }

    private async void LoadScene()
    {
        await WorldSceneManager.Instance.LoadScene(_Scene);
    }
    
    private void SyncSensitivity() {
	    _puzzleCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = _playerCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
	    _puzzleCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = _playerCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
    }

    private void MoveUpAnim(Ease ease, float duration)
    {
        _Bar.DOLocalMoveZ(_MoveUpAnim, duration).SetEase(ease).OnComplete(() => RotateAnim(ease,duration));
    }

    private void RotateAnim(Ease ease, float duration)
    {
        _Bar.DOLocalRotateQuaternion(Quaternion.Euler(0f, 180f, 0f), duration).SetEase(ease);
    }

    private void ActivatePhysics()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }

}
