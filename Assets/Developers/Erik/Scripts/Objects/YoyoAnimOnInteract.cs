using DG.Tweening;
using UnityEngine;


public class YoyoAnimOnInteract : MonoBehaviour, IInteractable, ICrosshair {

	[Header("---Animation---")]
	[SerializeField] private Vector3 endPosition;
	[SerializeField] private float duration;

	[Header("---ParticleSystem---")]
	[SerializeField] private ParticleSystem particle;

	private bool _canInteract = true;
	private bool _canEmitParticle = true;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");
	}


	public void Interact() {
		if (!_canInteract)
			return;

		Animate();
		_canInteract = false;
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return _canInteract ? CrosshairType.Interactable : CrosshairType.Default;
	}


	private void Animate() {
		transform.DOLocalMove(endPosition, duration).SetLoops(2, LoopType.Yoyo).OnStepComplete(() => {
			if (_canEmitParticle) {
				particle.Play();
				_canEmitParticle = false;
			}
		}).OnComplete(() => {
			_canInteract = true;
			_canEmitParticle = true;
		});
	}

}