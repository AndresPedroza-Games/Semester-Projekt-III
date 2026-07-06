using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;


public class Door : MonoBehaviour, IInteractable {

	[Header("---On Door Closed Event---")]
	public UnityEvent onDoorCloseAction;

	[Header("---Scenes To Unload---")]
	[SerializeField] private List<SceneReference> scenesToUnload;

	[Header("---Key---")]
	[SerializeField] private bool needsKeyToOpen;

	[Header("Animation Settings")]
	[SerializeField] private Vector3 openedRotation;
	[SerializeField] private Vector3 closedRotation;
	[SerializeField] private Ease ease;
	[SerializeField] private float duration = 1f;
	[SerializeField] private GameObject colWhenClosing;
	private Tween _rotationTween;

	private GameObject _currentPickedKey;
	private MeshCollider _meshCollider;

	private bool HasKey => _currentPickedKey != null;
	private bool _wasOpened;
	private bool _isClosing;
	private bool _isLocked;

	private EventSystemController _eventSystemController;


	private void Awake() {
		_meshCollider = GetComponent<MeshCollider>();
	}


	private void OnEnable() {
		_eventSystemController = EventSystemController.Instance;
		_eventSystemController.onItemPicked += OnItemPicked;
		_eventSystemController.onItemDropped += OnItemDropped;
	}


	private void OnDisable() {
		_eventSystemController.onItemPicked -= OnItemPicked;
		_eventSystemController.onItemDropped -= OnItemDropped;
	}


	private void OnItemPicked(GameObject obj) {
		if (obj.TryGetComponent(out Key key)) {
			_currentPickedKey = obj;
		}
	}


	private void OnItemDropped(GameObject obj) {
		_currentPickedKey = null;
	}


	public bool CanInteract(HoldController holdController) {
		if (_wasOpened)
			return false;

		if (_isLocked)
			return false;

		if (!needsKeyToOpen)
			return true;

		return HasKey;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		if (_wasOpened)
			return CrosshairType.Default;

		if (_isLocked)
			return CrosshairType.Lock;

		if (!needsKeyToOpen)
			return CrosshairType.Interactable;

		return HasKey ? CrosshairType.Interactable : CrosshairType.Lock;
	}


	public void Interact() {

		if (EventSystemBathroom.instance != null && ShowerValve._IsCompleted) {
			_isLocked = true;
			EventSystemBathroom.instance.LockDoor();
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
		if (needsKeyToOpen)
			_currentPickedKey.GetComponent<Key>().UseItem();

		Rotate(ease, duration, openedRotation);

		_wasOpened = true;

		_meshCollider.enabled = false;
	}


	public void CloseDoor() {
		_isClosing = true;

		colWhenClosing.SetActive(true);

		_meshCollider.enabled = false;

		Rotate(ease, duration, closedRotation);
	}


	private void Rotate(Ease e, float d, Vector3 targetRotation) {
		_rotationTween = transform.DOLocalRotateQuaternion(Quaternion.Euler(targetRotation), d).SetEase(e).SetLink(gameObject);

		_rotationTween.OnComplete(() => {
			_meshCollider.enabled = true;

			if (_isClosing) {
				UnloadScenes();
				onDoorCloseAction?.Invoke();
				EventSystemController.Instance.DoorClosed(gameObject.scene.name);
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

}