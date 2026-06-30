using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform _Design;

    private RaycastHit _Hit;

    private void Awake()
    {
        _Design = GetComponentInChildren<Transform>();
    }

    private void PressPlate()
    {
        
    }

    private RaycastHit[] DetectCollision()
    {
        return Physics.BoxCastAll(transform.position, _Design.localScale, Vector3.up);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, _Design.localScale);
    }
}
