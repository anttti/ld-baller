using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	[SerializeField] private GameObject ballSpawnPoint;
	[SerializeField] private GameObject ball;
	[SerializeField] private GameObject bonusSpawnPoint;
	[SerializeField] private GameObject bonusBlock;
	[SerializeField] private Text scoreText;
	[SerializeField] private Text winnerText;
	[SerializeField] private Text guideText;
	[SerializeField] private Text player1ChargeText;
	[SerializeField] private Text player2ChargeText;
	[SerializeField] private GameObject player1;
	[SerializeField] private GameObject player2;

	[SerializeField] private AudioSource musicAudioSource;
	[SerializeField] private AudioClip introLoop;
	[SerializeField] private AudioClip mainLoop;

	[SerializeField] private AudioSource hitAudioSource;
	[SerializeField] private AudioSource powerupAudioSource;

	private const int SCORE_LIMIT = 5;

	private int player1Score = 0;
	private int player2Score = 0;

	private float nextBonusTime = 0;
	private float nextRoundStartTime = 0;
	private bool shouldCreateBonus = true;

	private bool isNextRoundAbleToStart = true;
	private bool isGameOngoing = false;
	private GameObject currentBall;

	void Awake() {
		winnerText.text = "Baller!";
	}

	void PlayHitSound() {
		hitAudioSource.pitch = 0.7f + Random.value / 2f;
		hitAudioSource.Play ();
	}

	void PlayPowerupSound() {
		powerupAudioSource.pitch = 0.7f + Random.value / 2f;
		powerupAudioSource.Play ();
	}

	void OnBonusDestroy() {
		nextBonusTime = Time.time + (float)Random.Range (1, 5);
		shouldCreateBonus = true;
		PlayPowerupSound ();
	}

	void UpdatePlayer1Charge(float currentCharge) {
		player1ChargeText.text = "Charge: " + ((int)currentCharge).ToString();
	}

	void UpdatePlayer2Charge(float currentCharge) {
		player2ChargeText.text = "Charge: " + ((int)currentCharge).ToString();
	}

	void Update() {
		if (!isGameOngoing && Input.anyKeyDown && isNextRoundAbleToStart) {
			StartGame ();
		}
		if (isGameOngoing && Time.time > nextBonusTime && shouldCreateBonus) {
			shouldCreateBonus = false;
			GameObject clone = Instantiate (bonusBlock, bonusSpawnPoint.transform.position, Quaternion.identity) as GameObject;
			clone.SendMessage ("SetGameManager", gameObject);
		}
	}

	void StartGame() {
		musicAudioSource.clip = mainLoop;
		musicAudioSource.Play ();
		player1Score = 0;
		player2Score = 0;
		UpdateScores ();
		currentBall = Instantiate (ball, ballSpawnPoint.transform.position, Quaternion.identity) as GameObject;
		currentBall.SendMessage ("SetGameManager", gameObject);
		winnerText.enabled = false;
		guideText.enabled = false;
		isGameOngoing = true;
		isNextRoundAbleToStart = false;
		player1.SendMessage ("EnableMovement");
		player2.SendMessage ("EnableMovement");
		player1.SendMessage ("SetBall", currentBall);
		player2.SendMessage ("SetBall", currentBall);
	}

	void EndGame() {
		musicAudioSource.clip = introLoop;
		musicAudioSource.Play ();
		Destroy (currentBall);
		isGameOngoing = false;
		if (player1Score >= SCORE_LIMIT) {
			winnerText.text = "Player 1 won!";
		} else if (player2Score >= SCORE_LIMIT) {
			winnerText.text = "Player 2 won!";
		}
		winnerText.enabled = true;
		player1.SendMessage ("DisableMovement");
		player2.SendMessage ("DisableMovement");
		Invoke("EnableNextRoundStart", 2);
	}

	void EnableNextRoundStart() {
		isNextRoundAbleToStart = true;
		guideText.enabled = true;
	}

	void UpdateScores() {
		scoreText.text = player1Score + " - " + player2Score;

		if (player1Score >= SCORE_LIMIT || player2Score >= SCORE_LIMIT) {
			EndGame ();
		}
	}

	void ScorePointForPlayer(int player) {
		if (!isGameOngoing) {
			return;
		}
		if (player == 1) {
			player1Score++;
		} else if (player == 2) {
			player2Score++;
		}
		UpdateScores ();
	}
}
