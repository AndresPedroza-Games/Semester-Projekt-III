using UnityEngine;


[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Holdable : MonoBehaviour, IInteractable, IHoldable, IHighlightable {

	[Header("---Hold Definition---")]
	[SerializeField] private HoldDefinition holdDefinition;

	[Header("---Highlight Config---")]
	[SerializeField] private float borderThickness = 0.02f;


	private Renderer _renderer;
	private readonly int _borderThickness = Shader.PropertyToID("_BorderThickness");

	private HoldController _currentHolder;

	public Rigidbody Rigidbody { get; private set; }
	public Collider Collider { get; private set; }


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");

		_renderer = GetComponent<Renderer>();

		Rigidbody = GetComponent<Rigidbody>();
		Collider = GetComponent<Collider>();

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

		Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject;
	}


	public void Interact() {
	}


	public void Hold(HoldController holder) {
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