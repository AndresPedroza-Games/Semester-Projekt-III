using UnityEngine;


public class GrabTargetResolver : IHoldTargetResolver {

	public Vector3 GetTargetPosition(HoldContext context) {
		Ray ray = new(context.Camera.transform.position, context.Camera.transform.forward);
		float distance = (ray.origin - context.HoldPoint.position).magnitude;

		if (Physics.Raycast(ray, out RaycastHit hit, distance, context.IgnoreLayer)) {
			Vector3 offsetDir = (ray.origin - hit.point).normalized;
			return hit.point + offsetDir * context.Offset;
		}
		else {
			return context.HoldPoint.position;
		}
	}

}