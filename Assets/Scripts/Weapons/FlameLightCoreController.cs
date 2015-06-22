using UnityEngine;
using System.Collections;

public class FlameLightCoreController : MonoBehaviour {

	public GameObject childPrefab;
	public float fireRate;

	private GameController gc;
	private PositionTracker positionTracker;
	private Vector3 direction, playerPos;
	private float timer;


	void Start() {
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		timer = 0f;
	}


	void Update () {

		// Update data
		playerPos = positionTracker.playerPosition;
		direction = positionTracker.mouseDirection;

		// Point core in mouse direction
		transform.position = playerPos + direction * 1.5f;

		// Continually create flame lights
		timer = Mathf.Min (fireRate, timer + Time.deltaTime);
		if (timer == fireRate) {
			Instantiate(childPrefab, transform.position, Quaternion.identity);
			timer = 0f;
		}

		// If player lets go of mouse, kill it
		if (!gc.controllerScheme && Input.GetKeyUp(KeyCode.Mouse0)) {
			Destroy(transform.gameObject);
		}
		if (gc.controllerScheme && Input.GetAxis("Right Trigger") >= -0.5f) {
			Destroy(transform.gameObject);
		}

	} // end of Update()

} // end of FlameLightCoreController.cs
