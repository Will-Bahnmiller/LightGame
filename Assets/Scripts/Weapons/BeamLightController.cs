using UnityEngine;
using System.Collections;

public class BeamLightController : MonoBehaviour {
	
	public float maxDist, damage, power;

	private GameController gc;
	private PositionTracker positionTracker;
	private GameObject[] allBeams;
	private Vector3 playerPos, myDirection;
	private float myTime;


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

		// Update position based on current angle
		transform.position = myTime * myDirection + playerPos;
		myTime += power * Time.deltaTime;

		// If missile travels a certain distance, kill it
		if (Vector3.Distance(transform.position, playerPos) > maxDist) {
			Destroy(transform.gameObject);
		}

		// If player lets go of mouse, kill it
		if (!gc.controllerScheme && Input.GetKeyUp(KeyCode.Mouse0)) {
			Destroy(transform.gameObject);
		}
		if (gc.controllerScheme && Input.GetAxis("Right Trigger") >= -0.5f) {
			Destroy(transform.gameObject);
		}

	} // end of Update()


	void OnCollisionEnter(Collision coll) {

		Debug.Log ("Beam collided wtih " + coll.transform.name);

		// If enemy, take damage


		// Destroy this light and all that came before it
		for (int i = 0; i < allBeams.Length; i++) {
			Destroy(allBeams[i].transform.gameObject);
		}
		Destroy(transform.gameObject);

	}

} // end of BeamLightController.cs
