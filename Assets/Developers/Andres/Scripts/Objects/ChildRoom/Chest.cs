using DG.Tweening;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [Header("Animation")]
    [SerializeField] private Ease _Ease;
    [SerializeField] private float _Duration;

    private bool _IsOpened;
    private float _Angle;

    public bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        if (CanInteract(holdController))
            return CrosshairType.Interactable;

        return CrosshairType.Default;
    }

    public void Interact()
    {
        _IsOpened = !_IsOpened;
        Animation();
    }

    private void Animation()
    {
        _Angle = _IsOpened ? -45f : 0f;

        transform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(_Angle, 0f,0f)), _Duration).SetEase(_Ease).SetLink(gameObject);

    }
}
