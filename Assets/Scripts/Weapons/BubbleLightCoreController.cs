using UnityEngine;
using System.Collections;

public class BubbleLightCoreController : MonoBehaviour {
	
	public GameObject childPrefab;
	public float fireRate, duration, power, damage, minIntensity, maxIntensity, minSpread, maxSpread;

	private GameController gc;
	private PositionTracker positionTracker;
	private Light myLight;
	private GameObject child;
	private Vector3 myDirection;
	private float timer, timeAlive;
	private bool hasCollided, stopBubbles;
	
	
	void Start() {

		// Initiallize data
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		myLight = transform.GetComponent<Light>();
		myDirection = positionTracker.mouseDirection;
		timer = 0f;  timeAlive = 0f;
		hasCollided = false;  stopBubbles = false;

		// Move light depth
		transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

		// Launch in mouse direction
		transform.GetComponent<Rigidbody>().AddForce( myDirection * power, ForceMode.Impulse );
	}
	
	
	void Update () {
		
		// Continually create bubble lights after hitting something
		if (hasCollided && !stopBubbles) {

			// Update timers
			timer = Mathf.Min (fireRate, timer + Time.deltaTime);
			timeAlive = Mathf.Min (duration, timeAlive + Time.deltaTime);

			// Fire a bubble at fixed rate
			if (timer == fireRate) {

				// Create bubble
				child = Instantiate(childPrefab, transform.position, Quaternion.identity) as GameObject;
				child.transform.parent = transform;
				child.SendMessage( "SetSpread", minSpread + (maxSpread - minSpread) * timeAlive / duration );
				timer = 0f;

				// Decrease intensity
				myLight.intensity = maxIntensity - (maxIntensity - minIntensity) * timeAlive / duration;
			}
		}

		// Stop producing bubbles after the duration
		if (timeAlive == duration) {

			stopBubbles = true;

			// Once no more bubbles exist, kill core
			if (GameObject.FindGameObjectsWithTag("LightBullet Bubble").Length == 0) {
				Destroy(gameObject);
			}
		}
		
	} // end of Update()


	void OnCollisionEnter(Collision coll) {
		
		Debug.Log ("Bubble core collided with " + coll.transform.name);

		// If enemy, damage
		if (gc.canTakeDamage(coll.gameObject.tag)) {
			coll.gameObject.SendMessage("TakeDamage", damage);
		}

		// When hitting the environment, set flag and hold position
		if ( gc.isEnvironment(coll.transform.tag) && coll.contacts[0].normal.y >= 0.9f ) {
			hasCollided = true;
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
		}
	}
	
} // end of BubbleLightCoreController.cs
