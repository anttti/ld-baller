﻿using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	
	private GameObject gameManager;

	void OnCollisionEnter2D(Collision2D other) {
		gameManager.SendMessage ("PlayHitSound");
	}

	void SetGameManager(GameObject mgr) {
		gameManager = mgr;
	}
}
