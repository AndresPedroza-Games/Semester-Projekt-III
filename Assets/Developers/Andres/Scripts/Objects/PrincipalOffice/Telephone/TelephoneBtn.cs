using DG.Tweening;
using UnityEngine;
using System.Collections;

public class TelephoneBtn : MonoBehaviour, IInteractable
{
    [SerializeField] private Ease _Transition;
    [SerializeField] private float _Duration;
    [SerializeField] private float _Depth = 0.003f;

    private float _Pos;

    public bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        return CrosshairType.Interactable;
    }

    public void Interact()
    {
        _Pos = _Depth;

        AnimateVisuals(_Transition, _Duration);
        StartCoroutine(ReturnPos());
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        transform.DOLocalMoveY(_Pos, duration).SetEase(ease).SetLink(gameObject);
    }

    private IEnumerator ReturnPos()
    {
        yield return new WaitForSeconds(_Duration);

        _Pos = 0;
        AnimateVisuals(_Transition, _Duration);
    }
}
