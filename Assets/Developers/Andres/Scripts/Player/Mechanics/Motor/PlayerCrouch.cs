using System.Collections;
using UnityEngine;

public class PlayerCrouch
{
    private PlayerMotor _PlayerMotor;
    private CharacterController _CharacterController;
    private Transform _Head;
    private Transform _Transform;

    [Header("----Crouch Settings----")]
    private float _CrouchHeight;
    private float _StandHeight;

    [Space(10)]
    private Vector3 _CrouchCenter;
    private Vector3 _StandCenter;

    [Space(10)]
    private float _TransitionSpeed = 0.1f;

    [Space(10)]
    private float _Radius;
    private LayerMask _HeadCollision;

    public void Init()
    {
        _PlayerMotor = PlayerMotor.playerMotor;

        _CrouchHeight = _PlayerMotor.crouchHeight;
        _StandHeight = _PlayerMotor.standHeight;

        _CrouchCenter = _PlayerMotor.crouchCenter;
        _StandCenter = _PlayerMotor.standCenter;

        _TransitionSpeed = _PlayerMotor.transitionSpeed;

        _Radius = _PlayerMotor.characterController.radius;
        _HeadCollision = _PlayerMotor.headCollision;

        _Head = _PlayerMotor.head;

        _CharacterController = _PlayerMotor.characterController;
        _Transform = _PlayerMotor.GetComponent<Transform>();
    }

    private Vector3 SetCameraView(float startPos, float endPos) {
        float crouchViewPos = Mathf.Lerp(startPos, endPos, _TransitionSpeed);
        Vector3 newPos = new (_Head.position.x, crouchViewPos, _Head.position.z);

        return newPos;
    }

    public IEnumerator Crouch()
    {
        if (!CanStandUp())
            yield break;

        float timeElapsed = 0f;

        float targetHeight = _PlayerMotor.isCrouching ? _StandHeight : _CrouchHeight;
        float currentHeight = _CharacterController.height;

        Vector3 targetCenter = _PlayerMotor.isCrouching ? _StandCenter : _CrouchCenter;
        Vector3 currentCenter = _CharacterController.center;

        while(timeElapsed < _TransitionSpeed)
        {
            _CharacterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / _TransitionSpeed);
            _CharacterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / _TransitionSpeed);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _CharacterController.height = targetHeight;
        _CharacterController.center = targetCenter;

        _PlayerMotor.isCrouching = !_PlayerMotor.isCrouching;

        _Head.position = SetCameraView(targetHeight, _PlayerMotor.isCrouching ? currentHeight : -5f);
    }

    private bool CanStandUp()
    {
        Vector3 pos = _Transform.position + Vector3.up * (_StandHeight - _Radius);

        return !Physics.CheckSphere(pos, _Radius, _HeadCollision);
    }
}
