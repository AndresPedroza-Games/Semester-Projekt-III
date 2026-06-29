using UnityEngine;

public class TowerToy : Holdable
{
    public override bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }
}
