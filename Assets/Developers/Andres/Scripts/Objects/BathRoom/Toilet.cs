using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Toilet : MonoBehaviour, IInteractable
{
    [Header("Animation Settings")]

    [SerializeField] private Ease _Transition;
    [SerializeField] private float _Duration;

    [Header("Detection Settings")]
    [SerializeField] private Vector3 _Size = new Vector3(0.3f,0.3f,0.3f);
    [SerializeField] private Transform _Center;
    [SerializeField] private float _MaxDistance;

    private float _Angle;
    private bool _IsOpen;

    private void Awake()
    {
        _IsOpen = false;
    }

    private void Update()
    {
        if(_IsOpen)
            Flush();

    }

    public void Interact()
    {
        OpenClose();
    }

    private void Flush()
    {
        foreach (RaycastHit hit in DetectObjects())
        {
            if(hit.collider.GetType() != typeof(PlayerManager) && hit.collider.GetType() != typeof(Scissors) && hit.collider.GetType() != typeof(Toilet))
                Debug.Log(hit.collider.name);
        }

        if (DetectObjects().Length <= 0)
            Debug.Log("Empty");
    }
    
    private RaycastHit[] DetectObjects()
    {
        return Physics.BoxCastAll(_Center.position, _Center.position, _Size, Quaternion.identity,_MaxDistance);
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
            case false:
                _Angle = 0;
                break;

            case true:
                _Angle = 60;
                break;
        }

        AnimateVisuals(_Transition, _Duration);
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        transform.DOLocalRotateQuaternion(Quaternion.Euler(-_Angle, 0f, 0f), duration).SetEase(ease).SetLink(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(_Center.position, _Size);
    }
}
