using UnityEngine;


public class Grabber : MonoBehaviour {

	[Header("---Velocity---")]
	[SerializeField] private bool keepMomentum;

	[Header("---Hold Point---")]
	[SerializeField] private Transform holdPoint;

	[Header(("---Joint Config---"))]
	[SerializeField] private float breakForce = 1500f;
	[SerializeField] private float breakTorque = 1500f;

	[Header("---Joint Driver Config---")]
	[SerializeField] private float positionSpring = 800f;
	[SerializeField] private float positionDamper = 40f;
	[SerializeField] private float maxForce = 1000f;

	private GameObject holdBody;
	private Rigidbody holdRb;
	private ConfigurableJoint currentJoint;
	public Rigidbody GrabbedRb { get; private set; }


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
		holdRb.MovePosition(holdPoint.position);
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
		if (GrabbedRb != null) return;

		GrabbedRb = rb;

		rb.useGravity = false;

		currentJoint = rb.gameObject.AddComponent<ConfigurableJoint>();

		ConfigureJoint();
	}


	public void Drop() {
		if (GrabbedRb == null) return;

		if (!keepMomentum)
			GrabbedRb.linearVelocity = Vector3.zero;

		GrabbedRb.useGravity = true;

		Destroy(currentJoint);

		GrabbedRb = null;
	}


	private void OnDrawGizmos() {
		if (holdPoint != null) {
			Gizmos.color = Color.orange;
			Gizmos.DrawWireSphere(holdPoint.position, 0.1f);
		}
	}


}