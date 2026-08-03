using UnityEngine;


public class HoldContext {
	
	public Transform HoldPoint;
	public Vector3 HitPoint;
	public Holdable Holdable;

	public Camera Camera;

	public float Offset;
	public LayerMask IgnoreLayer;
	
	public Vector3 PullGrabOffset;
}