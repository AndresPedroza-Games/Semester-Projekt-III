using UnityEngine;


public class OpenDoorOnEnable : MonoBehaviour {

	private Door _door;


	private void Awake() {
		_door = GetComponent<Door>();
	}


	private void OnEnable() {
		if (_door)
			_door.OpenDoorSimple();
	}

}