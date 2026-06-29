using DG.Tweening;
using UnityEngine;

public class ShowerValve : MonoBehaviour, IInteractable
{
    private EventSystemBathroom _EventSystemBathroom;

    [SerializeField] private ParticleSystem _ParticleSystem;

    private ParticleSystem.EmissionModule  _Emission;

    [SerializeField] private Ease _Trasition;
    [SerializeField] private float _Duration;

    private float _Angle;
    private int _Steps;
    public static bool _IsCompleted;

    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;

        _IsCompleted = false;

        _Emission = _ParticleSystem.emission;
    }

    public bool CanInteract(HoldController holdController)
    {
        if (holdController.HasObject || _IsCompleted)
            return false;

        return true;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        return CrosshairType.Interactable;
    }

    public void Interact()
    {
        _EventSystemBathroom.InteractValve();
        Rotate();

        _ParticleSystem.Play();
        _Emission.rateOverTime = _Steps * 3;
    }

    private void Rotate()
    {
        if (_Steps == 5 && !_IsCompleted)
        {
            _EventSystemBathroom.door.canOpen = true;
            _IsCompleted = true;
            return;
        }

        _Steps++;

        _Steps = Mathf.Clamp(_Steps, 0, 5);

        _Angle = _Steps * 36f;

        AnimateVisuals(_Trasition, _Duration);
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, _Angle, 0f), duration).SetEase(ease).SetLink(gameObject);
    }
}
