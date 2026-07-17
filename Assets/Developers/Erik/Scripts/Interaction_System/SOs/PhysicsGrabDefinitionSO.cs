using UnityEngine;


[CreateAssetMenu(fileName = "PhysicsGrabDefinition", menuName = "Interaction/Hold Definitions/PhysicsGrab")]
public class PhysicsGrabDefinitionSO : HoldDefinition {

	private readonly HoldPhysicsProfile profile = new HoldPhysicsProfile() {
		keepMomentum = true,
		followRotation = true,
		constraints = RigidbodyConstraints.FreezeRotation,
		xMotion = ConfigurableJointMotion.Limited,
		yMotion = ConfigurableJointMotion.Limited,
		zMotion = ConfigurableJointMotion.Limited,
		angularXMotion = ConfigurableJointMotion.Free,
		angularYMotion = ConfigurableJointMotion.Free,
		angularZMotion = ConfigurableJointMotion.Free
	};
	private readonly GrabTargetResolver resolver = new GrabTargetResolver();


	public override void Hold(Holdable holdable, HoldController holder, Vector3 hitPoint) {
		holder.PhysicsHolder.Hold(holdable, resolver, profile, hitPoint);
	}


	public override void Release(Holdable holdable, HoldController holder) {
		holder.PhysicsHolder.Release();
	}

}