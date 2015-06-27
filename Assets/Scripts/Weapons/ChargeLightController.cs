using UnityEngine;
using System.Collections;

public class ChargeLightController : MonoBehaviour {

	public float missileSpeed, maxDist, minDamage, maxDamage, minSize, maxSize, chargeTime, delayToCharge;
	
	private GameController gc;
	private PositionTracker positionTracker;
	private Light myLight;
	private TrailRenderer tr;
	private Vector3 direction, playerPos;
	private bool isCharging;
	private float currentDamage, timeAlive;
	
	
	void Start() {
		
		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		myLight = transform.GetComponent<Light>();
		tr = transform.GetComponent<TrailRenderer>();
		playerPos = positionTracker.playerPosition;
		direction = positionTracker.mouseDirection;
		isCharging = true;
		timeAlive = 0f;
	}
	
	
	void Update () {

		// While holding fire button, charge up
		if (isCharging) {
			if ( (!gc.controllerScheme && Input.GetKey(KeyCode.Mouse0)) ||
			      (gc.controllerScheme && Input.GetAxis("Right Trigger") < -0.5f)
			    && positionTracker.player.GetComponent<PlayerController>().canMove) {

				// Keep track of player and mouse positions
				playerPos = positionTracker.playerPosition;
				direction = positionTracker.mouseDirection;

				// Increase damage, size by linear interpolation
				currentDamage = minDamage + (maxDamage - minDamage) * (timeAlive / chargeTime);
				myLight.range = minSize + (maxSize - minSize) * (timeAlive / chargeTime);
				timeAlive = Mathf.Min (timeAlive + Time.deltaTime, chargeTime);

				// Rotate to where mouse is facing
				transform.position = playerPos + ( 1.5f * direction );
			}

			// On release, stop charging
			else {
				isCharging = false;
				tr.enabled = true;
			}
		}

		// No longer charging so start moving
		else {

			transform.Translate(direction * missileSpeed * Time.deltaTime);

			// If missile travels a certain distance, kill it
			if (Vector3.Distance(transform.position, playerPos) > maxDist) {
				Destroy(transform.gameObject);
			}
		}

		// Show visual que when fully charged
		if (myLight.range == maxSize && currentDamage == maxDamage) {
			myLight.color = new Color(1f, 1f, 210f/255f);
		}

	} // end of Update()
	
	
	void OnCollisionEnter(Collision coll) {
		
		Debug.Log("Basic collided with " + coll.transform.name);

		// Only consider collisions if not charging
		if (!isCharging) {

			// If enemy, damage
			if (gc.canTakeDamage(coll.gameObject.tag)) {
				coll.gameObject.SendMessage("TakeDamage", currentDamage);
			}
			
			// Destroy this light when colliding
			Destroy(transform.gameObject);
		}
	}


	void OnCollisionStay(Collision coll) {
		OnCollisionEnter(coll);
	}

} // end of ChargeLightController.cs
