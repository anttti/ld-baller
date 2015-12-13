using UnityEngine;
using System.Collections;

public class HoopScoreDetector : MonoBehaviour {

	[SerializeField] private GameObject gameManager;
	[SerializeField] private int playerToScoreNumber;

	void OnTriggerEnter2D(Collider2D other) {
		gameManager.SendMessage ("ScorePointForPlayer", playerToScoreNumber);
	}
}
