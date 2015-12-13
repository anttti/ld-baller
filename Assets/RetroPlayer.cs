using UnityEngine;
using System.Collections;

public class RetroPlayer : MonoBehaviour {

	[SerializeField] private KeyCode jumpLeft;
	[SerializeField] private KeyCode jumpRight;
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

		if (!isCharging && Input.GetKeyDown (jumpLeft)) {
			currentJumpForce = 0;
			isCharging = true;
		}
		if (isCharging) {
			float delta = Time.deltaTime * JUMP_FORCE_CHARGE_SPEED;
			currentJumpForce = Mathf.Clamp (currentJumpForce + delta, currentJumpForce, MAX_JUMP_FORCE);

			if (playerNumber == 1) {
				gameManager.SendMessage ("UpdatePlayer1Charge", currentJumpForce);
			} else {
				gameManager.SendMessage ("UpdatePlayer2Charge", currentJumpForce);
			}
		}
		if (isCharging && Input.GetKeyUp (jumpLeft)) {
			rigidbody.AddForce (new Vector2 (0, currentJumpForce * 12), ForceMode2D.Impulse);
			isCharging = false;
		}

		if (Input.GetKeyDown (jumpRight)) {
			handRigidbody.AddForceAtPosition (new Vector2 (20f, 20f), new Vector2 (0, 0), ForceMode2D.Impulse);
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
		if (other.tag == "aBonus") {
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
		Invoke("RemoveSpecialPowers", specialPowerDuration);
	}

	void SpecialPowerShrink() {
		transform.localScale = new Vector3 (0.5f, 0.5f, 1);
		Invoke("RemoveSpecialPowers", specialPowerDuration);
	}

	void SpecialPowerFat() {
		transform.localScale = new Vector3 (2.5f, 1, 1);
		Invoke("RemoveSpecialPowers", specialPowerDuration);
	}

	void SpecialPowerMayhem() {
		//SetMotorSpeed (3000);
		Invoke("RemoveSpecialPowers", specialPowerDuration);
	}

	void RemoveSpecialPowers() {
		if (isMovementDisabled) {
			return;
		}
		transform.localScale = new Vector3 (1, 1, 1);
		//SetMotorSpeed (800);
	}

	void EnableMovement() {
		currentJumpForce = 0;
		//SetMotorSpeed (800);
	}

	void DisableMovement() {
		isMovementDisabled = true;
		SetMotorSpeed (0);
	}

	void SetMotorSpeed(int speed) {
		JointMotor2D motor = hingeJoint.motor;
		motor.motorSpeed = speed;
		hingeJoint.motor = motor;
	}
}
