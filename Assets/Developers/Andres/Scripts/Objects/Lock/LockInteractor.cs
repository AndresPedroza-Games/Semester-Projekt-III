using UnityEngine;

public class LockInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _Camera;

    private EventSystemDoctorOffice _EventSystemDoctorOffice;

    private void Start()
    {
        _EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
        _EventSystemDoctorOffice.onEndInteractionWithLock += ExitInteraction;   
    }

    public bool CanInteract(HoldController holdController)
    {
        return true;
    }

    public void Interact()
    {
        _EventSystemDoctorOffice.InteractWithLock();
        _Camera.SetActive(true);
        gameObject.SetActive(false);
        GameManager.miniGameActive = true;

    }

    public void ExitInteraction()
    {
        _Camera.SetActive(false);
        gameObject.SetActive(true);
        GameManager.miniGameActive = false;

    }
}
