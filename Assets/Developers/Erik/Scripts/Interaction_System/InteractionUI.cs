using UnityEngine;
using UnityEngine.UI;


public enum CrosshairType {

	Default,
	Interactable,
	HandOpen,
	HandClosed,
	HandPointer

}


public class InteractionUI : MonoBehaviour {


	[Header("---Crosshair Components---")]
	[SerializeField] private Image crosshairImage;

	[SerializeField] private Sprite crosshairDefault;
	[SerializeField] private Sprite crosshairInteractable;
	[SerializeField] private Sprite crosshairHandOpen;
	[SerializeField] private Sprite crosshairHandClosed;
	[SerializeField] private Sprite crosshairHandPointer;

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
		crosshairImage.sprite = type switch {
			CrosshairType.Default => crosshairDefault,
			CrosshairType.Interactable => crosshairInteractable,
			CrosshairType.HandOpen => crosshairHandOpen,
			CrosshairType.HandClosed => crosshairHandClosed,
			CrosshairType.HandPointer => crosshairHandPointer,
			_ => crosshairImage.sprite
		};
	}


	private CrosshairType DetermineType() {
		if (_holdController.HasObject)
			return CrosshairType.HandClosed;

		if (_detector.CurrentTarget == null || !_detector.CurrentTarget.CanInteract(_holdController))
			return CrosshairType.Default;

		return _detector.CurrentTarget.GetCrosshairType(_holdController);
	}


}