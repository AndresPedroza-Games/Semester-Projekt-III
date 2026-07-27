using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class DoorWardrobe : MonoBehaviour, IInteractable {

	[Header("---Doors---")]
	[SerializeField] private GameObject[] wardrobeDoors = new GameObject[2];
	private readonly List<Collider> _wardrobeDoorColliders = new();

	[Header("---Door States---")]
	[SerializeField] private DoorState initDoorState;
	[SerializeField] private DoorInteractionState initDoorInteractionState;

	[Header("Animation Settings")]
	[SerializeField] private Vector3 openedRotationDoor1;
	[SerializeField] private Vector3 openedRotationDoor2;
	[SerializeField] private Ease ease;
	[SerializeField] private float duration = 1f;

	private GameObject _currentPickedKey;

	private DoorState _doorState;
	private DoorInteractionState _doorInteractionState;

	private bool HasKey => _currentPickedKey != null;

	private EventSystemController _eventSystemController;
	private ObjectSfx _sfx;

	private Collider _interactionCollider;


	private void Awake() {
		_interactionCollider = GetComponent<Collider>();

		_sfx = GetComponent<ObjectSfx>();
		_doorState = initDoorState;
		_doorInteractionState = initDoorInteractionState;

		foreach (GameObject door in wardrobeDoors) {
			Collider col = door.GetComponent<Collider>();
			if (col) {
				_wardrobeDoorColliders.Add(col);
			}
		}
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

		_interactionCollider.enabled = false;

		_sfx.PlaySfx(SfxEvent.OnInteract);

		Rotate(ease, duration);

		_doorState = DoorState.Open;

		foreach (Collider col in _wardrobeDoorColliders) {
			col.enabled = false;
		}
	}


	private void Rotate(Ease e, float d) {
		Sequence seq = DOTween.Sequence();
		seq.SetLink(gameObject);

		seq.Join(wardrobeDoors[0].transform.DOLocalRotateQuaternion(Quaternion.Euler(openedRotationDoor1), d).SetEase(e));
		seq.Join(wardrobeDoors[1].transform.DOLocalRotateQuaternion(Quaternion.Euler(openedRotationDoor2), d).SetEase(e));

		seq.OnComplete(() => {
			foreach (Collider col in _wardrobeDoorColliders) {
				col.enabled = true;
			}
			_doorInteractionState = DoorInteractionState.Disabled;
		});

	}

}