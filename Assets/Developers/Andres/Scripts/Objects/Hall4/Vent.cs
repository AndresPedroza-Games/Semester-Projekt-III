using DG.Tweening;
using UnityEngine;

public class Vent : MonoBehaviour, IInteractable
{
    [Header("Animation Settings")]
    [SerializeField] private Ease _Transition;
    [SerializeField] private float _Duration;
    [SerializeField] private Transform _Hinge;
     
    private Collider _Collider;
    private float _Angle;

    private void Awake()
    {
        _Collider = GetComponent<MeshCollider>();
    }

    private void Start()
    {
         //EventSystemController.Instance.onCloseDoor += CloseVent;
    }

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
        _Angle = -100f;
        AnimateVisuals(_Transition, _Duration);
        _Collider.enabled = false;
        Debug.Log("Opened");
    }

     private void CloseVent()
     {
         _Angle = 0f;
         AnimateVisuals(_Transition, _Duration);
         Debug.Log("Closed");
     }

    private void AnimateVisuals(Ease ease, float duration)
    {
        _Hinge.DOLocalRotateQuaternion(Quaternion.Euler(0f, _Angle, 0f), duration).SetEase(ease).SetLink(gameObject);
    }

}
