using UnityEngine;
using System.Collections;

public class ChargeLightController : MonoBehaviour {

	public GameObject particlePrefab;
	public float missileSpeed, maxDist, minDamage, maxDamage,
			     minSize, maxSize, chargeTime, delayToCharge,
				 minParticleRate, maxParticleRate;
	
	private GameController gc;
	private PositionTracker positionTracker;
	private Light myLight;
	private TrailRenderer tr;
	private Vector3 direction, playerPos;
	private bool isCharging;
	private float currentDamage, timeAlive, timeDelay, timer, chargeAngle, pRate;
	
	
	void Start() {
		
		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		myLight = transform.GetComponent<Light>();
		myLight.range = minSize;
		tr = transform.GetComponent<TrailRenderer>();
		playerPos = positionTracker.playerPosition;
		direction = positionTracker.mouseDirection;
		isCharging = true;
		timeAlive = 0f;  timeDelay = 0f;  timer = 0f;  chargeAngle = 0f;  pRate = maxParticleRate;
	}
	
	
	void Update () {

		// While holding fire button, charge up
		if (isCharging) {

			// Only begin charging after a small delay
			if (timeDelay > delayToCharge) {

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

					// Keep spawning charge particles while charging
					timer += Time.deltaTime;
					if (timer >= pRate) {
						timer = 0f;
						spawnParticle();
					}
					pRate = maxParticleRate - (maxParticleRate - minParticleRate) * (timeAlive / chargeTime);
				}

				// On release, stop charging
				else {
					isCharging = false;
					tr.enabled = true;
				}
			}
			else {
				timeDelay += Time.deltaTime;
				playerPos = positionTracker.playerPosition;
				direction = positionTracker.mouseDirection;
				transform.position = playerPos + ( 1.5f * direction );
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


	void spawnParticle() {

		Vector3 pos = transform.position;
		float scale = 1.2f;

		// Determine spawn location of particle
		pos += Quaternion.Euler(0f, 0f, chargeAngle) * Vector3.right * scale;

		// Update position angle
		chargeAngle = (chargeAngle + 112.5f + Random.Range(-10f, 10f)) % 360f;

		// Spawn the particle
		GameObject p;
		p = Instantiate(particlePrefab, pos, Quaternion.identity) as GameObject;
		p.transform.parent = transform;
	}

	
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
