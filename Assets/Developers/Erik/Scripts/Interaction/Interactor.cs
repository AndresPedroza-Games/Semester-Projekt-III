using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Interactor : MonoBehaviour {

	[Header("---Interaction---")]
	[SerializeField] private float interactionDistance = 2.5f;
	public IPickable CurrentHeldObject { get; set; }
	public Grabber Grabber { get; private set; }
	public Picker Picker { get; private set; }

	[Header("---Crosshair---")] [SerializeField]
	private Image crosshairImage;

	[SerializeField] private Sprite crosshairDot;
	[SerializeField] private Sprite crosshairCircle;

	private GameObject currentObject;
	private GameObject newObject;

	public Camera Cam { get; private set; }
	private LayerMask interactableLayer;
	private bool isLookingAtInteractable;


	private void Awake() {
		Cam = Camera.main;
		Grabber = GetComponent<Grabber>();
		Picker = GetComponent<Picker>();
		interactableLayer = LayerMask.GetMask("Interactable");
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
		isLookingAtInteractable = LookingAtInteractable();

		Sprite targetSprite = isLookingAtInteractable ? crosshairCircle : crosshairDot;

		if (crosshairImage.sprite != targetSprite)
			crosshairImage.sprite = targetSprite;

		HighlightGameObject();
	}


	private bool LookingAtInteractable() {

		if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, interactionDistance)) {
			if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
				return interactable.CanInteract(this);
		}

		return false;
	}


	private void HighlightGameObject() {
		if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, interactionDistance, interactableLayer)) {
			newObject = hit.collider.gameObject;

			if (newObject.GetComponent<IInteractable>() != null) {
				if (currentObject != null)
					currentObject.GetComponent<Renderer>().material.SetFloat("_BorderThickness", 0.02f);

				currentObject = newObject;
				currentObject.GetComponent<Renderer>().material.SetFloat("_BorderThickness", 0.02f);
			}
		}
		else {
			if (currentObject != null) {
				currentObject.GetComponent<Renderer>().material.SetFloat("_BorderThickness", 0f);
				currentObject = null;
			}
		}
	}


	private void HandlePickUp(InputAction.CallbackContext ctx) {
		if (CurrentHeldObject != null) {
			Drop();
			return;
		}

		if (!Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, interactionDistance))
			return;

		if (hit.collider.TryGetComponent(out IPickable pickable)) {
			pickable.PickUp(this);
		}
	}


	private void Interact(InputAction.CallbackContext ctx) {
		if (!Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, interactionDistance))
			return;

		if (hit.collider.TryGetComponent(out IInteractable interactable))
			if (interactable.CanInteract(this))
				interactable.Interact(this);
	}


	private void Drop() {
		CurrentHeldObject.Drop();
		CurrentHeldObject = null;
	}


	private void OnDrawGizmos() {
		if (Cam == null) {
			Cam = Camera.main;
		}

		Debug.DrawRay(Cam.transform.position, Cam.transform.forward * interactionDistance, Color.blue);
	}

}