using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ShowerValve : MonoBehaviour, IInteractable
{
    private EventSystemBathroom _EventSystemBathroom;

    private VolumetricAdditionalLight _Fog;

    [Header("Fog Settings")]
    [SerializeField] private float _TransitionSpeed = 1f;

    [Header("Animation Settings")]
    [SerializeField] private Ease _Trasition;
    [SerializeField] private float _Duration;

    [Header("---Door---")]
    [SerializeField] private Door door;

    private float _Angle;
    private int _Steps;
    public static bool _IsCompleted;

    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;

        _IsCompleted = false;

        _Fog = FindFirstObjectByType<VolumetricAdditionalLight>(FindObjectsInactive.Include);
    }

    public bool CanInteract(HoldController holdController)
    {
        if (holdController.HasObject || _IsCompleted)
            return false;

        return true;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        return _IsCompleted ? CrosshairType.Default : CrosshairType.RotateCw;
    }

    public void Interact()
    {
        _EventSystemBathroom.InteractValve();
        Rotate();

        StartCoroutine(IncreaseFog());
    }

    private void Rotate()
    {
        _Steps++;
        
        if (_Steps == 5 && !_IsCompleted)
        {
            _IsCompleted = true;
            _EventSystemBathroom.TurnOnShower();
            door.SetDoorInteractionState(DoorInteractionState.Interactable);
        }


        _Steps = Mathf.Clamp(_Steps, 0, 5);

        _Angle = _Steps * 36f;

        AnimateVisuals(_Trasition, _Duration);
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, _Angle, 0f), duration).SetEase(ease).SetLink(gameObject);
    }

    private IEnumerator IncreaseFog()
    {
        float timeElapsed = 0f;

        while (timeElapsed < _TransitionSpeed)
        {
            float t = timeElapsed / _TransitionSpeed;
            t = Mathf.SmoothStep(0f, 1f, t);

            _Fog.Scattering = Mathf.Lerp(_Fog.Scattering, _Steps * 3, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

    }
}
