using UnityEngine;


[CreateAssetMenu(fileName = "PullDefinition", menuName = "Interaction/Hold Definitions/PhysicsPull")]
public class PullDefinitionRestrictedSO : HoldDefinition {

	public HoldPhysicsProfile profile; // = new HoldPhysicsProfile() {
	// 	useGravity = false,
	// 	useHitPoint = false,
	// 	constraints = RigidbodyConstraints.FreezeRotation,
	// 	xMotion = ConfigurableJointMotion.Free,
	// 	yMotion = ConfigurableJointMotion.Free,
	// 	zMotion = ConfigurableJointMotion.Free,
	// 	angularXMotion = ConfigurableJointMotion.Locked,
	// 	angularYMotion = ConfigurableJointMotion.Locked,
	// 	angularZMotion = ConfigurableJointMotion.Locked
	// };

	private readonly PullOriginTargetResolver resolver = new PullOriginTargetResolver();

	public override void Hold(Holdable holdable, HoldController holder, Vector3 hitPoint) {
		holder.PhysicsHolder.Hold(holdable, resolver, profile, hitPoint);
	}


	public override void Release(Holdable holdable, HoldController holder) {
		holder.PhysicsHolder.Release();
	}

}