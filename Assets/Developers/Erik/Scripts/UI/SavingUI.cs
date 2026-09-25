using System.Collections;
using TMPro;
using UnityEngine;


public class SavingUI : MonoBehaviour {

	[Header("TEXT ANIMATION")]
	[SerializeField] private GameObject savingIcon;
	[SerializeField] private TMP_Text savingText;

	private readonly string[] _states = { "Saving", "Saving.", "Saving..", "Saving..." };

	private Animator _animator;
	private Coroutine _textRoutine;


	private void Awake() {
		_animator = GetComponent<Animator>();

		savingText.gameObject.SetActive(false);
		savingIcon.SetActive(false);
	}


	private void OnEnable() {
		EventSystemController.Instance.onSaveGame += OnGameSaved;
	}


	private void OnDisable() {
		EventSystemController.Instance.onSaveGame -= OnGameSaved;
	}


	private void DeactivateObjects() {
		savingText.gameObject.SetActive(false);
		savingIcon.SetActive(false);

		if (_textRoutine != null) {
			StopCoroutine(_textRoutine);
			_textRoutine = null;
		}
	}


	private void OnGameSaved() {
		savingText.gameObject.SetActive(true);
		savingIcon.SetActive(true);

		_animator.Play("Saving_Anim", 0, 0f);
		_textRoutine ??= StartCoroutine(TextCoroutine());
	}


	private IEnumerator TextCoroutine() {
		int index = 0;

		while (true) {
			savingText.text = _states[index];
			index = (index + 1) % _states.Length;

			yield return new WaitForSecondsRealtime(0.4f);
		}

	}

}