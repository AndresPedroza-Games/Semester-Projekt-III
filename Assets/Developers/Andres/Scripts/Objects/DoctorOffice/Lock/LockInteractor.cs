using UnityEngine;


public class LockInteractor : MonoBehaviour, IInteractable, ICrosshair
{
    public static LockInteractor instance;

    public static GameObject cameraLock;

    private EventSystemDoctorOffice _EventSystemDoctorOffice;
    private Lock _Lock;

    private bool _CanInteract;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        gameObject.layer = LayerMask.NameToLayer("Interactable");
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
            cameraLock.SetActive(true);
            instance.gameObject.SetActive(false);
        }
    }

    public void ExitInteraction()
    {
        cameraLock.SetActive(false);
        instance.gameObject.SetActive(true);

        if(_Lock._LockPiecesList.Count > 0)
            foreach (LockPiece lockPiece in _Lock._LockPiecesList)
            {
                if(lockPiece)
                    lockPiece.ReleasePieceWithoutEventCall();
            }
    }
}
