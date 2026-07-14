using UnityEngine;

public class ToiletFlush : MonoBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem _ParticleSystem;

    private ToiletDetector _ToiletDetector;

    private void Start()
    {
        _ToiletDetector = ToiletDetector.Instance;
    }

    public bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        if (!holdController.HasObject)
            return CrosshairType.Interactable;

        return CrosshairType.Default;
    }

    public void Interact()
    {
        _ToiletDetector.Flush();
        _ParticleSystem.Play();
    }
}
