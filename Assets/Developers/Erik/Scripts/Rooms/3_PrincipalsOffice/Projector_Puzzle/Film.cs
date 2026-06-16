using UnityEngine;


public class Film : Holdable {

	[Header("---Rotation Config---")]
	[SerializeField] private GameObject decal;
	[SerializeField] private int rotationIncrement = 30;
	[SerializeField] private int correctAngle;

	public GameObject Decal => decal;
	public int CorrectAngle => correctAngle;
	public int Angle { get; private set; }
	public bool IsInserted { get; private set; }


	protected override void Awake() {
		base.Awake();

		decal.SetActive(false);
	}


	public override bool CanInteract(HoldController holdController) {
		return !IsInserted && !holdController.HasObject;
	}


	public void Rotate() {
		Angle = Mathf.RoundToInt(Angle + rotationIncrement + 360) % 360;

		UpdateVisuals();
	}


	public void Insert(Transform filmPosition) {
		CanBeHold = false;

		IsInserted = true;

		Rigidbody.isKinematic = true;

		transform.SetPositionAndRotation(filmPosition.position, filmPosition.rotation);
		transform.SetParent(filmPosition);

		Angle = 0;
		UpdateVisuals();
	}


	private void UpdateVisuals() {
		transform.localRotation = Quaternion.Euler(0f, Angle, 0f);

		decal.transform.localRotation = Quaternion.Euler(0f, 0f, -(Angle - CorrectAngle));;
	}

}