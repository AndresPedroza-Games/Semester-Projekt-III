using System.Collections;
using UnityEngine;

public class PlayerCrouch : PlayerController
{
    [Header("----Crouch Settings----")]
    [SerializeField] private float _CrouchHeight;
    [SerializeField] private float _StandHeight;

    [Space(10)]
    [SerializeField] private Vector3 _CrouchCenter;
    [SerializeField] private Vector3 _StandCenter;

    [Space(10)]
    [SerializeField] private float _TransitionSpeed = 0.1f;

    [Space(10)]
    [SerializeField] private float _Radius;
    [SerializeField] private LayerMask _HeadCollision;
    [SerializeField] protected KeyCode _CrouchKey = KeyCode.LeftShift;

    private void Start()
    {
        _Radius = characterController.radius;
    }

    public override void Crouch()
    {
        if(Input.GetKeyDown(_CrouchKey))
            StartCoroutine(CrouchStand());
    }

    private Vector3 SetCameraView(float startPos, float endPos)
    {
        float crouchViewPos = Mathf.Lerp(startPos, endPos, _TransitionSpeed);
        Vector3 newPos = new Vector3(mainCamera.transform.position.x, crouchViewPos, mainCamera.transform.position.z);

        return newPos;
    }

    private IEnumerator CrouchStand()
    {
        if (!CanStandUp())
            yield break;

        float timeElapsed = 0f;

        float targetHeight = isCrouching ? _StandHeight : _CrouchHeight;
        float currentHeight = characterController.height;

        Vector3 targetCenter = isCrouching ? _StandCenter : _CrouchCenter;
        Vector3 currentCenter = characterController.center;

        while(timeElapsed < _TransitionSpeed)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / _TransitionSpeed);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / _TransitionSpeed);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        ; isCrouching = !isCrouching;

        mainCamera.transform.position = SetCameraView(targetHeight, isCrouching ? currentHeight : -5f);

        Debug.Log("Is Crouching");
    }

    private bool CanStandUp()
    {
        Vector3 pos = transform.position + Vector3.up * (_CrouchHeight - _Radius);

        return !Physics.CheckSphere(pos, _Radius, _HeadCollision);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere( transform.position + Vector3.up * (_CrouchHeight - _Radius), _Radius);
    }
}
