using System.Collections.Generic;
using UnityEngine;


public class WaterDispenser : MonoBehaviour, IInteractable, ICrosshair
{
    [SerializeField] private GameObject _CupPrefab;
    [SerializeField] private Transform _CupSpawner;
    [SerializeField] private Transform _CupParent;
    [SerializeField] private int _MaxNumber;

    private bool _canInteract = true;
    private List<GameObject> _CupList = new List<GameObject>();


    private void Awake() {
	    gameObject.layer = LayerMask.NameToLayer("Interactable");
    }


    public bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        return _canInteract ? CrosshairType.Interactable : CrosshairType.Default; 
    }

    public void Interact() {
	    if (!_canInteract)
		    return;
	        
        CreateCup();

        if (_CupList.Count >= _MaxNumber)
	        _canInteract = false;

    }

    private void CreateCup()
    {
        if(_CupList.Count < _MaxNumber)
        {
            GameObject cup = Instantiate(_CupPrefab, _CupSpawner.position, Quaternion.identity, _CupParent);

            _CupList.Add(cup);
        }
    }
}
