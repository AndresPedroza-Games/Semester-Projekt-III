using UnityEngine;


[CreateAssetMenu(fileName = "PullDefinition", menuName = "Interaction/Hold Definitions/PhysicsPull")]
public class PullDefinitionSO : HoldDefinition {

	public HoldPhysicsProfile physicsProfile;

	private readonly HoldPhysicsProfile profile = new HoldPhysicsProfile() {
		useGravity = true,
		constraints = RigidbodyConstraints.FreezeRotation,
		xMotion = ConfigurableJointMotion.Free,
		yMotion = ConfigurableJointMotion.Free,
		zMotion = ConfigurableJointMotion.Free,
		angularXMotion = ConfigurableJointMotion.Locked,
		angularYMotion = ConfigurableJointMotion.Locked,
		angularZMotion = ConfigurableJointMotion.Locked
	};
	private readonly PullTargetResolver resolver = new PullTargetResolver();


	public override void Hold(Holdable holdable, HoldController holder, Vector3 hitPoint) {
		holder.PhysicsHolder.Hold(holdable, resolver, physicsProfile, hitPoint);
	}


	public override void Release(Holdable holdable, HoldController holder) {
		holder.PhysicsHolder.Release();
	}

}