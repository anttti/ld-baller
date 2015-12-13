using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour {

	private GameObject gameManager;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			gameManager.SendMessage ("OnBonusDestroy");
			Destroy (gameObject);
		}
	}

	void SetGameManager(GameObject mgr) {
		gameManager = mgr;
	}
}
