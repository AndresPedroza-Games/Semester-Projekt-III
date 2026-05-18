using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Interactor : MonoBehaviour {

	[Header("---Interaction---")]
	[SerializeField] private float interactionDistance = 2.5f;
	public IPickable CurrentPickedObj { get; set; }
	public Grabber Grabber { get; private set; }
	public Picker Picker { get; private set; }

	[Header("---Crosshair---")] [SerializeField]
	private Image crosshairImage;

	[SerializeField] private Sprite crosshairDot;
	[SerializeField] private Sprite crosshairCircle;

	private GameObject currentObject;
	private GameObject newObject;

	public Camera cam { get; private set; }
	private LayerMask interactableLayer;
	private bool isLookingAtInteractable;

	private EventSystemController eventSystemController;


	private void Awake() {
		cam = Camera.main;
		Grabber = GetComponent<Grabber>();
		Picker = GetComponent<Picker>();
		interactableLayer = LayerMask.GetMask("Interactable");
	}

    private void Start()
    {
		eventSystemController = EventSystemController.eventSystemController;
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

		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactionDistance)) {
			if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
				return interactable.CanInteract(this);
		}

		return false;
	}


	private void HighlightGameObject() {
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactionDistance, interactableLayer)) {
			newObject = hit.collider.gameObject;

			if (newObject.GetComponent<IInteractable>() != null) {
				if (currentObject != null)
					currentObject.GetComponent<Renderer>().material.SetFloat("_BorderThickness", 0.05f);

				currentObject = newObject;
				currentObject.GetComponent<Renderer>().material.SetFloat("_BorderThickness", 0.05f);
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
			if (interactable.CanInteract(this))
				interactable.Interact(this);
	}


	private void Drop(InputAction.CallbackContext ctx) {
		if (CurrentPickedObj == null) return;

		CurrentPickedObj.Drop();
		CurrentPickedObj = null;
	}


	private void OnDrawGizmos() {
		if (cam == null) {
			cam = Camera.main;
		}

		Debug.DrawRay(cam.transform.position, cam.transform.forward * interactionDistance, Color.blue);
	}

}