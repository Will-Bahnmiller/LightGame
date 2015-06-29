using UnityEngine;
using System.Collections;

public class FreezeLightCoreController : MonoBehaviour {

	public GameObject particlePrefab;
	public float missileSpeed, maxDist, damage, spinRate, particleRate;

	private GameController gc;
	private PositionTracker positionTracker;
	private Vector3 direction, playerPos;
	private float angle, timer;


	void Start() {

		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		playerPos = positionTracker.playerPosition;
		direction = positionTracker.mouseDirection;
		angle = 0f;  timer = 0f;
		
		// Provide initial position
		transform.position = playerPos;
	}


	void Update () {
		
		// Move light ball along given direction at given missile speed
		transform.position = transform.position + direction * missileSpeed * Time.deltaTime;

		// Rotate light ball at spin rate speed
		transform.rotation = Quaternion.Euler (0f, 0f, angle);
		angle = (angle + spinRate) % 360f;

		// Drop particles at a certain rate
		timer += Time.deltaTime;
		if (timer > particleRate) {
			timer = 0f;
			GameObject p;
			for (int i = 0; i < 3; i++) {
				p = Instantiate(particlePrefab, transform.position, Quaternion.identity) as GameObject;
				p.SendMessage("SetPos", i);
			}
		}
		
		// If missile travels a certain distance, kill it
		playerPos = positionTracker.playerPosition;
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
