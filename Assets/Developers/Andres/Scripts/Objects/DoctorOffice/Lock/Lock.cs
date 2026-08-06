using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class Lock : MonoBehaviour
{
    [SerializeField] private SceneReference _Scene;

    [SerializeField] private List<int> _Password = new List<int>();
    public List<LockPiece> _LockPiecesList;

    private EventSystemDoctorOffice _EventSystemDoctorOffice;

    private List<int> _CurrentCombination = new List<int>();
    private Dictionary<int, int> _AngleToPassword = new Dictionary<int, int>();

    public static GameObject SelectedPiece;

    
    private CinemachineVirtualCamera _puzzleCam;
    private CinemachineVirtualCamera _playerCam;


    private void Awake() {
	    _puzzleCam = GetComponentInChildren<CinemachineVirtualCamera>(true);
	    _playerCam = GameManager.Instance.Camera.GetComponent<CinemachineVirtualCamera>();
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
            _EventSystemDoctorOffice.PuzzleCompleted();
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

        Debug.Log("Puzzle Completed");
    }

    private async void LoadScene()
    {
        await WorldSceneManager.Instance.LoadScene(_Scene);
    }
    
    private void SyncSensitivity() {
	    _puzzleCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = _playerCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
	    _puzzleCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = _playerCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
    }
    
}
