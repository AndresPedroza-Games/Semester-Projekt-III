using System.Collections;
using UnityEngine;


public class Event002TurnOffLights : MonoBehaviour {

	[Header("---Particles---")]
	[SerializeField] private ParticleSystem _Particles;

	[SerializeField] private float _CoolDown;

	[Header("---Audio---")]
	[SerializeField] private AudioClip lightbulbBurstSfx;

	private Light _light;
	private AudioSource _audioSource;

	private bool _CanPlay;


	private void Awake() {
		_light = GetComponentInChildren<Light>();
		_audioSource = GetComponent<AudioSource>();
	}


	private void Update() {
		if (_CanPlay) {
			_CanPlay = false;
			StartCoroutine(RestartParticles());
		}
	}


	private void OnEnable() {
		EventSystemController.Instance.OnKey001PickedUp += TurnOffLight;
	}


	private void OnDisable() {
		EventSystemController.Instance.OnKey001PickedUp -= TurnOffLight;

	}


	private void TurnOffLight() {
		_light.enabled = false;
		_Particles.Play();

		PlaySound();

		_CanPlay = true;
	}


	private void PlaySound() {
		_audioSource.loop = false;
		_audioSource.PlayOneShot(lightbulbBurstSfx);
	}


	private IEnumerator RestartParticles() {
		yield return new WaitForSeconds(_CoolDown);
		_Particles.Play();
		_CanPlay = true;
	}

}