using UnityEngine;


public class SmallFan : Holdable {

	private Animator _animator;
	private bool _isOn = true;

	private ParticleSystem _particleSystem;


	protected override void Awake() {
		base.Awake();
		_animator = GetComponent<Animator>();
		_particleSystem = GetComponentInChildren<ParticleSystem>();
	}


	public override bool CanInteract(HoldController holdController) {
		return true;
	}


	public override void Interact() {
		base.Interact();

		_isOn = !_isOn;

		if (_isOn && _particleSystem) {
			_animator.speed = 1f;
			_particleSystem.Play();
		}
		else {
			_animator.speed = 0f;
			_particleSystem.Stop();
		}


	}


}