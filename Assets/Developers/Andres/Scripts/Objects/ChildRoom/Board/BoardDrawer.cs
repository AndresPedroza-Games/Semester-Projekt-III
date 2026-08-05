using DG.Tweening;
using UnityEngine;


public class BoardDrawer : Holdable {

	[Header("---Animation---")]
	[SerializeField] private Vector3 openedPosition;
	[SerializeField] private Ease ease;
	[SerializeField] private float animationDuration;

	protected override void Awake() {
		base.Awake();
		canBeHold = false;
	}


	private void OnEnable() {
		EventSystemChildRoom.eventSystemChildRoom.onPuzzleSolved += OpenDrawer;
	}


	protected override void OnDisable() {
		base.OnDisable();
		
		EventSystemChildRoom.eventSystemChildRoom.onPuzzleSolved -= OpenDrawer;
	}


	public override bool CanInteract(HoldController holdController) {
		return !holdController.HasObject && canBeHold;
	}


	public override CrosshairType GetCrosshairType(HoldController holdController) {
		return canBeHold ? CrosshairType.HandOpen : CrosshairType.Lock;
	}


	private void OpenDrawer() {
		transform.DOLocalMove(openedPosition, animationDuration).SetEase(ease).OnComplete(() => {
			canBeHold = true;
		});
		
	}

}