using UnityEngine;
using System.Collections;

public class ReboundLightController : MonoBehaviour {

	public float damage, range, speed;

	private GameController gc;
	private PositionTracker pt;
	private Vector3 playerPos, myDirection;
	private float mySpeed, dist;
	private bool movingAway;


	void Start () {
		gc = Camera.main.GetComponent<GameController>();
		pt = Camera.main.GetComponent<PositionTracker>();
		playerPos = pt.playerPosition;
		myDirection = pt.mouseDirection;
		movingAway = true;
	}
	

	void Update () {
	
		// Keep track of distance to player
		dist = Vector3.Distance(playerPos, transform.position);

		// Speed is dependent on distance from player
		mySpeed = speed + (speed)*(range-dist)/(range);

		// Move light away from where player was when intitially fired
		if (movingAway) {
			
			// Change direction after reaching range
			if (range - dist < 0.5f) {
				movingAway = false;
				range = dist;
			}
		}

		// After some distance or collision, head back to the player
		else {

			// Constantly change direction towards player
			playerPos = pt.playerPosition;
			myDirection = playerPos - transform.position;
			myDirection.z = 0f;
			myDirection.Normalize();

			// Kill if returned to player successfully
			if (dist < 1f) {
				Destroy(gameObject);
			}
		}

		// Move this light
		transform.Translate(myDirection * mySpeed * Time.deltaTime);

	} // end of Update()


	void OnCollisionEnter(Collision coll) {

		Debug.Log ("Rebound collided wtih " + coll.transform.name);

		// If enemy, damage
		if (gc.canTakeDamage(coll.gameObject.tag)) {
			coll.gameObject.SendMessage("TakeDamage", damage);
		}

		// If it was moving away, start heading back
		if (movingAway) {
			movingAway = false;
			range = dist;
		}

		// Otherwise something is blocking it from returning
		else {
			Destroy(gameObject);
		}
	}

} // end of ReboundLightController.cs
