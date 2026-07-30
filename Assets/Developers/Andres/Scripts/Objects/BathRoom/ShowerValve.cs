using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ShowerValve : MonoBehaviour, IInteractable
{
    private EventSystemBathroom _EventSystemBathroom;

    [SerializeField] private VolumetricAdditionalLight _Fog;

    [Header("Fog Settings")]
    [SerializeField] private float _TransitionSpeed = 1f;

    [Header("Animation Settings")]
    [SerializeField] private Ease _Trasition;
    [SerializeField] private float _Duration;
    [SerializeField] private Transform _PivotPoint;

    [Header("Water Settings")]
    [SerializeField] private ParticleSystem _ParticleSystem;
    [SerializeField] private GameObject _WaterPrefab;
    [SerializeField] private float _TransitionSpeedWater = 1f;

    private float _Angle;
    private int _Steps;
    private float _Timer;
    private float yAxis = 0;

    public static bool _IsCompleted;

    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;

        _IsCompleted = false;

    }

    private void Update()
    {
        if (_Steps == 5 && !_IsCompleted)
        {
            _IsCompleted = true;
            _EventSystemBathroom.TurnOnShower();
            _ParticleSystem.Stop();
        }

    }

    public bool CanInteract(HoldController holdController)
    {
        if (holdController.HasObject || _IsCompleted)
            return false;

        return true;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        if (CanInteract(holdController))
            return CrosshairType.Interactable;

        return CrosshairType.Default;
    }

    public void Interact()
    {
        _EventSystemBathroom.InteractValve();
        Rotate();

        StartCoroutine(IncreaseFog());
        StartCoroutine(IncreaseWater());

        _ParticleSystem.Play();
    }

    private void Rotate()
    {
        _Steps++;

        _Steps = Mathf.Clamp(_Steps, 0, 5);

        _Angle = _Steps * 10f;

        AnimateVisuals(_Trasition, _Duration);
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        _PivotPoint.DOLocalRotateQuaternion(Quaternion.Euler(0f, 0f, _Angle), duration).SetEase(ease).SetLink(gameObject);
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

    private IEnumerator IncreaseWater()
    {
        float timeElapsed = 0f;
        float lastY = yAxis;

        while (timeElapsed < _TransitionSpeedWater)
        {
            float t = timeElapsed / _TransitionSpeedWater;
            t = Mathf.SmoothStep(0f, 1f, t);

            yAxis = lastY;
            yAxis = Mathf.Lerp(yAxis, _Steps, t);

            _WaterPrefab.transform.position = new Vector3(_WaterPrefab.transform.position.x, yAxis / 10, _WaterPrefab.transform.position.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
