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
        if (!holdController.HasObject)
            return false;

        return holdController.HoldGameObject.GetComponent<Holdable>().HoldDefinition is SocketHoldDefinitionSO;
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
