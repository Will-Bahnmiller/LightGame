using UnityEngine;
using System.Collections;

public class PositionTracker : MonoBehaviour {

	public GameObject player, flashlight;
	public Vector3 playerPosition, mousePosition, mouseDirection;
	public float mouseDistance, angle;

	private GameController gc;


	void Start() {
		gc = Camera.main.GetComponent<GameController>();
		player = GameObject.FindGameObjectWithTag("Player");
	}


	void Update () {

		// Player location
		playerPosition = player.transform.position;

		// Mouse location in world coordinates
		if (!gc.controllerScheme) {
			mousePosition = new Vector3( Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
		                            	 Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
		}
		else {
			if (Mathf.Abs(Input.GetAxis("X Axis Right")) > .1f || Mathf.Abs(Input.GetAxis("Y Axis Right")) > .1f) {
				mousePosition = playerPosition + new Vector3(Input.GetAxis("X Axis Right"), -Input.GetAxis ("Y Axis Right"), 0f);
			}
			else {
				if (player.GetComponent<PlayerController>().facingRight == true) {
					mousePosition = playerPosition + Vector3.right * 2f;
				}
				else {
					mousePosition = playerPosition + Vector3.left * 2f;
				}
			}
		}

		// Distance between mouse position and player position
		mouseDistance = Vector3.Distance(playerPosition, mousePosition);

		// Unit vector that represents the direction from player to mouse
		mouseDirection = (mousePosition - playerPosition).normalized;

		// Determine angle of the direction
		angle = -Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
	}

} // end of PositionTracker.cs
