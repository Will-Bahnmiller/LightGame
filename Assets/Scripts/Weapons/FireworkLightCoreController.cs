using UnityEngine;
using System.Collections;

public class FireworkLightCoreController : MonoBehaviour {

	public GameObject tempPrefab;
	public GameObject childPrefab;
	public float missileSpeed, maxDist, radius;
	public int rings, minSparks, maxSparks;

	private PositionTracker pt;
	private Vector3 direction, playerPos;
	private float chargePercent;
	private bool isCharging, isFullyCharged, hasCollided, hasExploded;
	
	
	void Start() {

		// Initialize data
		pt = Camera.main.GetComponent<PositionTracker>();
		playerPos = pt.playerPosition;
		isCharging = true;  isFullyCharged = false;  hasCollided = false;
		hasExploded = false;
	}
	
	
	void Update () {

		// No longer charging so start moving
		if (!isCharging && !hasCollided && !hasExploded) {
			
			transform.Translate(direction * missileSpeed * Time.deltaTime);
			
			// If missile travels a certain distance, explode
			if (Vector3.Distance(transform.position, playerPos) > maxDist) {
				explode();
			}
		}
		
	} // end of Update()


	void CollisionDetected(Collision coll) {
		
		// Do stuff when colliding
		hasCollided = true;
		explode();
		if (isFullyCharged) {
			
		}
		
	}


	void explode() {

		// Set flag, disable light, start delay to kill, disable trail
		hasExploded = true;
		gameObject.GetComponent<Light>().enabled = false;
		gameObject.SendMessage("SetPersist", true);
		gameObject.GetComponent<TrailRenderer>().enabled = false;

		// Spawn sparks
		GameObject temp = Instantiate(tempPrefab, transform.position, Quaternion.identity) as GameObject;
		temp.SendMessage("SetDuration", childPrefab.GetComponent<FireworkLightController>().duration);
		int sparkCount = minSparks + Mathf.FloorToInt((maxSparks - minSparks) * chargePercent);
		for (int i = 1; i <= rings; i++) {
			for (int j = 0; j < sparkCount * i; j++) {
				Vector3 sparkPos = Quaternion.Euler(0f, 0f, j * 360f / (sparkCount * i)) * Vector3.up * i * radius;
				GameObject s = Instantiate(childPrefab, transform.position + sparkPos, Quaternion.identity) as GameObject;
				s.transform.parent = temp.transform;
				s.SendMessage("SetDirection", sparkPos);
				s.SendMessage("SetPower", chargePercent);
			}
		}
	
	} // end of explode()
	
	
	void PercentCharged(float p) {  chargePercent = p;  }
	void ChargingStatus(bool s) {  isCharging = s;  }
	void SetDirection(Vector3 d) {  direction = d;  }
	void SetFullyCharged(bool c) {  isFullyCharged = true;  }

} // end of FireworkLightCoreController.cs
