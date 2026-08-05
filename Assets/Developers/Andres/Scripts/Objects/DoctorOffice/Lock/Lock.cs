using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Lock : MonoBehaviour
{
    [SerializeField] private SceneReference _Scene;

    [SerializeField] private List<int> _Password = new List<int>();
    public List<LockPiece> _LockPiecesList;

    private EventSystemDoctorOffice _EventSystemDoctorOffice;

    private List<int> _CurrentCombination = new List<int>();
    private Dictionary<int, int> _AngleToPassword = new Dictionary<int, int>();

    private void Start()
    {
        _EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
        _EventSystemDoctorOffice.onReleasePiece += PuzzleCompleted;
        _EventSystemDoctorOffice.onPuzzleCompleted += PuzzleSolved;

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

    private void PuzzleCompleted()
    {
        if (CheckIfPuzzleCompleted())
        {
            _EventSystemDoctorOffice.PuzzleCompleted();
            GameManager.Instance.miniGameActive = false;
        }
    }

    private bool CheckIfPuzzleCompleted()
    {
        _CurrentCombination.Clear();

        foreach (LockPiece piece in _LockPiecesList)
        {
            float angle = piece._Steps * 36f;
            int currentAngle = Mathf.DeltaAngle(0f, angle).ConvertTo<int>();
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

    private void PuzzleSolved()
    {
        if (_Scene != null)
            LoadScene();

        Debug.Log("Puzzle Completed");
    }

    private async void LoadScene()
    {
        await WorldSceneManager.Instance.LoadScene(_Scene);
    }
}
