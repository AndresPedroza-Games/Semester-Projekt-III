using UnityEngine;
using UnityEngine.UI;


public class InteractionUI : MonoBehaviour {


	[Header("---Crosshair Components---")]
	[SerializeField] private Image crosshairImage;
	[Space(5)]
	[SerializeField] private CrosshairDataSO crosshairData;

	private InteractionDetector _detector;
	private HoldController _holdController;

	private CrosshairType _currentType;

	private bool _hasItemInSocket;


	private void Awake() {
		_detector = GetComponent<InteractionDetector>();
		_holdController = GetComponent<HoldController>();

		ApplyType(CrosshairType.Default);
	}


	private void OnEnable() {
		EventSystemController.Instance.onItemPicked += OnItemPicked;
		EventSystemController.Instance.onItemDropped += OnItemDropped;
		EventSystemController.Instance.onMainMenuEntered += OnMainMenuEntered;
	}


	private void OnDisable() {
		EventSystemController.Instance.onItemPicked -= OnItemPicked;
		EventSystemController.Instance.onItemDropped -= OnItemDropped;
		EventSystemController.Instance.onMainMenuEntered -= OnMainMenuEntered;
	}


	private void OnMainMenuEntered() {
		_hasItemInSocket = false;
	}


	private void OnItemPicked(GameObject obj) {
		if (!obj.TryGetComponent(out Holdable holdable))
			return;

		_hasItemInSocket = holdable.HoldDefinition is SocketHoldDefinitionSO;

	}


	private void OnItemDropped(GameObject obj) {
		_hasItemInSocket = false;
	}


	private void Update() {
		UpdateCrosshair();
	}


	private void UpdateCrosshair() {
		CrosshairType newType = DetermineType();

		if (_currentType == newType)
			return;

		_currentType = newType;
		ApplyType(newType);

	}


	private void ApplyType(CrosshairType type) {
		Sprite sprite = crosshairData.GetSprite(type);

		if (sprite)
			crosshairImage.sprite = sprite;
	}


	private CrosshairType DetermineType() {

		if (_hasItemInSocket) {
			if (_detector.CurrentTarget == null)
				return CrosshairType.HandClosed;

			if (_detector.CurrentTarget.CanInteract(_holdController))
				return _detector.CurrentTarget.GetCrosshairType(_holdController);

			return CrosshairType.HandClosed;
		}

		if (_holdController.HasObject)
			return CrosshairType.HandClosed;

		if (_detector.CurrentTarget == null)
			return CrosshairType.Default;

		return _detector.CurrentTarget.GetCrosshairType(_holdController);

	}


}