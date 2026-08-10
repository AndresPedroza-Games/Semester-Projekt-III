using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class Credits : MonoBehaviour {

	[Header("---Activate Credits Timer---")]
	[SerializeField] private float activateCreditsDelay = 5f;

	[Header("---Scenes To Unload---")]
	[SerializeField] private List<SceneReference> scenesToUnload;

	private Animator _animator;
	private Coroutine _delayRoutine;


	private void Awake() {
		_animator = GetComponent<Animator>();
	}


	public void MuteSound() {
		AudioListener.pause = true;
	}


	private async void LoadMainMenu() {
		await WorldSceneManager.Instance.LoadScene(BuildSettingsLoader.StartupScene);
		EventSystemController.Instance.MainMenuEntered();

		foreach (SceneReference sceneReference in scenesToUnload) {
			await WorldSceneManager.Instance.UnloadScene(sceneReference);
		}
	}


	private void DisablePlayerInputs() {
		InputManager.Instance.Controls.Movement.Disable();
		InputManager.Instance.Controls.Interaction.Disable();
		InputManager.Instance.Controls.Game.Disable();
	}


	public void PlayCreditsInstant() {
		_animator?.Play("Credits");
	}


	public void PlayCreditsAfterTimer() {
		_delayRoutine ??= StartCoroutine(PlayCreditsAfterDelay());
	}


	private IEnumerator PlayCreditsAfterDelay() {
		yield return new WaitForSeconds(activateCreditsDelay);

		_animator?.Play("Credits");
	}

}