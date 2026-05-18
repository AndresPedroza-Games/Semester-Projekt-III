using UnityEngine;
using UnityEngine.UI;


public class InteractionUI : MonoBehaviour {

	[Header("---Crosshair Components---")]
	[SerializeField] private Image crosshairImage;
	[SerializeField] private Sprite crosshairDot;
	[SerializeField] private Sprite crosshairCircle;

	private InteractionDetector detector;
	private HoldController holdController;


	private void Awake() {
		detector = GetComponent<InteractionDetector>();
		holdController = GetComponent<HoldController>();
	}


	private void Update() {
		UpdateCrosshair();
	}


	private void UpdateCrosshair() {
		bool valid = detector.CurrentTarget != null && detector.CurrentTarget.CanInteract(holdController);

		crosshairImage.sprite = valid ? crosshairCircle : crosshairDot;
	}

}