using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Film : Holdable {

	[Header("---Decal---")]
	[SerializeField] private GameObject decal;
	[SerializeField] private Material decalSelectedMaterial;
	public GameObject Decal => decal;
	private DecalProjector _decalProjector;
	private Material _decalBaseMaterial;

	[Header("---Rotation Config---")]
	[SerializeField] private int rotationIncrement = 30;
	[SerializeField] private int correctAngle;

	public bool IsAnimating { get; private set; }
	public int CorrectAngle => correctAngle;
	public int Angle { get; private set; }
	public bool IsInserted { get; private set; }


	protected override void Awake() {
		base.Awake();

		decal.SetActive(false);
		_decalProjector = decal.GetComponent<DecalProjector>();
		_decalBaseMaterial = _decalProjector.material;
	}


	public override bool CanInteract(HoldController holdController) {
		return !IsInserted && !holdController.HasObject;
	}


	public void SetDecalSelectedMaterial(bool selected) {
		_decalProjector.material = selected ? decalSelectedMaterial : _decalBaseMaterial;
	}


	public void Rotate(Ease ease, float duration) {
		Angle = (Angle + rotationIncrement + 360) % 360;

		AnimateVisuals(ease, duration);
	}


	public void Insert(Transform filmPosition, Ease ease, float duration) {
		canBeHold = false;

		IsInserted = true;

		Rigidbody.isKinematic = true;
		Rigidbody.interpolation = RigidbodyInterpolation.None;
		Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		
		decal.transform.localRotation = Quaternion.Euler(0f, 0f, -(Angle - CorrectAngle));

		Sequence seq = DOTween.Sequence();

		seq.Join(transform.DOMove(filmPosition.position, duration).SetEase(ease));
		seq.Join(transform.DORotateQuaternion(filmPosition.rotation, duration).SetEase(ease));

		seq.OnComplete(() => {
			transform.SetParent(filmPosition);
			Angle = 0;
			ApplyVisuals();
		});
	}


	private void AnimateVisuals(Ease ease, float duration) {
		IsAnimating = true;

		Sequence seq = DOTween.Sequence();

		seq.SetEase(ease);

		seq.Join(transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, Angle, 0f), duration));

		seq.Join(decal.transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, 0f, -(Angle - CorrectAngle)), duration));

		seq.OnComplete(() => IsAnimating = false);
	}


	private void ApplyVisuals() {
		transform.localRotation = Quaternion.Euler(0f, Angle, 0f);

	}

}