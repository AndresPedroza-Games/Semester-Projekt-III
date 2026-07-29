using System;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class Hall0EventManager : MonoBehaviour {

	public static Hall0EventManager Instance { get; private set; }


	public Action onEvent001;
	public Action onKey001PickedUp;


	private void Awake() {
		if (Instance && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}


	public void Event001() {
		onEvent001?.Invoke();
	}


	public void Key001PickedUp() {
		onKey001PickedUp?.Invoke();
	}

}