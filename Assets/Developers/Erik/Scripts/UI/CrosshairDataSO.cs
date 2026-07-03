using System;
using System.Collections.Generic;
using UnityEngine;


public enum CrosshairType {

	Default,
	Interactable,
	HandOpen,
	HandClosed,
	HandPointer,
	Lock,
	Eye,
	ArrowUp,
	ArrowDown,
	RotateCw

}


[CreateAssetMenu(fileName = "CrosshairDataSO", menuName = "Interaction/CrosshairData")]
public class CrosshairDataSO : ScriptableObject {

	[Serializable]
	public class Entry {

		public CrosshairType type;
		public Sprite sprite;

	}


	[SerializeField] private List<Entry> entries = new();

	private Dictionary<CrosshairType, Sprite> _lookup;


	private void OnEnable() {
		_lookup = new Dictionary<CrosshairType, Sprite>();

		foreach (Entry entry in entries)
			_lookup[entry.type] = entry.sprite;
	}


	public Sprite GetSprite(CrosshairType type) {
		return _lookup.GetValueOrDefault(type);
	}


}