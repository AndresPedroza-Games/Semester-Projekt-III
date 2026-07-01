using DG.Tweening;
using UnityEngine;

public class Vent : MonoBehaviour, IInteractable
{
    [Header("Animation Settings")]
    [SerializeField] private Transform _Hinge;
    [SerializeField] private Ease _Transition;
    [SerializeField] private float _Duration;

    private float _Angle;

    public bool CanInteract(HoldController holdController)
    {
        return holdController.HasObject;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        return CrosshairType.Interactable;
    }

    public void Interact()
    {
        OpenVent();
    }

    private void OpenVent()
    {
        _Angle = -90f;
        AnimateVisuals(_Transition, _Duration);
        Debug.Log("Opened");
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        _Hinge.DOLocalRotateQuaternion(Quaternion.Euler(0f, _Angle, 0f), duration).SetEase(ease).SetLink(gameObject);
    }

}
