using UnityEngine;
using System.Collections;

public class FreezeLightCoreController : MonoBehaviour {
	
	public float missileSpeed, maxDist, damage, spinRate;

	private GameController gc;
	private PositionTracker positionTracker;
	private Vector3 direction, playerPos;
	private float angle;


	void Start() {

		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		playerPos = positionTracker.playerPosition;
		direction = positionTracker.mouseDirection;
		angle = 0f;
		
		// Provide initial position
		transform.position = playerPos;
	}


	void Update () {
		
		// Move light ball along given direction at given missile speed
		transform.position = transform.position + direction * missileSpeed * Time.deltaTime;

		// Rotate light ball at spin rate speed
		transform.rotation = Quaternion.Euler (0f, 0f, angle);
		angle += spinRate;
		
		// If missile travels a certain distance, kill it
		if (Vector3.Distance(transform.position, playerPos) > maxDist) {
			Destroy(transform.gameObject);
		}
	}

	void OnCollisionEnter(Collision coll) {

		Debug.Log ("Freeze core collided with " + coll.transform.name);

		// If enemy, damage and apply slow
		if (gc.canTakeDamage(coll.gameObject.tag)) {
			coll.gameObject.SendMessage("TakeDamage", damage);
			//coll.gameObject.SendMessage("ApplySlow");
		}

		// Destroy this light
		Destroy(transform.gameObject);

	}

} // end of FreezeLightCoreController.cs
