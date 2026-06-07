using UnityEngine;


public class PhysicsHolder : MonoBehaviour {

	[Header("---Hold Point---")]
	[SerializeField] private Transform holdPoint;
	[Tooltip("Offset is used when the Hold Point is located within a different object")]
	[SerializeField] [Range(0.1f, 0.5f)] private float offset = 0.1f;

	[Header("---Layer Mask---")]
	[Tooltip("The layers to which the offset is applied")]
	public LayerMask grabLayerMask;

	[Header("---Follow Speed---")]
	[SerializeField] private float smoothSpeed = 15f;

	[Header(("---Joint Break Config---"))]
	[Tooltip("Break Distance shouldn't be less then Interaction Distance")]
	[SerializeField] private float breakDistance = 2f;
	[SerializeField] private float jointLimit = 0.5f;

	[Header("---Joint Driver Config---")]
	[SerializeField] private float positionSpring = 800f;
	[SerializeField] private float positionDamper = 40f;
	[SerializeField] private float maxForce = 1000f;


	private Holdable _holdable;
	private IHoldTargetResolver _resolver;
	private HoldPhysicsProfile _currentProfile;

	private GameObject _holdBody;
	private Rigidbody _holdRb;
	private ConfigurableJoint _joint;


	private Quaternion _rotationOffset;
	private Camera _cam;

	private HoldContext _context;
	private RigidbodyConstraints _originalConstraints;
	private bool _isKinematic;


	private void Awake() {
		_cam = Camera.main;
	}


	private void Start() {
		CreateHoldBody();
	}


	private void FixedUpdate() {
		CheckJointState();
		MoveHoldPosition();
		UpdateRotation();
	}


	public void Hold(Holdable newHoldable, IHoldTargetResolver targetResolver, HoldPhysicsProfile profile) {
		if (_holdable) return;

		_holdable = newHoldable;
		_resolver = targetResolver;
		_currentProfile = profile;

		Vector3 pullGrabOffset = _holdable.Rigidbody.position - holdPoint.position;
		pullGrabOffset.y = 0f;

		_context = new HoldContext {
			HoldPoint = holdPoint,
			Holdable = newHoldable,
			Camera = _cam,
			Offset = offset,
			IgnoreLayer = grabLayerMask,
			PullGrabOffset = pullGrabOffset
		};

		Rigidbody rb = _holdable.Rigidbody;

		_originalConstraints = rb.constraints;
		_isKinematic = rb.isKinematic;
		rb.isKinematic = false;

		rb.useGravity = profile.useGravity;
		rb.constraints = profile.constraints | _originalConstraints;

		ApplyMotionSettings(profile);

		_joint.connectedBody = rb;

		float holdY = holdPoint.eulerAngles.y;
		float objectY = rb.rotation.eulerAngles.y;

		_rotationOffset = Quaternion.Euler(0f, objectY - holdY, 0f);
	}


	public void Release() {
		if (!_holdable)
			return;

		if (!_currentProfile.keepMomentum)
			_holdable.Rigidbody.linearVelocity = Vector3.zero;

		_holdable.Rigidbody.useGravity = true;
		_holdable.Rigidbody.constraints = _originalConstraints;
		_holdable.Rigidbody.isKinematic = _isKinematic;

		_joint.connectedBody = null;

		_holdable = null;
		_resolver = null;
	}


	private void CheckJointState() {
		if (!_holdable)
			return;

		float distance = Vector3.Distance(holdPoint.position, _holdable.Rigidbody.position);

		if (distance > breakDistance) {
			_holdable.Release();
		}
	}


	private void UpdateRotation() {
		if (!_holdable)
			return;

		if (!_currentProfile.followRotation)
			return;

		Quaternion targetRot = Quaternion.Euler(0f, holdPoint.eulerAngles.y, 0f) * _rotationOffset;

		_holdable.Rigidbody.MoveRotation(Quaternion.Slerp(_holdable.Rigidbody.rotation, targetRot, smoothSpeed * Time.fixedDeltaTime));
	}


	private void MoveHoldPosition() {
		if (!_holdable) {
			_holdRb.MovePosition(holdPoint.position);
			return;
		}

		Vector3 targetPos = _resolver.GetTargetPosition(_context);

		Vector3 smoothPos = Vector3.Lerp(_holdRb.position, targetPos, smoothSpeed * Time.fixedDeltaTime);

		_holdRb.MovePosition(smoothPos);
	}


	private void ApplyMotionSettings(HoldPhysicsProfile profile) {
		_joint.xMotion = profile.xMotion;
		_joint.yMotion = profile.yMotion;
		_joint.zMotion = profile.zMotion;

		_joint.angularXMotion = profile.angularXMotion;
		_joint.angularYMotion = profile.angularYMotion;
		_joint.angularZMotion = profile.angularZMotion;
	}


	private void CreateHoldBody() {

		_holdBody = new GameObject("Physics Hold Body");

		_holdRb = _holdBody.AddComponent<Rigidbody>();
		_holdRb.isKinematic = true;
		_holdRb.useGravity = false;

		_joint = _holdBody.AddComponent<ConfigurableJoint>();

		_joint.autoConfigureConnectedAnchor = false;

		_joint.anchor = Vector3.zero;
		_joint.connectedAnchor = Vector3.zero;

		_joint.breakForce = Mathf.Infinity;
		_joint.breakTorque = Mathf.Infinity;

		JointDrive drive = new JointDrive { positionSpring = positionSpring, positionDamper = positionDamper, maximumForce = maxForce };

		_joint.xDrive = drive;
		_joint.yDrive = drive;
		_joint.zDrive = drive;

		SoftJointLimit limit = new SoftJointLimit { limit = jointLimit };
		_joint.linearLimit = limit;

		_joint.configuredInWorldSpace = true;
		_joint.projectionMode = JointProjectionMode.PositionAndRotation;
	}


	private void OnDrawGizmos() {
		if (_holdRb) {
			Gizmos.color = Color.purple;
			Gizmos.DrawWireSphere(_holdRb.position, 0.1f);
		}
	}

}