using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(HoldController), typeof(InteractionDetector))]
public class Interactor : MonoBehaviour {

	private HoldController _holdController;
	private InteractionDetector _detector;


	private void Awake() {
		_holdController = GetComponent<HoldController>();
		_detector = GetComponent<InteractionDetector>();
	}


	private void OnEnable() {
		InputManager.Instance.Interact.performed += Interact;
		InputManager.Instance.PickUp.performed += OnPickUpPerformed;
		InputManager.Instance.PickUp.canceled += OnPickUpCanceled;
		InputManager.Instance.InteractElementPuzzle.performed += InteractPuzzleElements;

	}


	private void OnDisable() {
		InputManager.Instance.Interact.performed -= Interact;
		InputManager.Instance.PickUp.performed -= OnPickUpPerformed;
		InputManager.Instance.PickUp.canceled -= OnPickUpCanceled;
		InputManager.Instance.InteractElementPuzzle.performed -= InteractPuzzleElements;
	}


	private void OnPickUpPerformed(InputAction.CallbackContext ctx) {
		if (HoldableIsKey() && _holdController.HasObject) {
			DropHoldable();
			return;
		}

		IInteractable target = _detector.CurrentTarget;

		if (target is IHoldable holdable) {
			holdable.Hold(_holdController);
		}

		if (_holdController.HasObject)
			EventSystemController.Instance.PickItem(_holdController.CurrentHoldable.GameObject());
	}


	private void OnPickUpCanceled(InputAction.CallbackContext ctx) {
		if (HoldableIsKey() && _holdController.HasObject)
			return;

		if (_holdController.HasObject)
			DropHoldable();
	}


	private void DropHoldable() {
		if (_holdController.HasObject)
			EventSystemController.Instance.DropItem(_holdController.HoldGameObject);

		_holdController.ReleaseCurrentHoldable();
	}


	private bool HoldableIsKey() {
		if (_holdController.HasObject)
			return _holdController.HoldGameObject.GetComponent<Holdable>().HoldDefinition is SocketHoldDefinitionSO;
		else
			return false;
	}


	private void Interact(InputAction.CallbackContext ctx) {

		if (GameManager.Instance.miniGameActive)
			return;

		if (_holdController.HasObject)
			if (_holdController.CurrentHoldable.CanInteract(_holdController)) {
				_holdController.CurrentHoldable.Interact();
				return;
			}

		IInteractable target = _detector.CurrentTarget;

		if (target == null) return;

		if (target.CanInteract(_holdController))
			target.Interact();

	}


	private void InteractPuzzleElements(InputAction.CallbackContext ctx) {
		if (!GameManager.Instance.miniGameActive)
			return;

		IInteractable target = _detector.CurrentTarget;

		if (target == null) return;

		if (target.CanInteract(_holdController)) { }
			target.Interact();
	}

}