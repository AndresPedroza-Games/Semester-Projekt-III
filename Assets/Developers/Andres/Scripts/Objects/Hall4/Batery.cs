using UnityEngine;


public class Batery : Holdable
{
    [SerializeField] private ConfigurableJoint _Joint;
    [SerializeField] private ParticleSystem _Particles;

    private Joint _joint;


    protected override void Awake() {
	    base.Awake();

	    _joint = GetComponent<Joint>();
    }

    private void OnJointBreak(float breakForce) {
	    RemoveBatery();
    }

    public override bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }

    private void RemoveBatery()
    {
        _Particles.Play();
        EventSystemHall4.instance.ReleaseBatery();
        Debug.Log("Removed");
    }
}
