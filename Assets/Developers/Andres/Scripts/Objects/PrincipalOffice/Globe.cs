using DG.Tweening;
using UnityEngine;


public class Globe : Holdable
{
    [SerializeField] private Transform _PivotPoint;
    [SerializeField] private Ease _Transition;
    [SerializeField] private float _Duration;

    private int _Steps;
    private float _Angle;

    public override void Interact()
    {
        Rotate();
    }


    public override CrosshairType GetCrosshairType(HoldController holdController) {
	    return canBeHold ? CrosshairType.Interactable : CrosshairType.Default;
    }


    private void Rotate()
    {
        _Steps++;

        _Angle = _Steps * 36f;

        AnimateVisuals(_Transition, _Duration);
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        _PivotPoint.DOLocalRotateQuaternion(Quaternion.Euler(-64f, 90f, _Angle), duration).SetEase(ease).SetLink(gameObject);
    }
}
