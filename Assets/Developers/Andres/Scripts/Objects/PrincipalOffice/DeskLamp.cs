using UnityEngine;

public class DeskLamp : Holdable
{
    [SerializeField] private Light _Bulb;

    private bool _IsActive;

    private void Start()
    {
        _IsActive = false;

        _Bulb.enabled = _IsActive;
    }

    public override void Interact()
    {
        _IsActive = !_IsActive;

        base.Interact();
        TurnOffOnLight(_IsActive);
    }

    private void TurnOffOnLight(bool status)
    {
        _Bulb.enabled = status;
    }

}
