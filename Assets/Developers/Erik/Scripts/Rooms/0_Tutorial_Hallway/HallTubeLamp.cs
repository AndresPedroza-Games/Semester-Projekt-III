using System.Collections;
using UnityEngine;


public class HallTubeLamp : MonoBehaviour {

	[Header("---Config---")]
	[SerializeField] private bool isOnFromBeginning = true;
	[SerializeField] private bool useTurnOnDelay = true;
	[SerializeField] private bool playBrokenParticlesWhenTurnedOff = true;
	[SerializeField] private bool playParticlesInLoopFromBeginning;

	[Header("---On Turned On---")]
	[SerializeField] private float turnOnDelay = 1f;

	[Header("---Particles---")]
	[SerializeField] private ParticleSystem particles;
	[SerializeField] private Vector2 particleLoopCooldown = new(1f, 5f);

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

		if (playParticlesInLoopFromBeginning)
			_particleRoutine ??= StartCoroutine(PlayParticleLoopCoroutine());
	}


	private void PlayParticleOnce() {
		particles?.Play();
	}


	private void PlayParticleLooped() {
		_particleRoutine ??= StartCoroutine(PlayParticleLoopCoroutine());
	}


	private void PlaySfx(AudioClip clip) {
		_audioSource?.PlayOneShot(clip);
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
		if (!playBrokenParticlesWhenTurnedOff) {
			SetLightActive(false);
			SetAudioSourceActive(false);
			return;
		}

		particles?.Play();
		PlayBulbBurstSfx();

		SetLightActive(false);

		_disableAudioAfterClipRoutine ??= StartCoroutine(DisableAudioAfterClip());

		_particleRoutine ??= StartCoroutine(PlayParticlesLoopWhileLampOffCoroutine());
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


	private IEnumerator PlayParticlesLoopWhileLampOffCoroutine() {
		while (!_light.enabled) {
			yield return new WaitForSeconds(Random.Range(particleLoopCooldown.x, particleLoopCooldown.y));
			particles?.Play();
		}

		_particleRoutine = null;
	}


	private IEnumerator PlayParticleLoopCoroutine() {
		while (gameObject.activeSelf) {
			yield return new WaitForSeconds(Random.Range(particleLoopCooldown.x, particleLoopCooldown.y));
			particles?.Play();
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