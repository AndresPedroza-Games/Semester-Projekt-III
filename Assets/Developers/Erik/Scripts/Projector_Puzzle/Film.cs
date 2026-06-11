using UnityEngine;


public class Film : MonoBehaviour, IInteractable {

	[Header("---Rotation Config---")]
	[SerializeField] private GameObject decal;
	[SerializeField] private int rotationIncrement = 30;
	[SerializeField] private int correctAngle;

	private int CurrentAngle => Mathf.RoundToInt(transform.eulerAngles.y);

	public int CorrectAngle => correctAngle;
	public int Angle { get; private set; }


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");
		Angle = CurrentAngle;
		Debug.Log(Angle);
	}


	public bool CanInteract(HoldController holdController) {
		return true;
	}


	public void Interact() {
		Angle = (Angle + rotationIncrement) % 360;

		Rotate(transform, Vector3.up);
		Rotate(decal.transform, Vector3.forward);

		EventSystemPrincipalsOffice.Instance.FilmRotated();
	}


	private void Rotate(Transform trans, Vector3 axis) {
		Debug.Log($"angle: {trans.name} {Angle}");

		trans.rotation = Quaternion.Euler(axis * Angle);
	}


}