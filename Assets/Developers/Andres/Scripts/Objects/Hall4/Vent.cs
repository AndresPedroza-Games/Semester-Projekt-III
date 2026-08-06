using DG.Tweening;
using UnityEngine;


public class Vent : MonoBehaviour, IInteractable, ICrosshair {

	[Header("Animation Settings")]
	[SerializeField] private Ease _Transition;
	[SerializeField] private float _Duration;
    private Tween _rotationTween;

    [Header("---Trigger---")]
    [SerializeField] private GameObject trigger;
    [SerializeField] private GameObject colWhenClosing;

    private Collider _Collider;
	private float _Angle;

	private Screwdriver _screwDriver;
	private DoorState _state;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");
		_Collider = GetComponent<Collider>();
		_state = DoorState.Closed;
	}


	private void Start() {
		//EventSystemController.Instance.onCloseDoor += CloseVent;
	}


	private void OnEnable() {
		EventSystemController.Instance.onItemPicked += OnItemPicked;
		EventSystemController.Instance.onItemDropped += OnItemDropped;
	}


	private void OnDisable() {
		EventSystemController.Instance.onItemPicked -= OnItemPicked;
		EventSystemController.Instance.onItemDropped -= OnItemDropped;
	}


	private void OnItemPicked(GameObject obj) {
		if (!obj.TryGetComponent(out Screwdriver screwdriver)) return;

		_screwDriver = screwdriver;
	}


	private void OnItemDropped(GameObject obj) {
		if (!obj.TryGetComponent(out Screwdriver screwdriver)) return;

		_screwDriver = null;
	}


	public bool CanInteract(HoldController holdController) {
		return _screwDriver && _state == DoorState.Closed;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return _screwDriver && _state == DoorState.Closed ? CrosshairType.Interactable : CrosshairType.Default;
	}


	public void Interact() {
		OpenVent();
	}


	private void OpenVent() {
		_screwDriver.UseItem();

		_Angle = -100f;
		AnimateVisuals(_Transition, _Duration);
		_Collider.enabled = false;
		Debug.Log("Opened");
	}


	public void CloseVent() {
		_Angle = 0f;
		AnimateVisuals(_Transition, _Duration);

        colWhenClosing.SetActive(true);

        if (trigger.activeSelf)
            trigger.SetActive(false);
    }


	private void AnimateVisuals(Ease ease, float duration) {
        _rotationTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, _Angle, 0f), duration).SetEase(ease).SetLink(gameObject);

        _rotationTween.OnComplete(() => {
            EventSystemController.Instance.DoorClosed(gameObject.scene.name);
        });
    }

}