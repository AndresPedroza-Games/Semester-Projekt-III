using UnityEngine;


public class Grabber : MonoBehaviour {

	[Header("---Velocity---")]
	[SerializeField] private bool keepMomentum = true;

	[Header("---Hold Point---")]
	[SerializeField] private Transform holdPoint;
	[SerializeField] private float holdSmoothFollowSpeed = 15f;
	[Tooltip("Offset is used when the Hold Point is located within a different object")]
	[SerializeField] [Range(0.1f, 0.5f)] private float offset = 0.1f;

	[Header(("---Joint Break Config---"))]
	[Tooltip("Break Distance shouldn't be less then Interaction Distance")]
	[SerializeField] private float breakDistance = 2f;
	[SerializeField] private float jointLimit = 0.5f;

	[Header("---Joint Driver Config---")]
	[SerializeField] private float positionSpring = 800f;
	[SerializeField] private float positionDamper = 40f;
	[SerializeField] private float maxForce = 1000f;


	private Holdable holdable;
	private RigidbodyConstraints originalConstraints;

	private GameObject holdBody;
	private Rigidbody holdRb;
	private ConfigurableJoint currentJoint;
	private LayerMask ignoreLayer;

	private Quaternion rotationOffset;
	private Camera cam;


	private void Awake() {
		cam = Camera.main;

		ignoreLayer = ~LayerMask.GetMask("Interactable");
	}


	private void Start() {
		holdBody = new GameObject("HoldBody");

		holdRb = holdBody.AddComponent<Rigidbody>();
		holdRb.isKinematic = true;
		holdRb.useGravity = false;

		holdRb.interpolation = RigidbodyInterpolation.Interpolate;
		holdRb.collisionDetectionMode = CollisionDetectionMode.Continuous;

		holdBody.transform.position = holdPoint.position;

		currentJoint = holdBody.AddComponent<ConfigurableJoint>();

		ConfigureJoint();
		currentJoint.connectedBody = null;
	}


	private void FixedUpdate() {
		CheckJointState();
		MoveHoldPoint();
		RotateObject();
	}


	private void MoveHoldPoint() {
		Ray ray = new(cam.transform.position, cam.transform.forward);
		float distance = (ray.origin - holdPoint.position).magnitude;

		Vector3 targetPos;

		if (Physics.Raycast(ray, out RaycastHit hit, distance, ignoreLayer)) {
			Vector3 offsetDir = (ray.origin - hit.point).normalized;
			targetPos = hit.point + offsetDir * offset;
		}
		else {
			targetPos = holdPoint.position;
		}

		Vector3 smoothPos = Vector3.Slerp(holdRb.position, targetPos, holdSmoothFollowSpeed * Time.fixedDeltaTime);

		holdRb.MovePosition(smoothPos);
	}


	private void RotateObject() {
		if (!holdable)
			return;

		float holdY = holdPoint.eulerAngles.y;

		Quaternion targetRotation = Quaternion.Euler(0f, holdY, 0f) * rotationOffset;

		holdable.Rigidbody.MoveRotation(Quaternion.Slerp(holdable.Rigidbody.rotation, targetRotation, holdSmoothFollowSpeed * Time.fixedDeltaTime));
	}


	public void Grab(Holdable newHoldable) {
		if (holdable) return;

		holdable = newHoldable;
		newHoldable.Rigidbody.useGravity = false;
		
		originalConstraints = newHoldable.Rigidbody.constraints;
		newHoldable.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

		currentJoint.connectedBody = holdable.Rigidbody;

		float holdY = holdPoint.eulerAngles.y;
		float objectY = newHoldable.Rigidbody.rotation.eulerAngles.y;
		float yOffset = objectY - holdY;

		rotationOffset = Quaternion.Euler(0f, yOffset, 0f);
	}


	public void Drop() {
		if (!holdable) return;

		if (!keepMomentum)
			holdable.Rigidbody.linearVelocity = Vector3.zero;
		
		holdable.Rigidbody.useGravity = true;
		holdable.Rigidbody.constraints = originalConstraints;

		currentJoint.connectedBody = null;
		holdable = null;
	}


	private void CheckJointState() {
		if (!holdable)
			return;

		float distance = Vector3.Distance(holdPoint.position, holdable.Rigidbody.position);

		if (distance > breakDistance) {
			holdable.Release();
		}
	}


	private void ConfigureJoint() {
		currentJoint.autoConfigureConnectedAnchor = false;

		currentJoint.anchor = Vector3.zero;
		currentJoint.connectedAnchor = Vector3.zero;

		currentJoint.xMotion = ConfigurableJointMotion.Limited;
		currentJoint.yMotion = ConfigurableJointMotion.Limited;
		currentJoint.zMotion = ConfigurableJointMotion.Limited;

		currentJoint.angularXMotion = ConfigurableJointMotion.Free;
		currentJoint.angularYMotion = ConfigurableJointMotion.Free;
		currentJoint.angularZMotion = ConfigurableJointMotion.Free;

		currentJoint.breakForce = Mathf.Infinity;
		currentJoint.breakTorque = Mathf.Infinity;

		SoftJointLimit limit = new SoftJointLimit { limit = jointLimit };
		currentJoint.linearLimit = limit;

		JointDrive drive = new JointDrive { positionSpring = positionSpring, positionDamper = positionDamper, maximumForce = maxForce };

		currentJoint.xDrive = drive;
		currentJoint.yDrive = drive;
		currentJoint.zDrive = drive;
	}


	private void OnDrawGizmos() {
		if (cam == null)
			cam = Camera.main;
		
		if (holdPoint && cam) {
			Ray ray = new(cam.transform.position, cam.transform.forward);
			float distance = (ray.origin - holdPoint.position).magnitude;
			Vector3 targetPos;

			if (Physics.Raycast(ray, out RaycastHit hit, distance, ignoreLayer)) {
				Vector3 offsetDir = (ray.origin - hit.point).normalized;
				targetPos = hit.point + offsetDir * offset;
			}
			else {
				targetPos = holdPoint.position;
			}

			Gizmos.color = Color.purple;
			Gizmos.DrawWireSphere(targetPos, 0.1f);
		}
	}


}