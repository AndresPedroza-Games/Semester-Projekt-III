using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TestNoteController : MonoBehaviour
{
    [SerializeField] private List<TestNoteQuestion> _TestAnswers = new List<TestNoteQuestion>();
    [SerializeField] private float _CoolDownRestart = 1f;

    private bool _IsCompleted;

    private void Awake()
    {
        _IsCompleted = false;
    }

    private void Update()
    {
        if(!_IsCompleted)
            CheckAnswer();
    }

    private void CheckAnswer()
    {
        if (!CheckIfAllQuestionAnswered())
            return;

        if (CheckIfAllAnswersCorrect())
        {
            EventSystemTestRoom.instance.PuzzleSolved();
            _IsCompleted = true;
            return;
        }

        StartCoroutine(RestartPuzzle());
    }

    private bool CheckIfAllAnswersCorrect()
    {
        foreach (TestNoteQuestion question in _TestAnswers)
        {
            if (question.correctAnswer != question.selectedAnswer)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckIfAllQuestionAnswered()
    {
        foreach (TestNoteQuestion question in _TestAnswers)
        {
            if (question.questionAnswered == false)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator RestartPuzzle()
    {
        yield return new WaitForSeconds(_CoolDownRestart);
        EventSystemTestRoom.instance.RestartPuzzle();
    }
}
