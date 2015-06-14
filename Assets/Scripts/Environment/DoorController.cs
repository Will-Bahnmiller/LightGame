using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	public GameObject doorLight, lampToOpen;
	public Vector3 pos1, pos2;
	public bool isLocked;

	private CameraController cameraController;
	private PositionTracker positionTracker;
	private GameObject myLight1, myLight2, player;
	private Vector3 myScale, playerPos;
	private bool isOpen;


	void Start() {

		// Initialize data
		isOpen = false;
		myScale = transform.localScale;

		// Obtain reference to camera and player
		cameraController = Camera.main.GetComponent<CameraController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();

		// Spawn door lights on either side of door
		if (gameObject.tag == "UpDownDoor") {
			myLight1 = Instantiate(doorLight, new Vector3(transform.position.x, transform.position.y + 1f, 0.7f),
			                       Quaternion.identity) as GameObject;
			myLight2 = Instantiate(doorLight, new Vector3(transform.position.x, transform.position.y - 1f, 0.7f),
			                       Quaternion.identity) as GameObject;
			pos1 = transform.position + Vector3.up * 2.5f;
			pos2 = transform.position + Vector3.down * 2.5f;
		}
		else {
			myLight1 = Instantiate(doorLight, new Vector3(transform.position.x + 1f, transform.position.y, 0.7f),
			                       Quaternion.identity) as GameObject;
			myLight2 = Instantiate(doorLight, new Vector3(transform.position.x - 1f, transform.position.y, 0.7f),
			                       Quaternion.identity) as GameObject;
			pos1 = transform.position + Vector3.right * 2.5f;
			pos2 = transform.position + Vector3.left * 2.5f;
		}
		myLight1.transform.parent = transform;
		myLight2.transform.parent = transform;
	}


	void Update () {

		// Determine if locked or not
		if (lampToOpen != null) {
			if (lampToOpen.GetComponent<LampController>().isFullyLit()) {
				isLocked = false;
			}
			else {
				isLocked = true;
			}
		}
		else  {
			isLocked = false;
		}

		// Keep track of player position
		playerPos = positionTracker.playerPosition;
	
		// If this door is visible to the player, limit camera movement
		if ( renderer.isVisible && isClosestDoor() ) {
			if (gameObject.tag == "UpDownDoor") {
				if (Mathf.Abs(transform.position.y - playerPos.y) <= 9f) {
					cameraController.upDownDoorVisible = true;
				}
				else {
					cameraController.upDownDoorVisible = false;
				}
			}
			if (gameObject.tag == "LeftRightDoor") {
				if (Mathf.Abs(transform.position.x - playerPos.x) <= 18.2f) {
					cameraController.leftRightDoorVisible = true;
				}
				else {
					cameraController.leftRightDoorVisible = false;
				}
			}
		}
		else {
			if (isClosestDoor()) {
				if (gameObject.tag == "UpDownDoor") {
					cameraController.upDownDoorVisible = false;
				}
				if (gameObject.tag == "LeftRightDoor") {
					cameraController.leftRightDoorVisible = false;
				}
			}
		}
		
		// Change door light color if door is locked
		if (isLocked) {
			myLight1.GetComponent<Light>().color = Color.red;
			myLight2.GetComponent<Light>().color = Color.red;
		}
		else {
			myLight1.GetComponent<Light>().color = Color.green;
			myLight2.GetComponent<Light>().color = Color.green;
		}

		// Once player is through the door, return to closed state
		//Debug.Log (Vector3.Distance(transform.position, playerPos));
		if (isOpen && Vector3.Distance(transform.position, playerPos) > 4.35f) {
			//Debug.Log ("Closing door with distance: "+Vector3.Distance(transform.position, playerPos));
			transform.localScale = myScale;
			isOpen = false;
		}

	} // end of Update()


	bool isClosestDoor() {
		if (transform.tag == "UpDownDoor" && cameraController.getClosestUPdoor() == gameObject) {
			return true;
		}
		if (transform.tag == "LeftRightDoor" && cameraController.getClosestLRdoor() == gameObject) {
			return true;
		}
		return false;
	}


	void OnCollisionEnter(Collision collisionInfo) {
	
		PlayerController pc = positionTracker.player.GetComponent<PlayerController>();

		// Player goes through door
		if (collisionInfo.gameObject.tag == "Player" && !isLocked) {

			// Player wants to go down through a door
			if (gameObject.tag == "UpDownDoor" && playerPos.y > transform.position.y) {
				pc.doorDown = true;
			}

			// Player wants to go up through a door
			else if (gameObject.tag == "UpDownDoor" && playerPos.y < transform.position.y) {
				pc.doorUp = true;
			}

			// Player wants to go left through a door
			else if (gameObject.tag == "LeftRightDoor" && playerPos.x > transform.position.x) {
				pc.doorLeft = true;
			}

			// Player wants to go right through a door
			else {
				pc.doorRight = true;
			}

			// Pause all objects
			// Camera.main.GetComponent<GameController>().cutscene = true;

			// Open door
			//Debug.Log ("opening door");
			transform.localScale = new Vector3(1f, 1f, 2f);
			isOpen = true;
		}
			
	} // end of OnCollisionEnter()

} // end of DoorController.cs
