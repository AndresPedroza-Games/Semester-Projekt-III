using UnityEngine;

public class Mirror : MonoBehaviour, IInteractable
{
    private EventSystemBathroom _EventSystemBathroom;

    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;
    }

    public bool CanInteract(HoldController holdController)
    {
        if (holdController.HasObject.GetType() == typeof(Scissors))
            return true;

        return false;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        return CrosshairType.Interactable;
    }

    public void Interact()
    {
        _EventSystemBathroom.CutHair();
        Debug.Log("Start Cutscene");
    }
}
