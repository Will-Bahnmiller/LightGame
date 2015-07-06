using UnityEngine;
using System.Collections;

public class FreezeLightCoreController : MonoBehaviour {

	public GameObject particlePrefab, childPrefab;
	public float missileSpeed, maxDist, spinRate, particleRate;

	private PositionTracker positionTracker;
	private Vector3 direction, playerPos;
	private float angle, timer;
	private bool isCharging, isFullyCharged, hasCollided, hasSpawned;


	void Start() {

		// Initialize data
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		playerPos = positionTracker.playerPosition;
		direction = positionTracker.mouseDirection;
		angle = 0f;  timer = 0f;
		isCharging = true;  isFullyCharged = false;  hasCollided = false;  hasSpawned = false;
	}


	void Update () {

		// No longer charging so start moving
		if (!isCharging && !hasCollided) {

			// Spawn children if not already done so
			if (!hasSpawned) {
				hasSpawned = true;
				for (int i = 0; i < 8; i++) {
					Vector3 childPos = transform.position + Quaternion.Euler(0f, 0f, i * 45f) * Vector3.up * 0.7f;
					GameObject c = Instantiate(childPrefab, childPos, Quaternion.identity) as GameObject;
					c.transform.parent = transform;
				}
			}

			// Move light ball along given direction at given missile speed
			transform.position = transform.position + direction * missileSpeed * Time.deltaTime;

			// Rotate light ball at spin rate speed
			transform.rotation = Quaternion.Euler (0f, 0f, angle);
			angle = (angle + spinRate) % 360f;

			// Drop particles at a certain rate
			timer += Time.deltaTime;
			if (timer > particleRate) {
				timer = 0f;
				GameObject p;
				for (int i = 0; i < 3; i++) {
					p = Instantiate(particlePrefab, transform.position, Quaternion.identity) as GameObject;
					p.SendMessage("SetPos", i);
				}
			}
			
			// If missile travels a certain distance, kill it
			playerPos = positionTracker.playerPosition;
			if (Vector3.Distance(transform.position, playerPos) > maxDist) {
				Destroy(transform.gameObject);
			}
		}
	
	} // end of Update()


	void CollisionDetected(Collision coll) {
		
		// Do stuff when colliding
		hasCollided = true;
		if (isFullyCharged) {
			// apply freeze to coll.transform
		}
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
		
	}
	
	
	void ChargingStatus(bool s) {  isCharging = s;  }
	void SetDirection(Vector3 d) {  direction = d;  }
	void SetFullyCharged(bool c) {  isFullyCharged = true;  }

} // end of FreezeLightCoreController.cs
