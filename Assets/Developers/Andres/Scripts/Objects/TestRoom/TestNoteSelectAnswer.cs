using UnityEngine;
using UnityEngine.UI;
using static TestNoteQuestion;

public class TestNoteSelectAnswer : MonoBehaviour, IInteractable
{
    [SerializeField] private NarrativeNote _NarrativeNote;
    [SerializeField] private TestNoteQuestion question;
    [SerializeField] private AnswerType answerType;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public bool CanInteract(HoldController holdController)
    {
        return _NarrativeNote.isInteracting;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        if (CanInteract(holdController))
            return CrosshairType.Interactable;

        return CrosshairType.Default;
    }

    public void Interact()
    {
        question.Select(answerType);
    }

    public void SetSelected(bool value)
    {
        Color c = image.color;
        c.a = value ? 1f : 0f;
        image.color = c;
    }
}
