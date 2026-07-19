using UnityEngine;

public class Batery : Holdable
{
    [SerializeField] private ConfigurableJoint _Joint;
    [SerializeField] private ParticleSystem _Particles;

    private bool _Removed;

    private void Update()
    {
        if (_Joint == null && !_Removed)
            RemoveBatery();
    }

    public override bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }

    public override void Hold(HoldController holder)
    {
        base.Hold(holder);
    }

    public override void Release()
    {
        base.Release();
    }

    private void RemoveBatery()
    {
        _Particles.Play();
        EventSystemHall4.instance.ReleaseBatery();
        _Removed = true;
        Debug.Log("Removed");
    }
}
