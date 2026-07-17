using UnityEngine;


public class PullTargetResolver : IHoldTargetResolver {

	public Vector3 GetTargetPosition(HoldContext context) {
		Vector3 target = context.HoldPoint.position + context.HoldPoint.TransformVector(context.PullGrabOffset);
		target.y = context.hitPoint.y;

		return target;
	}

}