using UnityEngine;


public class LockInteractor : MonoBehaviour, IInteractable, ICrosshair {

	public static LockInteractor instance;

	public static GameObject cameraLock;

	private EventSystemDoctorOffice _EventSystemDoctorOffice;
	private Lock _Lock;

	private Collider _collider;

	private bool _CanInteract;


	private void Awake() {
		if (instance && instance != this) {
			Destroy(gameObject);
			return;
		}

		instance = this;

		gameObject.layer = LayerMask.NameToLayer("Interactable");
		_Lock = GetComponentInParent<Lock>();
		_collider = GetComponent<Collider>();
		_CanInteract = true;

	}


	private void Start() {
		_EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
		_EventSystemDoctorOffice.onEndInteractionWithLock += ExitInteraction;
		_EventSystemDoctorOffice.onPuzzleCompleted += OnPuzzleCompleted;
	}


	private void OnDisable() {
		_EventSystemDoctorOffice.onEndInteractionWithLock -= ExitInteraction;
		_EventSystemDoctorOffice.onPuzzleCompleted -= OnPuzzleCompleted;
	}


	public bool CanInteract(HoldController holdController) {
		return true;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return _CanInteract ? CrosshairType.Interactable : CrosshairType.Default;
	}


	public void Interact() {
		if (_CanInteract) {
			_EventSystemDoctorOffice.InteractWithLock();
			cameraLock.SetActive(true);
			_collider.enabled = false;
		}
	}


	private void ExitInteraction() {
		cameraLock.SetActive(false);
		_collider.enabled = true;

		if (_Lock._LockPiecesList.Count > 0)
			foreach (LockPiece lockPiece in _Lock._LockPiecesList) {
				if (lockPiece)
					lockPiece.ReleasePieceWithoutEventCall();
			}
	}


	private void OnPuzzleCompleted() {
		cameraLock.SetActive(false);
		gameObject.SetActive(false);
	}

}