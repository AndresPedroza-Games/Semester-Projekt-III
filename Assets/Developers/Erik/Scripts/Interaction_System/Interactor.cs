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
		InputManager.Instance.PickUp.performed += HandlePickUp;
	}


	private void OnDisable() {
		InputManager.Instance.Interact.performed -= Interact;
		InputManager.Instance.PickUp.performed -= HandlePickUp;
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


	private void HandlePickUp(InputAction.CallbackContext ctx) {
		if (holdController.HasObject) {
			holdController.ReleaseCurrentHoldable();
			return;
		}

		IInteractable target = detector.CurrentTarget;

		if (target is IHoldable holdable) {
			holdable.Hold(holdController);
		}

	}


	private void Interact(InputAction.CallbackContext ctx) {
		IInteractable target = detector.CurrentTarget;

		if (target == null) return;

		if (target.CanInteract(holdController))
			target.Interact();

	}

}