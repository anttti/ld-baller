using UnityEngine;
using System.Collections;

public class HoopScoreDetector : MonoBehaviour {

	[SerializeField] private GameObject gameManager;
	[SerializeField] private int playerToScoreNumber;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Ball") {
			gameManager.SendMessage ("ScorePointForPlayer", playerToScoreNumber);
		}
	}
}
