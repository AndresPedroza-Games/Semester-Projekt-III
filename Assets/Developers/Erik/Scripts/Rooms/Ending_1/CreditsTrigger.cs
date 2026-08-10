using UnityEngine;


public class CreditsTrigger : MonoBehaviour {

	[Header("---Credits---")]
	[SerializeField] private Credits credits;

	[Header("---Instant Credits Animation---")]
	[SerializeField] private bool instantAnim;


	private void OnTriggerEnter(Collider other) {
		if (!other.CompareTag("Player"))
			return;

		InputManager.Instance.Controls.Game.Disable();
		if (instantAnim)
			credits.PlayCreditsInstant();
		else
			credits.PlayCreditsAfterTimer();

		gameObject.SetActive(false);
	}

}