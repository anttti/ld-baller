﻿using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour {
	Vector3 originalCameraPosition;

	float shakeAmt = 0;

	void OnCollisionEnter2D(Collision2D coll) {
		originalCameraPosition = Camera.main.transform.position;
		shakeAmt = coll.relativeVelocity.magnitude * .0025f;
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 0.3f);
	}

	void CameraShake() {
		if (shakeAmt > 0) {
			float quakeAmt = Random.value * shakeAmt * 2 - shakeAmt;

			Vector3 pp = Camera.main.transform.position;
			pp.y += quakeAmt;
			pp.x += quakeAmt;
			Camera.main.transform.position = pp;
		}
	}

	void StopShaking() {
		CancelInvoke("CameraShake");
		Camera.main.transform.position = originalCameraPosition;
	}
}