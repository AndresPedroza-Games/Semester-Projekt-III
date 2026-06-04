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
	}


	private void OnDisable() {
		InputManager.Instance.Interact.performed -= Interact;
		InputManager.Instance.PickUp.performed -= OnPickUpPerformed;
		InputManager.Instance.PickUp.canceled -= OnPickUpCanceled;
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

		if (holdController.HasObject)
			EventSystemController.Instance.PickItem(holdController.CurrentHoldable.GameObject());
	}


	private void OnPickUpCanceled(InputAction.CallbackContext ctx) {
		if (HoldableIsKey() && _holdController.HasObject)
			return;

		if (_holdController.HasObject)
			DropHoldable();
	}


	private void DropHoldable() {
		if (holdController.HasObject)
			EventSystemController.Instance.DropItem(holdController.HoldGameObject);
		
		holdController.ReleaseCurrentHoldable();
	}


	private bool HoldableIsKey() {
		if (_holdController.HasObject)
			return _holdController.CurrentHoldable.GetType() == typeof(Key);
		else
			return false;
	}


	private void Interact(InputAction.CallbackContext ctx) {
		IInteractable target = _detector.CurrentTarget;

		if (target == null) return;

		if (target.CanInteract(_holdController))
			target.Interact();

	}

}