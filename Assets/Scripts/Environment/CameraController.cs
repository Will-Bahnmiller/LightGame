using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public float cameraSpeed;
	public bool upDownDoorVisible, leftRightDoorVisible, panCamera;

	private PositionTracker positionTracker;
	private Vector3 target, playerPos, upPan, downPan, rightPan, leftPan;
	private float height, width;


	void Start() {

		// Initialize data
		panCamera = false;
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		upDownDoorVisible = false;  leftRightDoorVisible = false;
		height = 20f;  width = 36.5f;
	}


	void Update () {

		// Normally, camera follows the player
		if (!panCamera) {

			// Recompute the pan locations
			upPan = transform.position + Vector3.up * height;
			downPan = transform.position + Vector3.down * height;
			rightPan = transform.position + Vector3.right * width;
			leftPan = transform.position + Vector3.left * width;

			// Always center camera on player unless door in view
			playerPos = positionTracker.playerPosition;
			if (upDownDoorVisible) {	target.y = transform.position.y; }
			else { 						target.y = playerPos.y; }

			if (leftRightDoorVisible) { target.x = transform.position.x; }
			else { 						target.x = playerPos.x; }

			// Update camera position
			target.z = transform.position.z;
			transform.position = target;
		}

		// Player moved through a door
		else {

			PlayerController pc = positionTracker.player.GetComponent<PlayerController>();

			// Pan the camera to follow where the player went
			if (pc.doorUp) {
				transform.position = Vector3.Lerp(transform.position, upPan, Time.deltaTime * cameraSpeed);
			}
			if (pc.doorDown) {
				transform.position = Vector3.Lerp(transform.position, downPan, Time.deltaTime * cameraSpeed);
			}
			if (pc.doorRight) {
				transform.position = Vector3.Lerp(transform.position, rightPan, Time.deltaTime * cameraSpeed);
			}
			if (pc.doorLeft) {
				transform.position = Vector3.Lerp(transform.position, leftPan, Time.deltaTime * cameraSpeed);
			}

			// Once camera is done panning, reset flags
			if (Vector3.Distance(transform.position, upPan) < .1f
			    || Vector3.Distance(transform.position, downPan) < .1f
			    || Vector3.Distance(transform.position, rightPan) < .1f
			    || Vector3.Distance(transform.position, leftPan) < .1f) {
				panCamera = false;
				pc.doorUp = false;  pc.doorDown = false;
				pc.doorRight = false;  pc.doorLeft = false;
				pc.canMove = true;
				Camera.main.GetComponent<GameController>().cutscene = false;
			}
		}

	} // end of Update()

} // end of CameraController.cs
