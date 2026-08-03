using UnityEngine;


public class PullOriginTargetResolver : IHoldTargetResolver {

	public Vector3 GetTargetPosition(HoldContext context) {
		Vector3 target = context.HoldPoint.position + context.PullGrabOffset;
		target.y = context.Holdable.Rigidbody.position.y;

		return target;
	}

}