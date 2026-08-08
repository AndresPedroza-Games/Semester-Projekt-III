using UnityEngine;


[RequireComponent(typeof(Joint))]
public class Battery : Holdable {

	[Header("---Particles---")]
	[SerializeField] private ParticleSystem _Particles;

	[Header("---SFX---")]
	[SerializeField] private AudioClip onBreakSfx;
	[SerializeField] private float onBreakSfxVolume;
	[SerializeField] private AudioSource audioSource;


	private void OnJointBreak(float breakForce) {
		RemoveBattery();

		if (audioSource) {
			audioSource.Stop();
			audioSource.volume = onBreakSfxVolume;
			audioSource?.PlayOneShot(onBreakSfx);
		}
	}


	public override bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}


	private void RemoveBattery() {
		_Particles.Play();
		EventSystemHall4.instance.ReleaseBatery();
	}

}