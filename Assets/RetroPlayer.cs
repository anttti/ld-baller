using UnityEngine;
using System.Collections;

public class RetroPlayer : MonoBehaviour {

	[SerializeField] private KeyCode jumpLeft;
	[SerializeField] private KeyCode jumpRight;

	private Rigidbody2D rigidbody;
	private HingeJoint2D hingeJoint;
	private int specialPowerDuration = 5;
	private bool isMovementDisabled = false;

	void Awake () {
		rigidbody = GetComponent<Rigidbody2D> ();
		hingeJoint = GetComponent<HingeJoint2D> ();
	}

	void Update () {
		if (isMovementDisabled) {
			return;
		}
		if (Input.GetKeyDown (jumpLeft)) {
			rigidbody.AddForce (new Vector2 (-100f, 100f), ForceMode2D.Impulse);
		} else if (Input.GetKeyDown (jumpRight)) {
			rigidbody.AddForce (new Vector2 (100f, 100f), ForceMode2D.Impulse);
		}
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
		SetMotorSpeed (3000);
		Invoke("RemoveSpecialPowers", specialPowerDuration);
	}

	void RemoveSpecialPowers() {
		if (isMovementDisabled) {
			return;
		}
		transform.localScale = new Vector3 (1, 1, 1);
		SetMotorSpeed (800);
	}

	void EnableMovement() {
		SetMotorSpeed (800);
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
