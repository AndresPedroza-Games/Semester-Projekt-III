using System.Collections;
using DG.Tweening;
using UnityEngine;


public class LockPiece : MonoBehaviour, IInteractable, IHighlightable
{
    [Header("Animation Settings")]
    [SerializeField] private Ease _Ease;
    [SerializeField] private float _Duration = 0.3f;
    private float _Angle;
    private bool _canRotate = true;

    [Header("---On Piece Selected")]
    [SerializeField] float scaleMultiplier = 1.03f;
    
    [Header("---Highlight Config---")]
    [SerializeField] private float borderThickness = 0.02f;

    private EventSystemDoctorOffice _EventSystemDoctorOffice;

    private Vector3 _initialLocalScale;
    private bool _PieceIsSelected = false;

    public int _Steps;

    private Renderer _Renderer;
    private readonly int _BorderThickness = Shader.PropertyToID("_BorderThickness");

    private bool _CanInteract;

    private void Awake()
    {
        _CanInteract = true;
    }

    private void Start()
    {
        _EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
        _EventSystemDoctorOffice.onRotateLock += RotatePiece;
        _EventSystemDoctorOffice.onReleasePiece += ReleasePiece;
        _EventSystemDoctorOffice.onPuzzleCompleted += () => _CanInteract = false;

        _initialLocalScale = transform.localScale;
    }

    public bool CanInteract(HoldController holdController)
    {
        return true;
    }


    public CrosshairType GetCrosshairType(HoldController holdController) {
	    return _PieceIsSelected ? CrosshairType.HandClosed : CrosshairType.Interactable;
    }


    public void Interact()
    {
        if (!_PieceIsSelected && _CanInteract)
        {
            HighLightPiece(true);
            StartCoroutine(SetActive(true));
            Debug.Log($"Selected Piece {gameObject.name}");
        }
    }

    private void HighLightPiece(bool shouldHighlight) {
	    transform.localScale = shouldHighlight ? transform.localScale * scaleMultiplier : transform.localScale = _initialLocalScale;
	    //transform.position = new Vector3(transform.position.x, _StartPos + moveDistance, transform.position.z);
    }

    private void RotatePiece(Vector2 scroll)
    {
        if (!_PieceIsSelected || !_canRotate)
            return;

        float scrollY = scroll.y;

        switch (scrollY)
        {
            case > 0:
                _Steps++;
                break;

            case < 0:
                _Steps--;
                break;
        }

        _Steps = Mathf.Clamp(_Steps, -10, 10);

        if (_Steps == 10 || _Steps == -10)
            _Steps = 0;

        _Angle = _Steps * 36f;

        _canRotate = false;
        AnimateVisuals(_Ease, _Duration);
    }

    private void AnimateVisuals(Ease ease, float duration)
    {
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, _Angle, 0f), duration).SetEase(ease).OnComplete(() => _canRotate = true);
    }

    public void ReleasePiece()
    {
        if (_PieceIsSelected)
        {
            HighLightPiece(false);
            StartCoroutine(SetActive(false));
            Debug.Log("Release");
        }
    }

    public void Highlight()
    {
        if (!_Renderer)
            return;

        _Renderer.material.SetFloat(_BorderThickness, borderThickness);
    }

    public void RemoveHighlight()
    {
        if (!_Renderer)
            return;

        _Renderer.material.SetFloat(_BorderThickness, 0);
    }

    private IEnumerator SetActive(bool status)
    {
        yield return new WaitForSeconds(0.1f);
        _PieceIsSelected = status;
    }
}
