using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerMotor _PlayerMotor;

    private void Start()
    {
        _PlayerMotor = GetComponent<PlayerMotor>();
    }

    private void Update() {
        _PlayerMotor.Movement();
        _PlayerMotor.HandleGravity();
    }
}
