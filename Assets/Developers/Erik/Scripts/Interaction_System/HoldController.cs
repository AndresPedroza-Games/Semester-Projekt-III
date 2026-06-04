using UnityEngine;


public class HoldController : MonoBehaviour {

	public IHoldable CurrentHoldable { get; private set; }
	public GameObject HoldGameObject => CurrentHoldable.GameObject();

	public bool HasObject => CurrentHoldable != null;

	public Picker Picker { get; private set; }

	public PhysicsHolder PhysicsHolder { get; private set; }


	private void Awake() {
		Picker = GetComponent<Picker>();
		PhysicsHolder = GetComponent<PhysicsHolder>();
	}


	public void SetCurrentHoldable(IHoldable holdable) {
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