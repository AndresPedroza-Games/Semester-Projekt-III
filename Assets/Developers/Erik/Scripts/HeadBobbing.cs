using UnityEngine;


public class HeadBobbing : MonoBehaviour {

	[Header("---Bobbing Config---")]
	[SerializeField] private float smoothSpeed = 8f;

	[Header("---Idle---")]
	[SerializeField] private float idleAmplitudeMultiplier = 0.2f;
	[SerializeField] private float idleFrequency = 1.5f;

	[Header("---Walking---")]
	[SerializeField] private float amplitude = 0.015f;
	[SerializeField] private float frequency = 10f;

	private const float ToggleSpeed = 1f;
	private float _bobTimer;

	private float _currentAmplitude;
	private float _currentFrequency;

	private Vector3 _lastOffset;

	private CharacterController _controller;


	private void Awake() {
		_controller = GetComponentInParent<CharacterController>();

		_currentAmplitude = amplitude * idleAmplitudeMultiplier;
		_currentFrequency = idleAmplitudeMultiplier;
	}


	void Update() {

		if (!_controller.isGrounded) {
			RemoveLastOffset();
			return;
		}

		float speed = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z).magnitude;

		bool isWalking = speed >= ToggleSpeed;

		float targetAmplitude = isWalking ? amplitude : amplitude * idleAmplitudeMultiplier;

		float targetFrequency = isWalking ? frequency : idleFrequency;

		_currentAmplitude = Mathf.Lerp(_currentAmplitude, targetAmplitude, smoothSpeed * Time.deltaTime);

		_currentFrequency = Mathf.Lerp(_currentFrequency, targetFrequency, smoothSpeed * Time.deltaTime);

		_bobTimer += Time.deltaTime * _currentFrequency;

		PlayMotion(BobMotion());
	}


	private Vector3 BobMotion() {
		Vector3 offset = Vector3.zero;

		offset.y = Mathf.Sin(_bobTimer) * _currentAmplitude;
		offset.x = Mathf.Cos(_bobTimer * 0.5f) * _currentAmplitude * 0.5f;

		return offset;
	}


	private void PlayMotion(Vector3 newOffset) {
		transform.localPosition -= _lastOffset;

		_lastOffset = Vector3.Lerp(_lastOffset, newOffset, smoothSpeed * Time.deltaTime);

		transform.localPosition += _lastOffset;
	}


	private void RemoveLastOffset() {
		transform.localPosition -= _lastOffset;
		_lastOffset = Vector3.zero;
	}

}