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

    public IEnumerator Crouch()
    {
        if (!CanStandUp())
            yield break;

        _PlayerMotor.isCrouching = !_PlayerMotor.isCrouching;       

        float timeElapsed = 0f;

        float targetHeight = _PlayerMotor.isCrouching ? _StandHeight : _CrouchHeight;
        float currentHeight = _CharacterController.height;

        Vector3 targetCenter = _PlayerMotor.isCrouching ? _StandCenter : _CrouchCenter;
        Vector3 currentCenter = _CharacterController.center;

        Vector3 startCamPos = _Head.localPosition;

        Vector3 targetCamPos = _PlayerMotor.isCrouching ? new Vector3( _Head.localPosition.x,_StandHeight,_Head.localPosition.z): new Vector3(_Head.localPosition.x,_CrouchHeight,_Head.localPosition.z);

        while (timeElapsed < _TransitionSpeed)
        {
            float t = timeElapsed / _TransitionSpeed;
            t = Mathf.SmoothStep(0f, 1f, t);

            _CharacterController.height = Mathf.Lerp(currentHeight, targetHeight, t);
            _CharacterController.center = Vector3.Lerp(currentCenter, targetCenter, t);

            _Head.localPosition = Vector3.Lerp(startCamPos, targetCamPos, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _CharacterController.height = targetHeight;
        _CharacterController.center = targetCenter;

        _Head.localPosition = targetCamPos;


        yield return new WaitForSeconds(_PlayerMotor.coolDownCrouch);
        _PlayerMotor.animationPlaying = !_PlayerMotor.animationPlaying;
    }

    private bool CanStandUp()
    {
        Vector3 pos = _Transform.position + Vector3.up * (_StandHeight - _Radius);

        return !Physics.CheckSphere(pos, _Radius, _HeadCollision);
    }
}
