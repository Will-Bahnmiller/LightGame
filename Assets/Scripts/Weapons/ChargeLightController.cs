using UnityEngine;
using System.Collections;

public class ChargeLightController : MonoBehaviour {

	public float missileSpeed, maxDist;

	private PositionTracker positionTracker;
	private Vector3 direction, playerPos;
	private bool isCharging, isFullyCharged, hasCollided;
	
	
	void Start() {
		
		// Initialize data
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		playerPos = positionTracker.playerPosition;
		isCharging = true;  isFullyCharged = false;  hasCollided = false;
	}
	
	
	void Update () {

		// No longer charging so start moving
		if (!isCharging && !hasCollided) {

			transform.Translate(direction * missileSpeed * Time.deltaTime);

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

		}

	}
	

	void ChargingStatus(bool s) {  isCharging = s;  }
	void SetDirection(Vector3 d) {  direction = d;  }
	void SetFullyCharged(bool c) {  isFullyCharged = true;  }

} // end of ChargeLightController.cs
