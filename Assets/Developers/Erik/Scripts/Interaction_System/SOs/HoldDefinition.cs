using UnityEngine;

public abstract class HoldDefinition : ScriptableObject
{
	public abstract void Hold(Holdable holdable, HoldController holder, Vector3 hitPoint);

	public abstract void Release(Holdable holdable, HoldController holder);
}
