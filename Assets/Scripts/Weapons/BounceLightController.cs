using UnityEngine;
using System.Collections;

public class BounceLightController : MonoBehaviour {

	public GameObject bouncePrefab;
	public int multiplyNumber, bounceLimit;
	public float power, spread, disperseRate, damage, maxDist, maxDuration;
	public bool isFirst;

	private Light myLight;
	private GameObject child;
	private GameController gc;
	private PositionTracker pt;
	private Vector3 playerPos, myDirection, prevPos, inDirection;
	private float timeAlive;


	void Start () {

		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		pt = Camera.main.GetComponent<PositionTracker>();
		myLight = transform.GetComponent<Light>();
		playerPos = pt.playerPosition;
		prevPos = transform.position;
		timeAlive = 0f;
		
		// Toss light at this angle with some force
		if (isFirst) {  myDirection = pt.mouseDirection;  }
		transform.GetComponent<Rigidbody>().AddForce( myDirection * power, ForceMode.Impulse );
	}


	void Update() {

		// If missile travels a certain distance or is alive for some time, kill it
		if (Vector3.Distance(transform.position, playerPos) > maxDist || timeAlive > maxDuration) {
			Destroy(transform.gameObject);
		}

		// Keep track of where you just were
		if (prevPos != transform.position) {
			inDirection = transform.position - prevPos;
		}
		prevPos = transform.position;
		timeAlive += Time.deltaTime;
	}
	

	void OnCollisionEnter(Collision coll) {
		
		Debug.Log ("Bounce collided with " + coll.transform.name);

		// Only consider stuff after it's been alive for some time
		if (timeAlive > .05f) {

			// If enemy, damage
			if (gc.canTakeDamage(coll.gameObject.tag)) {
				coll.gameObject.SendMessage("TakeDamage", damage);
			}

			// Spawn new bounce lights at this location
			if (bounceLimit > 1) {

				// Calculate bounce vector
				Vector3 v = Vector3.Reflect(inDirection, coll.contacts[0].normal);
				v.z = 0f;
				v.Normalize();

				// Create child
				for (int i = 0; i < multiplyNumber; i++) {

					// Determine spread angle
					float tempAngle;
					if (spread%2 == 1) {  tempAngle = spread * (i - 1) / 2;  } // odd spread
					else 			   {  tempAngle = spread * (i - 1/2);  }   // even spread
					v = Quaternion.Euler(0f, 0f, tempAngle) * v;

					// Spawn child and set variables
					child = Instantiate(bouncePrefab, prevPos, Quaternion.identity) as GameObject;
					child.SendMessage("SetBounceLimit", bounceLimit-1);
					child.SendMessage("SetDirection", v);
					child.SendMessage("SetRelation", false);
					child.SendMessage("SetLight", myLight.range / disperseRate);
					child.SendMessage("SetDamage", damage / disperseRate);
				}
			}
			
			// Destroy this light
			Destroy(transform.gameObject);
		}

	} // end of OnCollisionEnter()


	void SetBounceLimit(int b) {  bounceLimit = b;  }
	void SetDirection(Vector3 d) {  myDirection = d;  }
	void SetRelation(bool f) {  isFirst = f;  }
	void SetLight(float r) {  transform.GetComponent<Light>().range = r;  }
	void SetDamage(float d) {  damage = d;  }

} // end of BounceLightController.cs
