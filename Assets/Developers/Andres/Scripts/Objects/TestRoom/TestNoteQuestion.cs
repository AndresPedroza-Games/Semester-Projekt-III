using UnityEngine;

public class TestNoteQuestion : MonoBehaviour
{
    public TestNoteSelectAnswer yes;
    public TestNoteSelectAnswer no;

    public AnswerType selectedAnswer;
    [field: SerializeField] public AnswerType correctAnswer { get; private set; }

    public bool questionAnswered;
    private AnswerType? _LastAnswer = null;

    private void Start()
    {
        EventSystemTestRoom.instance.onRestartPuzzle += RestartPuzzle;
    }

    public void Select(AnswerType answer)
    {
        selectedAnswer = answer;
        questionAnswered = true;

        yes.SetSelected(answer == AnswerType.Yes);
        no.SetSelected(answer == AnswerType.No);

        bool answerChanged = _LastAnswer == null || _LastAnswer != answer;

        if (answer == correctAnswer && answerChanged)
        {
            EventSystemTestRoom.instance.CrossAnswer(selectedAnswer == correctAnswer);
            Debug.Log("Correct");
        }

        _LastAnswer = answer;
    }

    private void RestartPuzzle()
    {
        selectedAnswer = AnswerType.None;
        Select(selectedAnswer);
        questionAnswered = false;
    }

    public enum AnswerType
    {
        None,
        Yes,
        No
    }
}
