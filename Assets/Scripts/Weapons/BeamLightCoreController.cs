using UnityEngine;
using System.Collections;

public class BeamLightCoreController : MonoBehaviour {

	public GameObject childPrefab;
	public float fireRate;

	private GameController gc;
	private PositionTracker positionTracker;
	private Vector3 direction, playerPos;


	void Start() {
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
	}


	void FixedUpdate () {

		// Update position to be the player's position
		playerPos = positionTracker.playerPosition;
		transform.position = playerPos;

		// Continually fire the beam
		GameObject b;
		b = Instantiate(childPrefab, playerPos, Quaternion.identity) as GameObject;
		b.transform.parent = transform;
		
		// If player lets go of mouse, kill it
		if (!gc.controllerScheme && Input.GetKeyUp(KeyCode.Mouse0)) {
			Destroy(transform.gameObject);
		}
		if (gc.controllerScheme && Input.GetAxis("Right Trigger") >= -0.5f) {
			Destroy(transform.gameObject);
		}
	}

} // end of BeamLightCoreController.cs

