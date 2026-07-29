using System.Collections;
using UnityEngine;


public class HallTubeLamp : MonoBehaviour {

	[Header("---Config---")]
	[SerializeField] private bool isOnFromBeginning = true;
	[SerializeField] private bool useTurnOnDelay = true;
	[SerializeField] private bool playBrokenParticles = true;

	[Header("---On Turned On---")]
	[SerializeField] private float turnOnDelay = 1f;

	[Header("---Particles---")]
	[SerializeField] private ParticleSystem particles;
	[SerializeField] private Vector2 particleLoopCooldown = new(1f, 3f);

	[Header("---Audio---")]
	[SerializeField] private AudioClip lightbulbBurstSfx;

	private Light _light;
	private AudioSource _audioSource;
	private Coroutine _particleRoutine;

	private Coroutine _turnLightOnRoutine;
	private Coroutine _disableAudioAfterClipRoutine;


	private void Awake() {
		_light = GetComponentInChildren<Light>();
		_audioSource = GetComponent<AudioSource>();

		_light.enabled = isOnFromBeginning;
	}


	public void TurnLightOn() {
		if (!useTurnOnDelay) {
			SetLightActive(true);
			SetAudioSourceActive(true);
			return;
		}

		_turnLightOnRoutine ??= StartCoroutine(TurnLightOnCoroutine(turnOnDelay));
	}


	public void TurnLightOff() {
		if (!playBrokenParticles) {
			SetLightActive(false);
			SetAudioSourceActive(false);
			return;
		}

		particles.Play();
		PlayBulbBurstSfx();

		SetLightActive(false);

		_disableAudioAfterClipRoutine ??= StartCoroutine(DisableAudioAfterClip());

		_particleRoutine ??= StartCoroutine(PlayParticlesLoopCoroutine());
	}


	private void PlayBulbBurstSfx() {
		_audioSource.PlayOneShot(lightbulbBurstSfx);
	}


	private void SetLightActive(bool toggle) {
		_light.enabled = toggle;
	}


	private void SetAudioSourceActive(bool toggle) {
		_audioSource.enabled = toggle;
	}


	private IEnumerator PlayParticlesLoopCoroutine() {
		while (!_light.enabled) {
			yield return new WaitForSeconds(Random.Range(particleLoopCooldown.x, particleLoopCooldown.y));
			particles.Play();
		}

		_particleRoutine = null;
	}


	private IEnumerator TurnLightOnCoroutine(float delay) {
		yield return new WaitForSeconds(delay);

		SetLightActive(true);
		SetAudioSourceActive(true);

		_turnLightOnRoutine = null;
	}


	private IEnumerator DisableAudioAfterClip() {
		while (_audioSource.isPlaying)
			yield return null;

		SetAudioSourceActive(false);
	}


}