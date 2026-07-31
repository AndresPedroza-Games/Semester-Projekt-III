using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(AudioSource))]
public class TwinEventFunctions : MonoBehaviour {

	[Header("---SFX---")]
	[SerializeField] private Vector2 pitchRange = new(0.7f, 1.3f);
	[SerializeField] private AudioClip footStepSfx;


	[Header("---Can see threshold---")]
	[SerializeField] [Range(0f, 1f)] private float canSeeThreshold = 0.8f;
	[SerializeField] private float reactionDistance = 2f;
	[SerializeField] private float rayLength = 2f;

	private AudioSource _audioSource;
	private Animator _animator;

	private Coroutine _checkPlayerLookRoutine;
	private bool _playerIsLookingAtUs;
	private Camera _cam;


	private void Awake() {
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
	}


	public void DisableGameObject() {
		StartCoroutine(DisableObjectAfterClip());
	}


	public void PlayFootStepSfx() {
		_audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
		_audioSource.PlayOneShot(footStepSfx);
	}


	private IEnumerator DisableObjectAfterClip() {
		while (_audioSource.isPlaying)
			yield return null;
		gameObject.SetActive(false);
	}


	public void RaiseGameEvent(GameEvent gameEvent) {
		gameEvent.Raise();
	}


	public void CheckForPlayerLookDirection() {
		_cam = Camera.main;

		_checkPlayerLookRoutine ??= StartCoroutine(CheckPlayerLookDirectionCoroutine());
	}


	private IEnumerator CheckPlayerLookDirectionCoroutine() {

		while (!_playerIsLookingAtUs) {
			Vector3 dir = (transform.position - _cam.transform.position).normalized;

			float distance = Vector3.Distance(transform.position, _cam.transform.position);

			bool noObstacleBetween = false || !Physics.Raycast(_cam.transform.position, _cam.transform.forward, rayLength);

			float dot = Vector3.Dot(_cam.transform.forward, dir);

			if (dot >= canSeeThreshold && noObstacleBetween || distance <= reactionDistance) {
				_playerIsLookingAtUs = true;
				_animator.Play("Hall3_TwinAnim_02");
			}

			yield return null;
		}

		_checkPlayerLookRoutine = null;
	}


	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, reactionDistance);
	}

}