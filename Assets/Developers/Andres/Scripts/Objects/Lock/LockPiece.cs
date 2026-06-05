using UnityEngine;

public class LockPiece : MonoBehaviour, IInteractable
{
    private EventSystemDoctorOffice _EventSystemDoctorOffice;

    private float _StartPos;
    private float _MoveDistance = 0.01f;
    private bool _PieceIsSelected = false;

    public int _Steps;

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

    public void Interact()
    {
        if (!_PieceIsSelected)
        {
            MovePiece(_MoveDistance);
            _PieceIsSelected = true;
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

    private void ReleasePiece()
    {
        if (_PieceIsSelected)
        {
            MovePiece(0f);
            _PieceIsSelected = false;
            Debug.Log("Release");
        }
    }

}
