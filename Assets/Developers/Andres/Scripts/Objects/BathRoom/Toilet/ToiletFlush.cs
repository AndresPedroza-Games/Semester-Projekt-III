using DG.Tweening;
using UnityEngine;


public class ToiletFlush : MonoBehaviour, IInteractable, ICrosshair {

	[Header("FLUSH")]
	[SerializeField] private ParticleSystem particles;
	[SerializeField] private LayerMask detectionLayer;

	[Header("ANIMATION")]
	[SerializeField] private Ease ease;
	[SerializeField] private float duration;
	[SerializeField] private Vector3 pulledRotation;

	[Header("Detection Settings")]
	[SerializeField] private Transform boxCenter;
	[SerializeField] private Vector3 halfExtends = new(0.3f, 0.3f, 0.3f);

	private readonly Collider[] _results = new Collider[10];
	private bool _canInteract = true;

	private Vector3 _scissorsStartPos;
	private bool _alreadyFlushed;


	private void Awake() {
		gameObject.layer = LayerMask.NameToLayer("Interactable");

		Scissors scissors = FindFirstObjectByType<Scissors>();

		if (scissors)
			_scissorsStartPos = scissors.transform.position;
	}


	public bool CanInteract(HoldController holdController) {
		return !holdController.HasObject && _canInteract;
	}


	public CrosshairType GetCrosshairType(HoldController holdController) {
		return _canInteract ? CrosshairType.Interactable : CrosshairType.Default;
	}


	public void Interact() {
		if (!_canInteract)
			return;

		Flush();
		_canInteract = false;
	}


	private void Flush() {
		transform.DORotate(pulledRotation, duration).SetEase(ease).SetLoops(2, LoopType.Yoyo).SetLink(gameObject).OnStepComplete(() => {
			if (_alreadyFlushed)
				return;

			DestroyObjectsInToilet();
			particles.Play();
			_alreadyFlushed = true;
		}).OnComplete(() => {
			_canInteract = true;
			_alreadyFlushed = false;
		});
	}


	private void DestroyObjectsInToilet() {
		int count = Physics.OverlapBoxNonAlloc(boxCenter.position, halfExtends, _results, Quaternion.identity, detectionLayer);

		for (int i = 0; i < count; i++) {

			Collider hit = _results[i];

			if (hit.TryGetComponent(out Scissors scissors)) {
				scissors.transform.rotation = Quaternion.identity;
				scissors.transform.position = _scissorsStartPos;
				continue;
			}

			Destroy(hit.gameObject);
		}

	}


#if UNITY_EDITOR
	private void OnDrawGizmosSelected() {
		if (!boxCenter)
			return;

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(boxCenter.position, halfExtends * 2);
	}
#endif

}