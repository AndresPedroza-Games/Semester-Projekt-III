using UnityEngine;


[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Holdable : MonoBehaviour, IInteractable, IHoldable, IHighlightable, IFlushable, ICrosshair {

	[Header("---Hold Definition---")]
	[SerializeField] private HoldDefinition holdDefinition;

	public HoldDefinition HoldDefinition => holdDefinition;

	public Transform StartParentIfSocketHold { get; private set; }

	private const float OutlineThickness = 0.02f;


	protected Renderer ren;
	private readonly int _borderThickness = Shader.PropertyToID("_BorderThickness");

	private HoldController _currentHolder;
	public bool canBeHold = true;

	public Rigidbody Rigidbody { get; private set; }
	public Collider Collider { get; private set; }

	protected ObjectSfx sfx;


	protected virtual void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");

		sfx = GetComponent<ObjectSfx>();

		ren = GetComponent<Renderer>();

		Rigidbody = GetComponent<Rigidbody>();
		Collider = GetComponent<Collider>();

		if (holdDefinition is SocketHoldDefinitionSO)
			StartParentIfSocketHold = transform.parent;

		if (holdDefinition is PullDefinitionRestrictedSO)
			Rigidbody.isKinematic = true;

		if (holdDefinition is PullDefinitionUnrestrictedSO) {
			Rigidbody.mass = 3f;
			Rigidbody.linearDamping = 2f;
			Rigidbody.angularDamping = 2f;
		}

		ConfigurePhysics();
	}


	protected virtual void OnDisable() {
		if (_currentHolder)
			Destroy(gameObject);
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


	public virtual CrosshairType GetCrosshairType(HoldController holdController) {
		return canBeHold ? CrosshairType.HandOpen : CrosshairType.Default;
	}


	public virtual void Interact() {
		sfx?.PlaySfx(SfxEvent.OnInteract);
	}


	private void OnCollisionEnter(Collision collision) {
		if (_currentHolder)
			return;

		if (!sfx)
			return;

		if (sfx.AudioSource.isPlaying)
			return;

		if (collision.relativeVelocity.magnitude < 0.5f)
			return;

		sfx?.PlaySfx(SfxEvent.OnCollision);
	}


	public virtual void Hold(HoldController holder, Vector3 hitPoint) {
		if (!canBeHold)
			return;

		sfx?.PlaySfx(SfxEvent.OnPickup);

		_currentHolder = holder;

		holdDefinition.Hold(this, holder, hitPoint);

		holder.SetCurrentHoldable(this);

	}


	public virtual void Release() {
		holdDefinition.Release(this, _currentHolder);

		EventSystemController.Instance.DropItem(gameObject);

		_currentHolder?.ClearCurrentHoldable();

		_currentHolder = null;
	}


	public void Highlight() {
		if (!ren)
			return;

		ren.material.SetFloat(_borderThickness, OutlineThickness);
	}


	public void RemoveHighlight() {
		if (!ren)
			return;

		ren.material.SetFloat(_borderThickness, 0f);
	}


	public void Flush() {
		gameObject.SetActive(false);
	}

}