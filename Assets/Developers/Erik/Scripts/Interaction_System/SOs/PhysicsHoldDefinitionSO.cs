using UnityEngine;

[CreateAssetMenu(fileName = "PhysicsHoldDefinition", menuName = "Interaction/Hold Definitions/Physics")]
public class PhysicsHoldDefinitionSO : HoldDefinition
{
	public override void Hold(Holdable holdable, HoldController holder)
	{
		holder.Grabber.Grab(holdable);
	}

	public override void Release(Holdable holdable, HoldController holder)
	{
		holder.Grabber.Drop();
	}
   
}
