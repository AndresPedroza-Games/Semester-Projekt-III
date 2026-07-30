using System.Collections;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class TwinEventFunctions : MonoBehaviour {

	[Header("---SFX---")]
	[SerializeField] private Vector2 pitchRange = new(0.7f, 1.3f);
	[SerializeField] private AudioClip footStepSfx;

	private AudioSource _audioSource;


	private void Awake() {
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


}