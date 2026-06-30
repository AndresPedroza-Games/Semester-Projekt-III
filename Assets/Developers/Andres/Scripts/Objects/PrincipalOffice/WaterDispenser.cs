using UnityEngine;
using System.Collections.Generic;

public class WaterDispenser : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _CupPrefab;
    [SerializeField] private Transform _CupSpawner;
    [SerializeField] private Transform _CupParent;
    [SerializeField] private int _MaxNumber;

    private List<GameObject> _CupList = new List<GameObject>();

    public bool CanInteract(HoldController holdController)
    {
        return !holdController.HasObject;
    }

    public CrosshairType GetCrosshairType(HoldController holdController)
    {
        return CrosshairType.Interactable; 
    }

    public void Interact()
    {
        CreateCup();
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
