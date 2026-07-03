using System;
using UnityEngine;


public class HoldController : MonoBehaviour {

	public Holdable CurrentHoldable { get; private set; }
	public GameObject HoldGameObject => CurrentHoldable.GameObject();

	public bool HasObject => CurrentHoldable != null;

	public Picker Picker { get; private set; }

	public PhysicsHolder PhysicsHolder { get; private set; }


	private void Awake() {
		Picker = GetComponent<Picker>();
		PhysicsHolder = GetComponent<PhysicsHolder>();
	}


	private void OnEnable() {
		WorldSceneManager.onSceneUnloaded += ReleaseCurrentHoldable;
	}


	private void OnDisable() {
		WorldSceneManager.onSceneUnloaded -= ReleaseCurrentHoldable;
	}


	public void SetCurrentHoldable(Holdable holdable) {
		if (HasObject)
			return;

		CurrentHoldable = holdable;
	}


	public void ReleaseCurrentHoldable() {
		if (!HasObject)
			return;
	
		CurrentHoldable.Release();
		CurrentHoldable = null;
	}


	public void ClearCurrentHoldable() {
		CurrentHoldable = null;
	}

}