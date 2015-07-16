using UnityEngine;
using System.Collections;

public class WaveLightCoreController : MonoBehaviour {

	public GameObject tempPrefab;
	public GameObject childPrefab;
	public int minSparks, maxSparks;

	private float chargePercent;
	private bool isCharging, isFullyCharged;
	
	
	void Start() {

		// Initialize data
		isCharging = true;  isFullyCharged = false;
	}
	
	
	void Update () {

		// No longer charging, so explode
		if (!isCharging) { explode(); }

		// Do something when fully charged
		if (isFullyCharged) { }
		
	} // end of Update()
	


	void explode() {

		// Disable light, start delay to kill
		gameObject.GetComponent<Light>().enabled = false;
		gameObject.SendMessage("SetPersist", true);
		rigidbody.detectCollisions = false;

		// Spawn sparks
		GameObject temp = Instantiate(tempPrefab, transform.position, Quaternion.identity) as GameObject;
		temp.SendMessage("SetDuration", childPrefab.GetComponent<WaveLightController>().duration);
		int sparkCount = minSparks + Mathf.FloorToInt((maxSparks - minSparks) * chargePercent);
		for (int i = 0; i < sparkCount; i++) {
			Vector3 sparkPos = Quaternion.Euler(0f, 0f, i * (360f / sparkCount)) * Vector3.up * 0.1f;
			GameObject s = Instantiate(childPrefab, transform.position + sparkPos, Quaternion.identity) as GameObject;
			s.transform.parent = temp.transform;
			s.SendMessage("SetDirection", sparkPos);
			s.SendMessage("SetPower", chargePercent);
		}

		Destroy(gameObject);
	
	} // end of explode()


	void CollisionDetected(Collision coll) {
		// Do stuff when colliding	
	}

	
	void PercentCharged(float p) {  chargePercent = p;  Debug.Log ("charge: " + chargePercent); }
	void ChargingStatus(bool s) {  isCharging = s;  }
	void SetDirection(Vector3 d) {  } 
	void SetFullyCharged(bool c) {  isFullyCharged = true;  }

} // end of FireworkLightCoreController.cs
