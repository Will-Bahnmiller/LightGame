using UnityEngine;
using System.Collections;

public class FreezeLightCoreController : MonoBehaviour {
	
	public float missileSpeed, maxDist, damage, spinRate;

	private PositionTracker positionTracker;
	private Vector3 direction, playerPos;
	private float angle;


	void Start() {

		// Initialize data
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		playerPos = positionTracker.playerPosition;
		direction = positionTracker.mouseDirection;
		angle = 0f;
		
		// Provide initial position
		transform.position = playerPos;
	}


	void Update () {
		
		// Move light ball along given direction at given missile speed
		transform.position = transform.position + direction * missileSpeed;

		// Rotate light ball at spin rate speed
		transform.rotation = Quaternion.Euler (0f, 0f, angle);
		angle += spinRate;
		
		// If missile travels a certain distance, kill it
		if (Vector3.Distance(transform.position, playerPos) > maxDist) {
			Destroy(transform.gameObject);
		}
	}

	void OnCollisionEnter(Collision coll) {

		Debug.Log ("Freeze core collided wtih " + coll.transform.name);

		// If enemy, take damage and freeze/slow


		// Destroy this light
		Destroy(transform.gameObject);

	}

} // end of FreezeLightCoreController.cs
