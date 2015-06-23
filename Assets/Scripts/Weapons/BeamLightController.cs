using UnityEngine;
using System.Collections;

public class BeamLightController : MonoBehaviour {
	
	public float maxDist, damage, power;

	private GameController gc;
	private PositionTracker positionTracker;
	private GameObject[] allBeams;
	private Vector3 playerPos, myDirection;
	private float myTime, pos;


	void Start() {

		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		allBeams = GameObject.FindGameObjectsWithTag("LightBeam");
		myTime = 1f;
	}


	void Update () {

		// Update information
		playerPos = positionTracker.playerPosition;
		myDirection = positionTracker.mouseDirection;
		Vector3 offset = new Vector3(pos * Mathf.Sin(positionTracker.angle * Mathf.Deg2Rad),
		                             pos * Mathf.Cos(positionTracker.angle * Mathf.Deg2Rad), 0f);

		// Update position based on current angle
		transform.position = myTime * myDirection + playerPos + offset;
		myTime += power * Time.deltaTime;

		// If missile travels a certain distance, kill it
		if (Vector3.Distance(transform.position, playerPos) > maxDist) {
			Destroy(transform.gameObject);
		}

		// If player lets go of mouse, kill it
		if (!gc.controllerScheme && (Input.GetKeyUp(KeyCode.Mouse0) || !Input.GetKey(KeyCode.Mouse0))) {
			Destroy(transform.gameObject);
		}
		if (gc.controllerScheme && Input.GetAxis("Right Trigger") >= -0.5f) {
			Destroy(transform.gameObject);
		}

	} // end of Update()


	void OnCollisionEnter(Collision coll) {

		Debug.Log ("Beam collided with " + coll.transform.name);

		// If enemy, damage
		if (gc.canTakeDamage(coll.gameObject.tag)) {
			coll.gameObject.SendMessage("TakeDamage", damage);
		}

		// Destroy this light and all that came before it
		for (int i = 0; i < allBeams.Length; i++) {
			if (allBeams[i] != null) {
				Destroy(allBeams[i].transform.gameObject);
			}
		}
		Destroy(transform.gameObject);

	}


	void SetPos(float p) {
		pos = p;
	}

} // end of BeamLightController.cs
