using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Interactor : MonoBehaviour {

	[Header("---Interaction---")]
	[SerializeField] private float interactionDistance = 2.5f;

	[Header("---Crosshair---")] [SerializeField]
	private Image crosshairImage;

	[SerializeField] private Sprite crosshairDot;
	[SerializeField] private Sprite crosshairCircle;

	private GameObject currentObject;
	private GameObject newObject;

	private Camera cam;
	private LayerMask interactableLayer;
	private bool isLookingAtInteractable;

	public Inventory Inventory { get; private set; }
	public Grabber Grabber { get; private set; }


	private void Awake() {
		cam = Camera.main;
		Inventory = GetComponent<Inventory>();
		Grabber = GetComponent<Grabber>();
		interactableLayer = LayerMask.GetMask("Interactable");
	}


	private void OnEnable() {
		InputManager.Instance.Interact.performed += Interact;
		InputManager.Instance.Drop.performed += Drop;
	}


	private void OnDisable() {
		InputManager.Instance.Interact.performed -= Interact;
		InputManager.Instance.Drop.performed -= Drop;
	}


	private void Update() {
		isLookingAtInteractable = LookingAtInteractable();

		Sprite targetSprite = isLookingAtInteractable ? crosshairCircle : crosshairDot;

		if (crosshairImage.sprite != targetSprite)
			crosshairImage.sprite = targetSprite;

		HighlightGameObject();
	}


	private bool LookingAtInteractable() {
		return Physics.Raycast(cam.transform.position, cam.transform.forward, interactionDistance, interactableLayer);
	}


	private void HighlightGameObject() {
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactionDistance, interactableLayer)) {
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


	private void Interact(InputAction.CallbackContext ctx) {

		if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactionDistance))
			return;

		if (hit.collider.TryGetComponent(out IInteractable interactable))
			interactable.Interact(this);
	}


	private void Drop(InputAction.CallbackContext ctx) {
		if(Grabber.IsHolding)
			Grabber.Drop();
		
		if (Inventory.HoldingObject == null) return;

		if (Inventory.HoldingObject.TryGetComponent(out IPickable pickable))
			pickable.Drop(Inventory);
		Inventory.HoldingObject = null;
	}


	private void OnDrawGizmos() {
		if (cam == null) {
			cam = Camera.main;
		}

		Debug.DrawRay(cam.transform.position, cam.transform.forward * interactionDistance, Color.blue);
	}

}