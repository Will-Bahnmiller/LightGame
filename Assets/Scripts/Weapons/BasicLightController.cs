﻿using UnityEngine;
using System.Collections;

public class BasicLightController : MonoBehaviour {
	
	public float missileSpeed, maxDist, damage;

	private PositionTracker positionTracker;
	private Vector3 direction, playerPos;


	void Start() {

		// Initialize data
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		playerPos = positionTracker.playerPosition;
		direction = positionTracker.mouseDirection;
	}


	void Update () {

		// Move light ball along given direction at given missile speed
		transform.Translate(direction * missileSpeed * Time.deltaTime);

		// If missile travels a certain distance, kill it
		if (Vector3.Distance(transform.position, playerPos) > maxDist) {
			Destroy(transform.gameObject);
		}
	}


	void OnCollisionEnter(Collision coll) {

		Debug.Log("Basic collided with " + coll.transform.name);

		// If enemy, damage


		// Destroy this light when colliding
		Destroy(transform.gameObject);
	}
	
} // end of BasicLightController.cs