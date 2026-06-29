using UnityEngine;

public class Cup : Holdable
{
    public override bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }
}
