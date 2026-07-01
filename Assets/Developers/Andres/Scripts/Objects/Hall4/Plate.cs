using DG.Tweening;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform _Design;

    [Header("Animation Settings")]
    [SerializeField] private Ease _Transition;
    [SerializeField] private float _Duration;
    [SerializeField] private float _Depth = 0.0003f;

    private bool _IsPressed;

    private void Awake()
    {
        _Design = GetComponentInChildren<Transform>();
    }

    private void PressPlate()
    {
        _IsPressed = true;
        Debug.Log("Pressed");
        AnimateVisuals(_Transition, _Duration);
        EventSystemHall4.instance.PressPlate();
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        transform.DOLocalMoveY(_Depth, duration).SetEase(ease).SetLink(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_IsPressed)
            PressPlate();
    }

}
