using UnityEngine;


[RequireComponent(typeof(Animator))]
public class BallerinaMusicBox : Holdable {

	[Header("---AudiSource---")]
	[SerializeField] private AudioSource audioSource;

	private bool _canInteract;
	private Animator _animator;


	protected override void Awake() {
		base.Awake();

		_animator = GetComponent<Animator>();
		_animator.enabled = false;
	}


	private void Start() {
		EventSystemChildRoom.eventSystemChildRoom.onPuzzlePieceEventTriggerd += PlayMusicAndAnimation;
	}


	protected override void OnDisable() {
		base.OnDisable();

		EventSystemChildRoom.eventSystemChildRoom.onPuzzlePieceEventTriggerd -= PlayMusicAndAnimation;
	}


	public override bool CanInteract(HoldController holdController) {
		return !holdController.HasObject && _canInteract;
	}


	public override void Interact() {
		base.Interact();

		_canInteract = false;
		_animator.enabled = false;
		audioSource.Stop();
	}


	public override CrosshairType GetCrosshairType(HoldController holdController) {
		return _canInteract ? CrosshairType.Interactable : CrosshairType.HandOpen;
	}


	private void PlayMusicAndAnimation() {
		_canInteract = true;
		_animator.enabled = true;
		audioSource.Play();
	}

}