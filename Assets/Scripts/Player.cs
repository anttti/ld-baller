using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	[SerializeField] private KeyCode changeRotation;
	[SerializeField] private KeyCode jump;
	[SerializeField] private LayerMask whatIsGround;
	[SerializeField] private GameObject fingers;
	[SerializeField] private GameObject hand;

	private HingeJoint2D hingeJoint;
	private Rigidbody2D rigidbody;
	private Rigidbody2D handRigidbody;

	private bool isGrounded;
	private Transform groundCheck;
	const float groundedRadius = .2f;

	private float motorSpeed = 300;
	private float motorDirection = 1;

	void Awake () {
		groundCheck = transform.Find ("GroundCheck");
	}

	void Start () {
		hingeJoint = GetComponent<HingeJoint2D> ();	
		rigidbody = GetComponent<Rigidbody2D> ();
		handRigidbody = hand.GetComponent<Rigidbody2D> ();
		/*
		JointMotor2D motor = hingeJoint.motor;
		motor.motorSpeed = motorSpeed;
		hingeJoint.motor = motor;
		*/
	}

	private void FixedUpdate() {
		isGrounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders [i].gameObject != gameObject) {
				isGrounded = true;
			}
		}
	}

	void Update () {
		/*
		JointMotor2D motor = hingeJoint.motor;
		if (!isGrounded) {
			motor.motorSpeed = 0;
		} else {
			motor.motorSpeed = motorSpeed * motorDirection;
		}
		hingeJoint.motor = motor;

		if (Input.GetKeyDown (changeRotation)) {
			motorDirection = -motorDirection;
		}
		*/

		if (Input.GetKeyDown (changeRotation)) {
			handRigidbody.AddForceAtPosition (new Vector2 (20f, 20f), new Vector2 (0, 0), ForceMode2D.Impulse);
		}
			
		if (Input.GetKeyDown (jump) && isGrounded) {
			// Jump to the direction fingers are pointing
			bool jumpRight = fingers.transform.position.x > transform.position.x;
			float jumpForce = Random.Range (20f, 40f);
			if (jumpRight) {
				rigidbody.AddForce (new Vector2 (10f, jumpForce), ForceMode2D.Impulse);
			} else {
				rigidbody.AddForce (new Vector2 (-10f, jumpForce), ForceMode2D.Impulse);
			}
		}
	}
}
