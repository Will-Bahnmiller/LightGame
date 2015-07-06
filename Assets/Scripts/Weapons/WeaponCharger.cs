using UnityEngine;
using System.Collections;

public class WeaponCharger : MonoBehaviour {

	public Component weaponScript;
	public GameObject particlePrefab;
	public Color fullyChargedColor;
	public float minDamage, maxDamage, minSize, maxSize, chargeTime, delayToCharge,
				 minParticleRate, maxParticleRate;
	public bool enableTrail;
	
	private GameController gc;
	private PositionTracker positionTracker;
	private Light myLight;
	private TrailRenderer tr;
	private Vector3 direction, playerPos;
	private bool isCharging;
	private float currentDamage, timeAlive, timeDelay, timer, chargeAngle, pRate;
	
	
	void Awake() {
		
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
				
			// Charge conditions
			if ( gc.shootWeapon && positionTracker.player.GetComponent<PlayerController>().canMove) {
					
				// Rotate to where mouse is facing
				playerPos = positionTracker.playerPosition;
				direction = positionTracker.mouseDirection;
				transform.position = playerPos + ( 1.5f * direction );

				// Only begin charging after a small delay
				if (timeDelay >= delayToCharge) {
					
					// Increase damage, size by linear interpolation
					currentDamage = minDamage + (maxDamage - minDamage) * (timeAlive / chargeTime);
					myLight.range = minSize + (maxSize - minSize) * (timeAlive / chargeTime);
					timeAlive = Mathf.Min (timeAlive + Time.deltaTime, chargeTime);
					
					// Keep spawning charge particles while charging
					timer += Time.deltaTime;
					if (timer >= pRate) {
						timer = 0f;
						spawnParticle();
					}
					pRate = maxParticleRate - (maxParticleRate - minParticleRate) * (timeAlive / chargeTime);
				}

				// Delay timer
				else {  timeDelay += Time.deltaTime;  }

			}

			// On release, stop charging
			else {
				isCharging = false;
				tr.enabled = enableTrail;
				gameObject.SendMessage("ChargingStatus", false);
				gameObject.SendMessage("SetDirection", direction);
				gameObject.GetComponent<SphereCollider>().enabled = true;
			}
			
		}
		
		// Show visual que when fully charged
		if (myLight.range == maxSize && currentDamage == maxDamage) {
			myLight.color = fullyChargedColor;
			gameObject.SendMessage("SetFullyCharged", true);
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
		
		Debug.Log(transform.name + " collided with " + coll.transform.name);
		
		// Only consider collisions if not charging
		if (!isCharging) {
			
			// If enemy, damage
			if (gc.canTakeDamage(coll.gameObject.tag)) {
				coll.gameObject.SendMessage("TakeDamage", currentDamage);
			}
			
			// Destroy this light when colliding
			gameObject.GetComponent<SphereCollider>().enabled = false;
			gameObject.GetComponent<Light>().enabled = false;
			gameObject.SendMessage("CollisionDetected", coll);
			gameObject.SendMessage("SetPersist", true);
		}
	}
	
	
	void OnCollisionStay(Collision coll) {
		OnCollisionEnter(coll);
	}

} // end of WeaponCharger.cs
