using System.Collections.Generic;
using UnityEngine;


public class CubeFollow : MonoBehaviour {

	[Header("---Cube Detection---")]
	[SerializeField] private LayerMask ignoreLayer;
	[SerializeField] private Transform boxCenter;
	[SerializeField] private Vector3 boxHalfExtends;
	private readonly Collider[] _results = new Collider[20];

	[Header("---Cube Config---")]
	public float followDistance;
	public float followSpeed;
	public List<Transform> checkpoints;

	private int _checkpointIndex;

	private Transform _playerTransform;
	private Transform _currentTarget;
	private float _currentDistanceToPlayer;

	private Rigidbody _rb;


	private void Start() {
		_rb = GetComponent<Rigidbody>();

		_playerTransform = GameManager.Instance.Player.transform;

		if (checkpoints.Count > 0)
			_currentTarget = checkpoints[_checkpointIndex];
	}


	private void FixedUpdate() {
		MoveToCheckPoints();
		DetectCollision();

	}


	private void DetectCollision() {
		int count = Physics.OverlapBoxNonAlloc(boxCenter.position, boxHalfExtends, _results, boxCenter.rotation);

		for (int i = 0; i < count; i++) {
			Collider hit = _results[i];

			if (hit.gameObject == gameObject)
				continue;

			if ((ignoreLayer.value & (1 << hit.gameObject.layer)) != 0)
				continue;

			if (hit.gameObject.scene == gameObject.scene) {
				hit.gameObject.SetActive(false);
			}
		}
	}


	private void MoveToCheckPoints() {
		if (!_playerTransform || !_currentTarget)
			return;

		_currentDistanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

		if (_currentDistanceToPlayer <= followDistance)
			return;

		Vector3 targetPos = Vector3.MoveTowards(transform.position, _currentTarget.position, followSpeed * Time.deltaTime);

		_rb.MovePosition(targetPos);

		if (Vector3.Distance(transform.position, _currentTarget.position) == 0f) {
			_checkpointIndex++;

			if (_checkpointIndex < checkpoints.Count) {
				_currentTarget = checkpoints[_checkpointIndex];
			}
			else
				_currentTarget = null;
		}
	}


#if UNITY_EDITOR
	private void OnDrawGizmos() {
		if (!boxCenter || boxHalfExtends == Vector3.zero)
			return;

		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(boxCenter.position, boxHalfExtends * 2);
	}
#endif

}