using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;


public enum DoorState {

	Open,
	Closed

}


public enum DoorInteractionState {

	Disabled,
	Interactable,
	RequiresKey,
	Locked

}


public class Door : MonoBehaviour, IInteractable, ICrosshair {

	[Header("---Trigger---")]
	[SerializeField] private GameObject trigger;

	[Header("---On Door Closed Event---")]
	public UnityEvent onDoorCloseAction;

	[Header("---Scenes To Unload---")]
	[SerializeField] private List<SceneReference> scenesToUnload;

	[Header("---Door States---")]
	[SerializeField] private DoorState initDoorState;
	[SerializeField] private DoorInteractionState initDoorInteractionState;

	[Header("Animation Settings")]
	[SerializeField] private Vector3 openedRotation;
	[SerializeField] private Vector3 closedRotation;
	[SerializeField] private Ease ease;
	[SerializeField] private float duration = 1f;
	[SerializeField] private GameObject colWhenClosing;
	private Tween _rotationTween;

	private GameObject _currentPickedKey;
	private MeshCollider _meshCollider;

	private DoorState _doorState;
	private DoorInteractionState _doorInteractionState;

	private bool HasKey => _currentPickedKey;

	private EventSystemController _eventSystemController;
	private ObjectSfx _sfx;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");
		_meshCollider = GetComponent<MeshCollider>();
		_sfx = GetComponent<ObjectSfx>();
		_doorState = initDoorState;
		_doorInteractionState = initDoorInteractionState;
	}


	private void Start() {
		if (EventSystemBathroom.instance)
			EventSystemBathroom.instance.onTurnOnShower += () => _doorInteractionState = DoorInteractionState.Interactable;
	}


	private void OnEnable() {
		_eventSystemController = EventSystemController.Instance;
		_eventSystemController.onItemPicked += OnItemPicked;
		_eventSystemController.onItemDropped += OnItemDropped;


		HoldController holdController = GameManager.Instance.Interactor.GetComponent<HoldController>();
		if (holdController && holdController.HasObject)
			OnItemPicked(holdController.HoldGameObject);
	}


	private void OnDisable() {
		_eventSystemController.onItemPicked -= OnItemPicked;
		_eventSystemController.onItemDropped -= OnItemDropped;
	}


	private void OnItemPicked(GameObject obj) {
		if (_doorInteractionState != DoorInteractionState.RequiresKey) return;

		if (obj.TryGetComponent(out Key _)) {
			_currentPickedKey = obj;
		}
	}


	private void OnItemDropped(GameObject obj) {
		if (_doorInteractionState == DoorInteractionState.RequiresKey)
			_currentPickedKey = null;
	}


	public bool CanInteract(HoldController holdController) {
		if (_doorState == DoorState.Open)
			return false;

		switch (_doorInteractionState) {
			case DoorInteractionState.Disabled:
			case DoorInteractionState.Locked:
				break;

			case DoorInteractionState.Interactable:
				return true;

			case DoorInteractionState.RequiresKey:
				return HasKey;

		}

		return false;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		if (_doorState == DoorState.Open)
			return CrosshairType.Default;

		switch (_doorInteractionState) {
			case DoorInteractionState.Disabled:
				break;

			case DoorInteractionState.Locked:
				return CrosshairType.Lock;

			case DoorInteractionState.RequiresKey:
				return HasKey ? CrosshairType.Interactable : CrosshairType.Lock;

			case DoorInteractionState.Interactable:
				return CrosshairType.Interactable;

		}

		return CrosshairType.Default;
	}


	public void Interact() {

		if (EventSystemBathroom.instance && ShowerValve._IsCompleted) {
			EventSystemBathroom.instance.LockDoor();
			_doorInteractionState = DoorInteractionState.Locked;
			Debug.Log("Locked");
		}

		if (!CanInteract(null)) {
			return;
		}

		OpenDoor();
		//else
		//PlayLockedDoorSound...

	}


	private void OpenDoor() {
		if (_doorInteractionState == DoorInteractionState.RequiresKey)
			_currentPickedKey.GetComponent<Key>().UseItem();

		_sfx?.PlaySfx(SfxEvent.OnInteract);

		Rotate(ease, duration, openedRotation);

		_doorState = DoorState.Open;

		_meshCollider.enabled = false;
	}


	public void CloseDoor() {
		colWhenClosing?.SetActive(true);

		if (trigger && trigger.activeSelf)
			trigger.SetActive(false);

		_meshCollider.enabled = false;

		_doorState = DoorState.Closed;

		Rotate(ease, duration, closedRotation);
	}


	private void Rotate(Ease e, float d, Vector3 targetRotation) {
		_rotationTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(targetRotation), d).SetEase(e).SetLink(gameObject);

		_rotationTween.OnComplete(() => {
			_meshCollider.enabled = true;

			if (_doorState == DoorState.Closed) {
				_sfx?.PlaySfx(SfxEvent.OnDoorClose);
				UnloadScenes();
				onDoorCloseAction?.Invoke();
				EventSystemController.Instance.DoorClosed(gameObject.scene.name);
				_doorInteractionState = DoorInteractionState.Disabled;

				FindFirstObjectByType<CameraShake>(FindObjectsInactive.Include).Shake();
			}
		});

	}


	private async void UnloadScenes() {
		if (scenesToUnload.Count > 0) {
			for (int i = 0; i < scenesToUnload.Count; i++) {
				await WorldSceneManager.Instance.UnloadScene(scenesToUnload[i]);
			}
		}
	}


	public void CloseDoorWithoutExtras() {
		colWhenClosing?.SetActive(true);

		if (trigger.activeSelf)
			trigger.SetActive(false);

		_meshCollider.enabled = true;

		_doorState = DoorState.Closed;
		_doorInteractionState = DoorInteractionState.Disabled;
		transform.localEulerAngles = closedRotation;

		onDoorCloseAction?.Invoke();
		EventSystemController.Instance.DoorClosed(gameObject.scene.name);
	}


	public void SetDoorInteractionState(DoorInteractionState newState) {
		_doorInteractionState = newState;
	}

}