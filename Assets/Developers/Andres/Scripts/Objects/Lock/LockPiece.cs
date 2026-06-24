using System.Collections;
using UnityEngine;

public class LockPiece : MonoBehaviour, IInteractable, IHighlightable
{

    [Header("---Highlight Config---")]
    [SerializeField] private float borderThickness = 0.02f;

    private EventSystemDoctorOffice _EventSystemDoctorOffice;

    private float _StartPos;
    private float _MoveDistance = 0.01f;
    private bool _PieceIsSelected = false;

    public int _Steps;

    private Renderer _Renderer;
    private readonly int _BorderThickness = Shader.PropertyToID("_BorderThickness");

    private void Start()
    {
        _EventSystemDoctorOffice = EventSystemDoctorOffice.instace;
        _EventSystemDoctorOffice.onRotateLock += RotatePiece;
        _EventSystemDoctorOffice.onReleasePiece += ReleasePiece;

        _StartPos = transform.position.y;
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
        if (!_PieceIsSelected)
        {
            MovePiece(_MoveDistance);
            StartCoroutine(SetActive(true));
            Debug.Log($"Selected Piece {gameObject.name}");
        }
    }

    private void MovePiece(float moveDistance)
    {
        transform.position = new Vector3(transform.position.x, _StartPos + moveDistance, transform.position.z);
    }

    private void RotatePiece(Vector2 scroll)
    {
        if (!_PieceIsSelected)
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

        transform.rotation = Quaternion.Euler(_Steps * 36f, 0f, 0f);
    }

    public void ReleasePiece()
    {
        if (_PieceIsSelected)
        {
            MovePiece(0f);
            StartCoroutine(SetActive(false));
            Debug.Log("Release");
            //RemoveHighlight();
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
