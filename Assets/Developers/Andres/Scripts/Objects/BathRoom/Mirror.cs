using TMPro;
using UnityEngine;

public class Mirror : MonoBehaviour, IInteractable
{
    [SerializeField] private TMP_Text _Text;

    private EventSystemBathroom _EventSystemBathroom;

    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;

        _EventSystemBathroom.onLockDoor += () => _Text.text = "Pick scissors";
        _EventSystemBathroom.onTakeScissors += () => _Text.text = "Interact with mirror";
        _EventSystemBathroom.onCutHair += () => _Text.text = "";
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
