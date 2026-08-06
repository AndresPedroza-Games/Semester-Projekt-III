using System;
using System.Collections;
using UnityEngine;

public class ToiletFlush : MonoBehaviour, IInteractable, ICrosshair
{
    [SerializeField] private ParticleSystem _ParticleSystem;
    [SerializeField] private float _CoolDown;

    private ToiletDetector _ToiletDetector;

    private bool _CanInteract;


    private void Awake() {
	    gameObject.layer = LayerMask.NameToLayer("Interactable");
    }


    private void Start()
    {
        _ToiletDetector = ToiletDetector.Instance;
        _CanInteract = true;
    }

    public bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject && _CanInteract;
    }

    public CrosshairType GetCrosshairType(HoldController holdController) {
	    return holdController.HasObject ? CrosshairType.Default : CrosshairType.Interactable;
    }

    public void Interact()
    {
        _ToiletDetector.Flush();
        _ParticleSystem.Play();
        _CanInteract = false;
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(_CoolDown);
        _CanInteract = true;
    }
}
