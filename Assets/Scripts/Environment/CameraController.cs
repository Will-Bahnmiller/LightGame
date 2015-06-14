using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public float cameraSpeed;
	public bool upDownDoorVisible, leftRightDoorVisible, panCamera;

	private GameObject closestUDdoor, closestLRdoor;
	private GameObject[] LRdoors, UDdoors;
	private PositionTracker positionTracker;
	private Vector3 target, playerPos, upPan, downPan, rightPan, leftPan;
	private float height, width;


	void Start() {

		// Initialize data
		panCamera = false;
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		upDownDoorVisible = false;  leftRightDoorVisible = false;
		height = 20f;  width = 36.5f;
		LRdoors = GameObject.FindGameObjectsWithTag("LeftRightDoor");
		UDdoors = GameObject.FindGameObjectsWithTag("UpDownDoor");
	}


	void Update () {

		// Keep track of the closest door
		FindClosestDoors();

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

			// Reset flags
			upDownDoorVisible = false;
			leftRightDoorVisible = false;

			// Pan the camera to follow where the player went
			PlayerController pc = positionTracker.player.GetComponent<PlayerController>();
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


	void FindClosestDoors() {

		GameObject temp1 = null, temp2 = null;

		// Search for closest left-right door
		foreach (GameObject d in LRdoors) {
			if (temp1 == null || Vector3.Distance(transform.position, temp1.transform.position) >
			    						Vector3.Distance(transform.position, d.transform.position)) {
				temp1 = d;
			}
		}

		// Search for closest up-down door
		foreach (GameObject d in UDdoors) {
			if (temp2 == null || Vector3.Distance(transform.position, temp2.transform.position) >
			    Vector3.Distance(transform.position, d.transform.position)) {
				temp2 = d;
			}
		}

		// Assign doors
		closestLRdoor = temp1;
		closestUDdoor = temp2;

	} // end of FindClosestDoors()


	public GameObject getClosestLRdoor() {
		return closestLRdoor;
	}
	public GameObject getClosestUPdoor() {
		return closestUDdoor;
	}

} // end of CameraController.cs
