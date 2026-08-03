using UnityEngine;


public class PullHitPointTargetResolver : IHoldTargetResolver {

	public Vector3 GetTargetPosition(HoldContext context) {
		Vector3 target = context.HoldPoint.position + context.HoldPoint.TransformVector(context.PullGrabOffset);
		target.y = context.HitPoint.y;

		return target;
	}

}