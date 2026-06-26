using System.Collections.Generic;
using UnityEngine;


public class CubeFollow : MonoBehaviour {

	[Header("---Cube Config---")]
	public float followDistance;
	public float followSpeed;
	public List<Transform> checkpoints;
	public List<GameObject> floorObjects;

	private int _checkpointIndex = 0;

	private Transform _playerTransform;
	private Transform _currentTarget;
	private float _currentDistanceToPlayer;
	private float _currentDistanceToCheckPoint;

	private Rigidbody _rb;


	private void Start() {
		_rb = GetComponent<Rigidbody>();

		_playerTransform = GameManager.Instance.Player.transform;

		if (checkpoints.Count > 0)
			_currentTarget = checkpoints[_checkpointIndex];
	}


	private void FixedUpdate() {
		if (!_playerTransform || !_currentTarget)
			return;

		_currentDistanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

		if (_currentDistanceToPlayer <= followDistance)
			return;

		Vector3 targetPos = Vector3.MoveTowards(transform.position, _currentTarget.position, followSpeed * Time.deltaTime);

		_rb.MovePosition(targetPos);

		if (Vector3.Distance(transform.position, _currentTarget.position) == 0f) {
			if (_checkpointIndex < floorObjects.Count)
				floorObjects[_checkpointIndex].SetActive(false);

			_checkpointIndex++;

			if (_checkpointIndex < checkpoints.Count) {
				_currentTarget = checkpoints[_checkpointIndex];
			}
			else
				_currentTarget = null;
		}
	}

}