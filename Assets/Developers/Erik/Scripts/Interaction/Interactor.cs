using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour {
    private Camera cam;
    [SerializeField] private float interactionDistance = 2.5f;


    private void Awake() {
        cam = Camera.main;
    }

    private void OnEnable() {
        InputManager.Instance.Interact.performed += Interact;
    }

    private void OnDisable() {
        InputManager.Instance.Interact.performed -= Interact;
    }


    private void Interact(InputAction.CallbackContext ctx) {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, interactionDistance)) {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable)) {
                interactable.Interact();
            }
        }
    }

    private void OnDrawGizmos() {
        if (cam == null) {
            cam = Camera.main;
        }

        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactionDistance, Color.blue);
    }
}
