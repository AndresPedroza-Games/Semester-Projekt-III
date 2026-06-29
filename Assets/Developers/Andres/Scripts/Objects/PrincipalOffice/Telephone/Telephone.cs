using UnityEngine;

public class Telephone : Holdable
{
    public override bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }
}
