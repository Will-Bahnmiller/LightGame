using UnityEngine;
using System.Collections;

public class BeamLightCoreController : MonoBehaviour {

	public GameObject childPrefab;
	public float fireRate;

	private GameController gc;
	private PositionTracker positionTracker;
	private Vector3 direction, playerPos;
	private bool oddEven;
	private float timer;


	void Start() {

		// Initiallize data
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		oddEven = true;
		timer = 0f;
	}


	void Update () {

		// Update timer
		timer = Mathf.Min (timer + Time.deltaTime, fireRate);

		// Update position to be the player's position
		playerPos = positionTracker.playerPosition;
		transform.position = playerPos;

		// Continually fire the beam
		if (timer == fireRate) {
			GameObject b1, b2;
			if (oddEven) {
				b1 = Instantiate(childPrefab, playerPos, Quaternion.identity) as GameObject;
				b1.transform.parent = transform;
				b1.SendMessage("SetPos", 0f);
				oddEven = false;
			}
			else {
				b1 = Instantiate(childPrefab, playerPos, Quaternion.identity) as GameObject;
				b1.transform.parent = transform;
				b1.SendMessage("SetPos", -.2f);
				b2 = Instantiate(childPrefab, playerPos, Quaternion.identity) as GameObject;
				b2.transform.parent = transform;
				b2.SendMessage("SetPos", .2f);
				oddEven = true;
			}
			timer = 0f;
		}

		
		// If player lets go of mouse, kill it
		if (!gc.controllerScheme && Input.GetKeyUp(KeyCode.Mouse0)) {
			Destroy(transform.gameObject);
		}
		if (gc.controllerScheme && Input.GetAxis("Right Trigger") >= -0.5f) {
			Destroy(transform.gameObject);
		}
	}

} // end of BeamLightCoreController.cs

