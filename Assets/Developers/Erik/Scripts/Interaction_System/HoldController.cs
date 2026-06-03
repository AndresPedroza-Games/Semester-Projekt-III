using UnityEngine;


public class HoldController : MonoBehaviour {

	public IHoldable CurrentHoldable { get; private set; }

	public bool HasObject => CurrentHoldable != null;

	public Grabber Grabber { get; private set; }
	public Picker Picker { get; private set; }


	private void Awake() {
		Grabber = GetComponent<Grabber>();
		Picker = GetComponent<Picker>();
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