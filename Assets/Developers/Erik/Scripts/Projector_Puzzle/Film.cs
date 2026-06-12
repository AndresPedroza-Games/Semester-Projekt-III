using UnityEngine;


public class Film : Holdable {

	[Header("---Rotation Config---")]
	[SerializeField] private GameObject decal;
	[SerializeField] private int rotationIncrement = 30;
	[SerializeField] private int correctAngle;

	public int CorrectAngle => correctAngle;
	public int Angle { get; private set; }
	public bool IsInserted { get; private set; }


	protected override void Awake() {
		base.Awake();
		
		decal.SetActive(false);
	}


	public void Rotate(float input) {
		
		Angle = Mathf.RoundToInt(Angle + rotationIncrement * input + 360) % 360;

		Debug.Log($"{name}: {Angle}; needs to be: {correctAngle}");
		
		UpdateVisuals();

		EventSystemPrincipalsOffice.Instance.FilmRotated();
	}


	private void UpdateVisuals() {
		transform.rotation = Quaternion.Euler(0f, Angle, 0f);

		decal.transform.rotation = Quaternion.Euler(0f, 0f, Angle - CorrectAngle);
	}


	public void Insert(Transform filmPosition) {
		CanBeHeld = false;
		
		IsInserted = true;

		decal.SetActive(true);

		Rigidbody.isKinematic = true;

		transform.SetPositionAndRotation(filmPosition.position, filmPosition.rotation);

		Angle = 0;
		UpdateVisuals();
	}


	public void Remove() {
		Debug.Log($"Removed: {gameObject.name}");
	}


}