using UnityEngine;
using System.Collections;

public class RetroPlayer : MonoBehaviour {

	[SerializeField] private KeyCode jumpLeft;
	[SerializeField] private KeyCode jumpRight;

	private Rigidbody2D rigidbody;
	private HingeJoint2D hingeJoint;
	private int specialPowerDuration = 5;

	void Awake () {
		rigidbody = GetComponent<Rigidbody2D> ();
		hingeJoint = GetComponent<HingeJoint2D> ();
	}

	void Update () {
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
		JointMotor2D motor = hingeJoint.motor;
		motor.motorSpeed = 3000;
		hingeJoint.motor = motor;
		Invoke("RemoveSpecialPowers", specialPowerDuration);
	}

	void RemoveSpecialPowers() {
		Debug.Log ("Removing SPECIAL POWERS!");
		transform.localScale = new Vector3 (1, 1, 1);
		JointMotor2D motor = hingeJoint.motor;
		motor.motorSpeed = 800;
		hingeJoint.motor = motor;
	}
}
