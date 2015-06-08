using UnityEngine;
using System.Collections;

public class FlameLightController : MonoBehaviour {
	
	public float maxDist, damage, spread, power;

	private PositionTracker positionTracker;
	private Vector3 playerPos, corePos, myDirection;
	private float angle;


	void Start() {

		// Initialize data
		corePos = GameObject.FindGameObjectWithTag("LightBullet FlameCore").transform.position;
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		playerPos = positionTracker.playerPosition;
		
		// Provide initial position and angle
		transform.position = corePos;
		angle = positionTracker.angle;
		angle = -Random.Range(angle - spread, angle + spread);

		// Toss flame at this angle with some force
		myDirection = new Vector3( Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad), 0f );
		transform.GetComponent<Rigidbody>().AddForce( myDirection * power, ForceMode.Impulse );
	}

	
	void Update () {

		// If missile travels a certain distance, kill it
		if (Vector3.Distance(transform.position, playerPos) > maxDist) {
			Destroy(transform.gameObject);
		}
	}


	void OnCollisionEnter(Collision coll) {

		Debug.Log ("Flame collided wtih " + coll.transform.name);

		// If enemy, take damage and apply burn


		// Destroy this light
		Destroy(transform.gameObject);

	}

} // end of FlameLightController.cs
