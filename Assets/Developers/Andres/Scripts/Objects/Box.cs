using DG.Tweening;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Ease _Ease;
    [SerializeField] private float _Duration = 0.3f;
    private float _Angle;

    [SerializeField] private GameObject _Hinge;
    [SerializeField] private GameObject _Key;

    private EventSystemChildRoom _EventSystemChildRoom;

    private void Start()
    {
        _EventSystemChildRoom = EventSystemChildRoom.eventSystemChildRoom;
        _EventSystemChildRoom.onPuzzleSolved += OpenBox;
    }


    private void OpenBox()
    {
        Debug.Log("Box opened");
        RotateHinge();
        _Key.gameObject.SetActive(true);
    }

    private void RotateHinge()
    {
        _Angle = -45f;
        AnimateVisuals(_Ease,_Duration);
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        Sequence seq = DOTween.Sequence();

        seq.SetEase(ease);

        seq.Join(_Hinge.transform.DOLocalRotateQuaternion(Quaternion.Euler(_Angle, 0f, 0f), duration));
    }
}
