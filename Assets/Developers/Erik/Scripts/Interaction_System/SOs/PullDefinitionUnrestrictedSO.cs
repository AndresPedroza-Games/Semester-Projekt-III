using UnityEngine;


[CreateAssetMenu(fileName = "PullDefinitionUnrestricted", menuName = "Interaction/Hold Definitions/PhysicsPullUnrestricted")]
public class PullDefinitionUnrestrictedSO : HoldDefinition {

	public HoldPhysicsProfile profile; // = new HoldPhysicsProfile() {
	// 	keepMomentum = true,
	// 	useGravity = true,
	// 	constraints = RigidbodyConstraints.None,
	// 	xMotion = ConfigurableJointMotion.Free,
	// 	yMotion = ConfigurableJointMotion.Free,
	// 	zMotion = ConfigurableJointMotion.Free,
	// 	angularXMotion = ConfigurableJointMotion.Locked,
	// 	angularYMotion = ConfigurableJointMotion.Free,
	// 	angularZMotion = ConfigurableJointMotion.Locked
	// };
	private readonly PullTargetResolver resolver = new PullTargetResolver();


	public override void Hold(Holdable holdable, HoldController holder) {
		holder.PhysicsHolder.Hold(holdable, resolver, profile);
	}


	public override void Release(Holdable holdable, HoldController holder) {
		holder.PhysicsHolder.Release();
	}

}