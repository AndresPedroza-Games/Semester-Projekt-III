using UnityEngine;


public class DisableHallPart : MonoBehaviour {

	[Header("---Hall Part to disable---")]
	[SerializeField] private GameObject hallPart;


	private void OnTriggerEnter(Collider other) {

		CubeFollow cube = other.GetComponent<CubeFollow>();

		if (!cube)
			return;

		if (!hallPart)
			return;

		hallPart.SetActive(false);
		gameObject.SetActive(false);
	}

}