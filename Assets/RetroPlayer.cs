using UnityEngine;
using System.Collections;

public class RetroPlayer : MonoBehaviour {

	[SerializeField] private KeyCode jump;
	[SerializeField] private KeyCode punch;
	[SerializeField] private AudioSource jumpAudioSource;
	[SerializeField] private GameObject gameManager;
	[SerializeField] private int playerNumber;
	[SerializeField] private GameObject hand;

	private const float MAX_JUMP_FORCE = 100f;
	private const float JUMP_FORCE_CHARGE_SPEED = 45f;

	private Rigidbody2D rigidbody;
	private HingeJoint2D hingeJoint;
	private Rigidbody2D handRigidbody;
	private int specialPowerDuration = 5;
	private bool isMovementDisabled = false;
	private GameObject ball;
	private float currentJumpForce = 0;
	private bool isCharging = false;

	void Awake () {
		rigidbody = GetComponent<Rigidbody2D> ();
		hingeJoint = GetComponent<HingeJoint2D> ();
		handRigidbody = hand.GetComponent<Rigidbody2D> ();
	}

	void SetBall(GameObject currentBall) {
		ball = currentBall;
	}

	void Update () {
		if (isMovementDisabled) {
			return;
		}

		// Move always towards the ball
		if (!ball) {
			return;
		}
		if (ball.transform.position.x < transform.position.x) {
			rigidbody.AddForce (new Vector2 (-100f, 0));
		} else {
			rigidbody.AddForce (new Vector2 (100f, 0));
		}

		if (!isCharging && Input.GetKeyDown (jump)) {
			currentJumpForce = 0;
			isCharging = true;
		}
		if (isCharging) {
			float delta = Time.deltaTime * JUMP_FORCE_CHARGE_SPEED;
			currentJumpForce = Mathf.Clamp (currentJumpForce + delta, currentJumpForce, MAX_JUMP_FORCE);
			UpdateCharge (currentJumpForce);
		}
		if (isCharging && Input.GetKeyUp (jump)) {
			rigidbody.AddForce (new Vector2 (0, currentJumpForce * 12), ForceMode2D.Impulse);
			isCharging = false;
			UpdateCharge (0f);
		}

		if (Input.GetKeyDown (punch)) {
			float force = playerNumber == 1 ? 20f : -20f;
			handRigidbody.AddForceAtPosition (new Vector2 (force, force), new Vector2 (0, 0), ForceMode2D.Impulse);
		}
	}

	void UpdateCharge(float charge) {
		if (playerNumber == 1) {
			gameManager.SendMessage ("UpdatePlayer1Charge", charge);
		} else {
			gameManager.SendMessage ("UpdatePlayer2Charge", charge);
		}
	}

	void Jump(float dir) {
		rigidbody.AddForce (new Vector2 (dir, 100f), ForceMode2D.Impulse);
		PlayJumpSound ();
	}

	void PlayJumpSound() {
		jumpAudioSource.pitch = 0.7f + Random.value / 2f;
		jumpAudioSource.Play ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Bonus") {
			int random = Random.Range (0, 4);
			switch (random) {
			case 0:
				SpecialPowerGrow ();
				return;
			case 1:
				SpecialPowerShrink ();
				return;
			case 2:
				SpecialPowerFat ();
				return;
			case 3:
				SpecialPowerMayhem ();
				return;
			}
		}
	}

	void SpecialPowerGrow()
	{
		transform.localScale = new Vector3 (2, 2, 1);
		gameManager.SendMessage ("UpdateMode", "HUGE MODE");
		Invoke("RemoveSpecialPowers", specialPowerDuration);
	}

	void SpecialPowerShrink() {
		transform.localScale = new Vector3 (0.5f, 0.5f, 1);
		gameManager.SendMessage ("UpdateMode", "TINY MODE");
		Invoke("RemoveSpecialPowers", specialPowerDuration);
	}

	void SpecialPowerFat() {
		transform.localScale = new Vector3 (2.5f, 1, 1);
		gameManager.SendMessage ("UpdateMode", "FATZO MODE");
		Invoke("RemoveSpecialPowers", specialPowerDuration);
	}

	void SpecialPowerMayhem() {
		SetMotorSpeed (1800);
		gameManager.SendMessage ("UpdateMode", "MAYHEM!!1");
		Invoke("RemoveSpecialPowers", specialPowerDuration);
	}

	void RemoveSpecialPowers() {
		gameManager.SendMessage ("UpdateMode", "");
		if (isMovementDisabled) {
			return;
		}
		transform.localScale = new Vector3 (1, 1, 1);
		hingeJoint.useMotor = false;
	}

	void EnableMovement() {
		currentJumpForce = 0;
		isMovementDisabled = false;
	}

	void DisableMovement() {
		isMovementDisabled = true;
		hingeJoint.useMotor = false;
	}

	void SetMotorSpeed(int speed) {
		JointMotor2D motor = hingeJoint.motor;
		motor.motorSpeed = speed;
		hingeJoint.motor = motor;
		hingeJoint.useMotor = true;
	}
}
