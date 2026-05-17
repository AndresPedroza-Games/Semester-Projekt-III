using UnityEngine;


[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Holdable : MonoBehaviour, IInteractable, IHoldable {

	[Header("---Hold Definition")]
	[SerializeField] private HoldDefinition holdDefinition;

	private HoldController currentHolder;

	public Rigidbody Rigidbody { get; private set; }
	public Collider Collider { get; private set; }


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");

		Rigidbody = GetComponent<Rigidbody>();
		Collider = GetComponent<Collider>();

		ConfigurePhysics();
	}


	private void ConfigurePhysics() {
		if (!Rigidbody)
			return;

		Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}


	public void Interact() {
	}


	public void Hold(HoldController holder) {
		currentHolder = holder;

		holdDefinition.Hold(this, holder);

		holder.SetCurrentHoldable(this);
	}


	public void Release() {
		holdDefinition.Release(this, currentHolder);

		currentHolder?.ClearCurrentHoldable();

		currentHolder = null;
	}

}