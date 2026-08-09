using System.Collections;
using DG.Tweening;
using UnityEngine;


public class ShowerValve : MonoBehaviour, IInteractable, ICrosshair
{
    private EventSystemBathroom _EventSystemBathroom;

    [SerializeField] private VolumetricAdditionalLight _Fog;

    [Header("Fog Settings")]
    [SerializeField] private float _TransitionSpeed = 5f;

    [Header("Animation Settings")]
    [SerializeField] private Ease _Trasition;
    [SerializeField] private float _Duration;
    [SerializeField] private Transform _PivotPoint;
    [SerializeField] private float _Angle = 50f;

    [Header("Water Settings")]
    [SerializeField] private ParticleSystem _ParticleSystem;
    [SerializeField] private GameObject _WaterPrefab;
    [SerializeField] private float _TransitionSpeedWater = 5f;
    [SerializeField] private float maxWaterLevel = 5f;

    // private int _Steps;
    private float _Timer;
    private float yAxis;

    private bool _canInteract = true;
    public static bool _IsCompleted;


    private void Awake() {
	    gameObject.layer = LayerMask.NameToLayer("Interactable");
    }


    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;

        _IsCompleted = false;
    }


    public bool CanInteract(HoldController holdController) {
	    return !holdController.HasObject && _canInteract;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        if (CanInteract(holdController))
            return CrosshairType.Interactable;

        return CrosshairType.Default;
    }

    public void Interact() {
	    _canInteract = false;
	    
        _EventSystemBathroom.InteractValve();
        Rotate();

        StartCoroutine(IncreaseFog());
        StartCoroutine(IncreaseWater());

        _ParticleSystem.Play();
    }

    private void Rotate()
    {
        // _Steps++;

        // _Steps = Mathf.Clamp(_Steps, 0, 5);

        // _Angle = _Steps * 10f;

        _PivotPoint.DOLocalRotateQuaternion(Quaternion.Euler(0f, 0f, _Angle), _Duration).SetEase(_Trasition).SetLink(gameObject);
    }


    private IEnumerator IncreaseFog()
    {
        float timeElapsed = 0f;
        float startScattering = _Fog.Scattering;

        while (timeElapsed < _TransitionSpeed)
        {
            float t = timeElapsed / _TransitionSpeed;
            t = Mathf.SmoothStep(0f, 1f, t);

            _Fog.Scattering = Mathf.Lerp(startScattering, 16f, t);
	
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _Fog.Scattering = 16f;
    }

    private IEnumerator IncreaseWater()
    {
        float timeElapsed = 0f;
        float startY = yAxis;

        while (timeElapsed < _TransitionSpeedWater)
        {
            float t = timeElapsed / _TransitionSpeedWater;
            t = Mathf.SmoothStep(0f, 1f, t);

            yAxis = Mathf.Lerp(startY, maxWaterLevel, t);

            _WaterPrefab.transform.position = new Vector3(_WaterPrefab.transform.position.x, yAxis / 10, _WaterPrefab.transform.position.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yAxis = maxWaterLevel;
        _WaterPrefab.transform.position = new Vector3(_WaterPrefab.transform.position.x, yAxis / 10, _WaterPrefab.transform.position.z );
        
        _IsCompleted = true;
        _EventSystemBathroom.TurnOnShower();
        _ParticleSystem.Stop();
        
    }
}
