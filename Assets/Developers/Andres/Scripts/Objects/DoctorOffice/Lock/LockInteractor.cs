using UnityEngine;

public class LockInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _Camera;

    private EventSystemDoctorOffice _EventSystemDoctorOffice;
    private Lock _Lock;

    private bool _CanInteract;

    private void Awake()
    {
        _Lock = GetComponentInParent<Lock>();
        _CanInteract = true;
    }

    private void Start()
    {
        _EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
        _EventSystemDoctorOffice.onEndInteractionWithLock += ExitInteraction;
        _EventSystemDoctorOffice.onPuzzleCompleted += ExitInteraction;
        _EventSystemDoctorOffice.onPuzzleCompleted += () => _CanInteract = false;
    }

    public bool CanInteract(HoldController holdController)
    {
        return true;
    }


    public CrosshairType GetCrosshairType(HoldController holdController) {
	    return _CanInteract ? CrosshairType.Interactable : CrosshairType.Default;
    }


    public void Interact()
    {
        if (_CanInteract)
        {
            _EventSystemDoctorOffice.InteractWithLock();
            _Camera.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void ExitInteraction()
    {
        _Camera.SetActive(false);
        gameObject.SetActive(true);

        foreach (LockPiece lockPiece in _Lock._LockPiecesList)
        {
            lockPiece.ReleasePiece();
        }
    }
}
