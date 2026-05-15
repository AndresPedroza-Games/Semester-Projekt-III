using UnityEngine;


public class Grabber : MonoBehaviour {

	[Header("---Velocity---")]
	[SerializeField] private bool keepMomentum = true;

	[Header("---Hold Point---")]
	[SerializeField] private Transform holdPoint;
	[SerializeField] private float holdSmoothFollowSpeed = 15f;
	[Tooltip("Offset is used when the Hold Point is located within a different object")]
	[SerializeField] [Range(0.1f, 0.5f)] private float offset = 0.1f;

	[Header(("---Joint Config---"))]
	[SerializeField] private float breakForce = 1500f;
	[SerializeField] private float breakTorque = 1500f;

	[Header("---Joint Driver Config---")]
	[SerializeField] private float positionSpring = 800f;
	[SerializeField] private float positionDamper = 40f;
	[SerializeField] private float maxForce = 1000f;


	private Rigidbody grabbedRb;

	private Interactor interactor;

	private GameObject holdBody;
	private Rigidbody holdRb;
	private ConfigurableJoint currentJoint;
	private LayerMask ignoreLayer;

	private float grabPlayerYRotation;
	private Quaternion grabbedObjectRotation;


	private void Awake() {
		interactor = GetComponent<Interactor>();
		ignoreLayer = ~LayerMask.GetMask("Interactable");
	}


	private void Start() {
		holdBody = new GameObject("HoldBody");
		holdBody.AddComponent<Rigidbody>();

		holdRb = holdBody.GetComponent<Rigidbody>();
		holdRb.isKinematic = true;
		holdRb.useGravity = false;
		holdRb.interpolation = RigidbodyInterpolation.Interpolate;
		holdRb.collisionDetectionMode = CollisionDetectionMode.Continuous;

		holdBody.transform.position = holdPoint.position;
	}


	private void FixedUpdate() {
		MoveHoldPoint();
		RotateObject();
	}


	private void MoveHoldPoint() {
		Ray ray = new(interactor.cam.transform.position, interactor.cam.transform.forward);
		float distance = (ray.origin - holdPoint.position).magnitude;

		Vector3 targetPos;

		if (Physics.Raycast(ray, out RaycastHit hit, distance, ignoreLayer)) {
			Vector3 offsetDir = (ray.origin - hit.point).normalized;
			targetPos = hit.point + offsetDir * offset;
		}
		else {
			targetPos = holdPoint.position;
		}
		Vector3 smoothPos = Vector3.Lerp(holdRb.position, targetPos, holdSmoothFollowSpeed * Time.fixedDeltaTime );

		holdRb.MovePosition(smoothPos);
	}


	private void RotateObject() {
		if (!grabbedRb) return;

		float currentPlayerY = interactor.cam.transform.eulerAngles.y;

		float deltaY = currentPlayerY - grabPlayerYRotation;

		Quaternion targetRotation = Quaternion.Euler(0f, deltaY, 0f) * grabbedObjectRotation;

		holdRb.MoveRotation(targetRotation);
	}


	private void ConfigureJoint() {
		currentJoint.connectedBody = holdRb;

		currentJoint.autoConfigureConnectedAnchor = false;

		currentJoint.anchor = Vector3.zero;
		currentJoint.connectedAnchor = Vector3.zero;

		currentJoint.xMotion = ConfigurableJointMotion.Limited;
		currentJoint.yMotion = ConfigurableJointMotion.Limited;
		currentJoint.zMotion = ConfigurableJointMotion.Limited;

		currentJoint.angularXMotion = ConfigurableJointMotion.Limited;
		currentJoint.angularYMotion = ConfigurableJointMotion.Limited;
		currentJoint.angularZMotion = ConfigurableJointMotion.Limited;

		currentJoint.rotationDriveMode = RotationDriveMode.Slerp;
		JointDrive angularDrive = new JointDrive {
			positionSpring = 300f,
			positionDamper = 25f,
			maximumForce = maxForce
		};
		currentJoint.slerpDrive = angularDrive;
		currentJoint.targetRotation = Quaternion.identity;

		SoftJointLimit limit = new SoftJointLimit { limit = 0.5f };
		currentJoint.linearLimit = limit;

		currentJoint.breakForce = breakForce;
		currentJoint.breakTorque = breakTorque;

		JointDrive drive = new JointDrive { positionSpring = positionSpring, positionDamper = positionDamper, maximumForce = maxForce };

		currentJoint.xDrive = drive;
		currentJoint.yDrive = drive;
		currentJoint.zDrive = drive;
	}


	public void Grab(Rigidbody rb) {
		if (grabbedRb) return;

		grabPlayerYRotation = interactor.cam.transform.eulerAngles.y;
		grabbedObjectRotation = rb.rotation;

		grabbedRb = rb;

		rb.useGravity = false;
		//rb.constraints = RigidbodyConstraints.FreezeRotation;

		currentJoint = rb.gameObject.AddComponent<ConfigurableJoint>();

		ConfigureJoint();
	}


	public void Drop() {
		if (!grabbedRb) return;

		if (!keepMomentum)
			grabbedRb.linearVelocity = Vector3.zero;

		grabbedRb.useGravity = true;
		//grabbedRb.constraints = RigidbodyConstraints.None;

		Destroy(currentJoint);

		grabbedRb = null;
	}


	private void OnDrawGizmos() {
		if (holdPoint && interactor) {
			Ray ray = new(interactor.cam.transform.position, interactor.cam.transform.forward);
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