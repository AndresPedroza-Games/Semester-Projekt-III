using UnityEngine;


[RequireComponent(typeof(InteractionDetector))]
public class InteractionHighlighter : MonoBehaviour {

	private InteractionDetector _detector;

	private IHighlightable _currentHighlightable;


	private void Awake() {
		_detector = GetComponent<InteractionDetector>();
	}


	private void Update() {
		HandleHighlight();
	}


	private void HandleHighlight() {

		IInteractable target = _detector.CurrentTarget;

		if (target == null) {
			ClearHighlight();
			return;
		}

		MonoBehaviour targetBehaviour = target as MonoBehaviour;

		if (!targetBehaviour) {
			ClearHighlight();
			return;
		}

		if (!targetBehaviour.TryGetComponent(out IHighlightable highlightable)) {
			ClearHighlight();
			return;
		}

		if (_currentHighlightable == highlightable)
			return;

		ClearHighlight();

		_currentHighlightable = highlightable;
		_currentHighlightable.Highlight();
	}


	private void ClearHighlight() {

		if (_currentHighlightable == null)
			return;

		_currentHighlightable.RemoveHighlight();
		_currentHighlightable = null;
	}


	private void OnDisable() {
		ClearHighlight();
	}

}