using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

	private Rigidbody2D rigidbody;

	void Awake () {
		rigidbody = GetComponent<Rigidbody2D> ();
		rigidbody.centerOfMass = new Vector2 (0, 0);
	}
}
