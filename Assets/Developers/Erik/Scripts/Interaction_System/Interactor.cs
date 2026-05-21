using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(HoldController), typeof(InteractionDetector), typeof(InteractionUI))]
[RequireComponent(typeof(Grabber), typeof(Picker))]
public class Interactor : MonoBehaviour {

	private HoldController holdController;
	private InteractionDetector detector;


	private void Awake() {
		holdController = GetComponent<HoldController>();
		detector = GetComponent<InteractionDetector>();
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


	private void Update() {
		//HighlightGameObject();
	}


	// private void HighlightGameObject() {
	// 	if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, interactionDistance, interactableLayer)) {
	// 		newObject = hit.collider.gameObject;
	//
	// 		if (newObject.GetComponent<IInteractable>() != null) {
	// 			if (currentObject != null)
	// 				currentObject.GetComponent<Renderer>().material.SetFloat("_BorderThickness", 0.02f);
	//
	// 			currentObject = newObject;
	// 			currentObject.GetComponent<Renderer>().material.SetFloat("_BorderThickness", 0.02f);
	// 		}
	// 	}
	// 	else {
	// 		if (currentObject != null) {
	// 			currentObject.GetComponent<Renderer>().material.SetFloat("_BorderThickness", 0f);
	// 			currentObject = null;
	// 		}
	// 	}
	// }


	private void OnPickUpPerformed(InputAction.CallbackContext ctx) {
		if (HoldableIsKey() && holdController.HasObject) {
			DropHoldable();
			return;
		}

		IInteractable target = detector.CurrentTarget;

		if (target is IHoldable holdable) {
			holdable.Hold(holdController);
		}
	}


	private void OnPickUpCanceled(InputAction.CallbackContext ctx) {
		if (HoldableIsKey() && holdController.HasObject)
			return;

		if (holdController.HasObject)
			DropHoldable();
	}


	private void DropHoldable() {
		holdController.ReleaseCurrentHoldable();
	}


	private bool HoldableIsKey() {
		if (holdController.HasObject)
			return holdController.CurrentHoldable.GetType() == typeof(Key);
		else
			return false;
	}


	private void Interact(InputAction.CallbackContext ctx) {
		IInteractable target = detector.CurrentTarget;

		if (target == null) return;

		if (target.CanInteract(holdController))
			target.Interact();

	}

}