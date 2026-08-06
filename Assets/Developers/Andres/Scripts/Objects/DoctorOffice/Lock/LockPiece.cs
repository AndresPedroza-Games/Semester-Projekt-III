using DG.Tweening;
using UnityEngine;


public class LockPiece : MonoBehaviour, IHighlightable, IInteractable, ILeftClickable , ICrosshair
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
    private bool IsSelected => Lock.SelectedPiece == gameObject;

    public int _Steps;

    private Renderer _Renderer;
    private readonly int _BorderThickness = Shader.PropertyToID("_BorderThickness");

    private bool _CanInteract;

    private void Awake()
    {
	    gameObject.layer = LayerMask.NameToLayer("Interactable");
        _CanInteract = true;
    }

    private void Start()
    {
        _EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
        _EventSystemDoctorOffice.onRotateLock += RotatePiece;
        _EventSystemDoctorOffice.onPieceSelected += OnPieceSelected;
        _EventSystemDoctorOffice.onPuzzleCompleted += () => _CanInteract = false;

        _initialLocalScale = transform.localScale;
    }


    private void OnDisable() {
	    _EventSystemDoctorOffice.onRotateLock -= RotatePiece;
        _EventSystemDoctorOffice.onPieceSelected -= OnPieceSelected;
    }


    public bool CanInteractWithLeftClick(HoldController holdController) {
	    return true;
    }


    public void OnLeftClick() {
	    if (!_CanInteract)
		    return;
	    
	    if(!IsSelected)
			SelectPiece();
	    else 
		    ReleasePiece();
    }


    public void Interact() {
	    
    }


    public bool CanInteract(HoldController holdController) {
	    return !IsSelected;
    }


    public CrosshairType GetCrosshairType(HoldController holdController) {
	    return IsSelected ? CrosshairType.HandClosed : CrosshairType.Interactable;
    }


    public void SelectPiece()
    {
        _EventSystemDoctorOffice.PieceSelected(gameObject);
    }


    private void OnPieceSelected(GameObject obj) {
	    HighLightPiece(obj == gameObject);
    }

    private void HighLightPiece(bool shouldHighlight) {
	    transform.localScale = shouldHighlight ? _initialLocalScale * scaleMultiplier : _initialLocalScale;
	    //transform.position = new Vector3(transform.position.x, _StartPos + moveDistance, transform.position.z);
    }

    private void RotatePiece(Vector2 scroll)
    {
        if (!IsSelected || !_canRotate)
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
        HighLightPiece(false);
        _EventSystemDoctorOffice.ReleasePiece();
    }


    public void ReleasePieceWithoutEventCall() {
        HighLightPiece(false);
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
}
