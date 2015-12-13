using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	[SerializeField] private GameObject bonusSpawnPoint;
	[SerializeField] private GameObject bonusBlock;

	private int player1Score = 0;
	private int player2Score = 0;

	private float nextBonusTime = 0;
	private bool shouldCreateBonus = true;

	void OnBonusDestroy() {
		nextBonusTime = Time.time + (float)Random.Range (1, 5);
		shouldCreateBonus = true;
	}

	void Update() {
		if (Time.time > nextBonusTime && shouldCreateBonus) {
			shouldCreateBonus = false;
			GameObject clone = Instantiate (bonusBlock, bonusSpawnPoint.transform.position, Quaternion.identity) as GameObject;
			clone.SendMessage ("SetGameManager", gameObject);
		}
	}

	void UpdateScores() {
		Debug.Log ("Scores: " + player1Score + " - " + player2Score);
	}

	void ScorePointForPlayer(int player) {
		if (player == 1) {
			player1Score++;
		} else if (player == 2) {
			player2Score++;
		}
		UpdateScores ();
	}
}
