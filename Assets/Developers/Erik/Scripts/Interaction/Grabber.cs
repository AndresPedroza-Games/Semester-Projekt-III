using UnityEngine;


public class Grabber : MonoBehaviour {

	[Header("---Holdpoint---")]
	[SerializeField] private GameObject holdPoint;


	[Header(("---Joint Config---"))]
	[SerializeField] private float breakForce = 1500f;
	[SerializeField] private float breakTorque = 1500f;


	[Header("---Joint Driver Config---")]
	[SerializeField] private float positionSpring = 2000f;
	[SerializeField] private float positionDamper = 200f;
	[SerializeField] private float maxForce = 100f;

	private Rigidbody holdBody;
	private ConfigurableJoint currentJoint;
	private Rigidbody grabbedRb;

	public bool IsHolding { get; private set; }


	private void Start() {
		holdBody = holdPoint.AddComponent<Rigidbody>();
		holdBody.isKinematic = true;
	}


	private void ConfigureJoint() {
		currentJoint.connectedBody = holdBody;

		currentJoint.autoConfigureConnectedAnchor = false;

		currentJoint.anchor = Vector3.zero;
		currentJoint.connectedAnchor = Vector3.zero;

		currentJoint.xMotion = ConfigurableJointMotion.Limited;
		currentJoint.yMotion = ConfigurableJointMotion.Limited;
		currentJoint.zMotion = ConfigurableJointMotion.Limited;

		currentJoint.breakForce = breakForce;
		currentJoint.breakTorque = breakTorque;

		JointDrive drive = new JointDrive { positionSpring = positionSpring, positionDamper = positionDamper, maximumForce = maxForce };

		currentJoint.xDrive = drive;
		currentJoint.yDrive = drive;
		currentJoint.zDrive = drive;
	}


	public void Grab(Rigidbody rb) {
		if (grabbedRb != null) return;

		IsHolding = true;
		grabbedRb = rb;

		rb.useGravity = false;

		currentJoint = rb.gameObject.AddComponent<ConfigurableJoint>();

		ConfigureJoint();
	}


	public void Drop() {
		if (grabbedRb == null) return;

		grabbedRb.useGravity = true;

		Destroy(currentJoint);

		IsHolding = false;
		grabbedRb = null;
	}

}