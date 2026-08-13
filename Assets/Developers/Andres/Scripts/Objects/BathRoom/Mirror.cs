using System.Collections;
using TMPro;
using UnityEngine;


public class Mirror : MonoBehaviour, IInteractable, ICrosshair
{
    [Header("Fog Settings")]
    [SerializeField] private float _TransitionSpeed = 1f;
    [SerializeField] private float _Cooldown = 2f;
    [SerializeField] private float _MaxAlphaValue = 2f;
    [SerializeField] private TMP_Text _Text;

    [Header("Camera")]
    [SerializeField] private GameObject _Camera;
    private Transform _Player;

    private EventSystemBathroom _EventSystemBathroom;
    private bool _canInteract;


    private void Awake() {
	    gameObject.layer = LayerMask.NameToLayer("Interactable");
        _Player = FindAnyObjectByType<PlayerMotor>(FindObjectsInactive.Include).head;
    }


    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;

        _EventSystemBathroom.onTurnOnShower += () => StartCoroutine(AppearText());

        _EventSystemBathroom.onLockDoor += () => ChangeText("Scissors");
        _EventSystemBathroom.onTakeScissors += () => ChangeText("cut");
        _EventSystemBathroom.onCutHair += () => ChangeText("");
    }


    private void OnEnable() {
	    EventSystemController.Instance.onItemPicked += OnItemPicked;
	    EventSystemController.Instance.onItemDropped += OnItemDropped;
    }


    private void OnDisable() {
	    EventSystemController.Instance.onItemPicked -= OnItemPicked;
	    EventSystemController.Instance.onItemDropped -= OnItemDropped;
    }

    private void Update()
    {
        RotateCamera();
    }


    private void OnItemPicked(GameObject obj) {
	    if (!obj.TryGetComponent(out Holdable holdable)) return;
	    
	    if (holdable.HoldDefinition is SocketHoldDefinitionSO)
		    _canInteract = true;
    }
    
    
    private void OnItemDropped(GameObject obj) {
	    if (!obj.TryGetComponent(out Holdable holdable)) return;
	    
	    if (holdable.HoldDefinition is SocketHoldDefinitionSO)
		    _canInteract = false;
    }


    public bool CanInteract(HoldController holdController) {
	    return _canInteract;
    }

    public CrosshairType GetCrosshairType(HoldController holdController) {
	    return _canInteract ? CrosshairType.Interactable : CrosshairType.Default;
    }

    public void Interact()
    {
        _EventSystemBathroom.CutHair();
    }

    private void ChangeText(string text)
    {
        _Text.alpha = 0;
        _Text.text = text;

        StartCoroutine(AppearText());
    }

    private IEnumerator AppearText()
    {
        yield return new WaitForSeconds(_Cooldown);

        float timeElapsed = 0f;

        while (timeElapsed < _TransitionSpeed)
        {
            float t = timeElapsed / _TransitionSpeed;
            t = Mathf.SmoothStep(0f, 1f, t);

            _Text.alpha = Mathf.Lerp(0, _MaxAlphaValue, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    
    private void RotateCamera()
    {
        Vector3 cameraPos = _Camera.transform.position;
        Vector3 target = _Player.position;

        Vector3 direction = target - cameraPos;

        float angle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;

        _Camera.transform.localRotation = Quaternion.Euler(angle - 90f, direction.y, 0f);
    }
}
