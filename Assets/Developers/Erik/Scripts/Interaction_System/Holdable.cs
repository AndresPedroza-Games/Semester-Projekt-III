using UnityEngine;


[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Holdable : MonoBehaviour, IInteractable, IHoldable, IHighlightable {

	[Header("---Hold Definition---")]
	[SerializeField] private HoldDefinition holdDefinition;

	public Transform StartParentIfSocketHold { get; private set; }

	[Header("---Highlight Config---")]
	[SerializeField] private float borderThickness = 0.02f;


	private Renderer _renderer;
	private readonly int _borderThickness = Shader.PropertyToID("_BorderThickness");

	private HoldController _currentHolder;
	public bool CanBeHold { get; set; } = true;

	public Rigidbody Rigidbody { get; private set; }
	public Collider Collider { get; private set; }


	protected virtual void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");

		_renderer = GetComponent<Renderer>();

		Rigidbody = GetComponent<Rigidbody>();
		Collider = GetComponent<Collider>();

		if (holdDefinition is SocketHoldDefinitionSO)
			StartParentIfSocketHold = transform.parent;

		if (holdDefinition is PullDefinitionSO)
			Rigidbody.isKinematic = true;

		ConfigurePhysics();
	}


	public GameObject GameObject() {
		return this.gameObject;
	}


	private void ConfigurePhysics() {
		if (!Rigidbody)
			return;

		if (Rigidbody.collisionDetectionMode == CollisionDetectionMode.Discrete)
			Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

		Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	}


	public virtual bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return CanBeHold ? CrosshairType.HandOpen : CrosshairType.Default;

	}


	public virtual void Interact() {
	}


	public void Hold(HoldController holder) {
		if (!CanBeHold)
			return;

		_currentHolder = holder;

		holdDefinition.Hold(this, holder);

		holder.SetCurrentHoldable(this);
	}


	public void Release() {
		holdDefinition.Release(this, _currentHolder);

		_currentHolder?.ClearCurrentHoldable();

		_currentHolder = null;
	}


	public void Highlight() {
		if (!_renderer)
			return;

		_renderer.material.SetFloat(_borderThickness, borderThickness);
	}


	public void RemoveHighlight() {
		if (!_renderer)
			return;

		_renderer.material.SetFloat(_borderThickness, 0f);
	}

}