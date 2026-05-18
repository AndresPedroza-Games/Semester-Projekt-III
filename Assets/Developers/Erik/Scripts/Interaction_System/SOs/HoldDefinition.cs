using UnityEngine;

public abstract class HoldDefinition : ScriptableObject
{
	public abstract void Hold(Holdable holdable, HoldController holder);

	public abstract void Release(Holdable holdable, HoldController holder);
}
