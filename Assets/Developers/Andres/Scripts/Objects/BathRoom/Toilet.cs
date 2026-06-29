using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Toilet : MonoBehaviour, IInteractable
{
    [Header("Animation Settings")]

    [SerializeField] private Transform _PivotPoint;
    [SerializeField] private Ease _Transition;
    [SerializeField] private float _Duration;

    [Header("Detection Settings")]

    [SerializeField] private Vector3 _Size = new Vector3(0.3f,0.3f,0.3f);
    [SerializeField] private Transform _Center;
    [SerializeField] private float _MaxDistance;

    private float _Angle;
    private bool _IsOpen;
     
    public void Interact()
    {
        //OpenClose();
        _Angle = -0.003f;

        AnimateVisuals2(_Transition,_Duration);
        StartCoroutine(ReturnPos());

        Flush();
    }

    private void Flush()
    {
        foreach (RaycastHit hit in DetectObjects())
        {
            if(hit.collider.GetType() != typeof(PlayerManager))
                Debug.Log("Hit");
        }

        if (DetectObjects().Length <= 0)
            Debug.Log("Empty");
    }
    
    private RaycastHit[] DetectObjects()
    {
        return Physics.BoxCastAll(_Center.position, _Center.position, _Size, Quaternion.identity, _MaxDistance);
    }

    public bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        return CrosshairType.Interactable;
    }

    private void OpenClose()
    {
        _IsOpen = !_IsOpen;

        switch (_IsOpen)
        {
            case true:
                _Angle = 0;
                break;

            case false:
                _Angle = 90;
                break;
        }

        AnimateVisuals(_Transition, _Duration);
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        _PivotPoint.DOLocalRotateQuaternion(Quaternion.Euler(-_Angle, 0f, 0f), duration).SetEase(ease).SetLink(gameObject);
    }

    private void AnimateVisuals2(Ease ease, float duration)
    {
        _PivotPoint.DOLocalMoveY(_Angle, duration).SetEase(ease).SetLink(gameObject);
    }

    private IEnumerator ReturnPos()
    {
        yield return new WaitForSeconds(_Duration);

        _Angle = 0;
        AnimateVisuals2(_Transition, _Duration);
    }
}
