using UnityEngine;

public class Fan : MonoBehaviour
{
    [Header("Wind Settings")]
    [SerializeField] private float _WindMaxForce;
    [SerializeField] private float _WindMinForce;
    [SerializeField] private bool _IsActive;

    private EventSystemHall4 _EventSystemHall4;

    private void Start()
    {
        _EventSystemHall4 = EventSystemHall4.instance;
        _EventSystemHall4.onPressPlate += DeactivateCollision;

        _IsActive = true;
    }

    private void DeactivateCollision()
    {
        gameObject.SetActive(false);
    }

    private float CalculateFoce(Collider other)
    {

        float distance = Vector3.Distance(transform.position, other.transform.position);

        float force = Mathf.Lerp(_WindMaxForce / 100f, _WindMinForce / 100f, distance / 10f);

        return force;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_IsActive) return;

        PlayerMotor player = other.GetComponent<PlayerMotor>();

        if (player != null)
        {
            player.externalForce = Vector3.left * CalculateFoce(other);
            Debug.Log("Push Player");
        }

    }

}
