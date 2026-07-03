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


	private void Awake() {
		_detector = GetComponent<InteractionDetector>();
		_holdController = GetComponent<HoldController>();
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
		if (_holdController.HasObject)
			return CrosshairType.HandClosed;

		if (_detector.CurrentTarget == null)
			return CrosshairType.Default;

		return _detector.CurrentTarget.GetCrosshairType(_holdController);
	}


}