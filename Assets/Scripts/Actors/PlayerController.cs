using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float jumpHeight, moveSpeed;
	public bool canMove, hasMouseLight, facingRight;
	public bool doorUp, doorDown, doorRight, doorLeft;

	private GameController gc;
	private PositionTracker positionTracker;
	private GameObject doorGoingThrough;
	private float inputSpeed, normalSpeed;
	private bool isJumping;
	private Vector3 temp, mousePos;


	void Start() {

		// Initialize data
		doorUp = false;  doorDown = false;  doorRight = false;  doorLeft = false;
		canMove = true;  isJumping = true;  facingRight = true;
		normalSpeed = moveSpeed;

		// Once player is loaded, enable tracking of player
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		positionTracker.enabled = true;
	}


	void Update () {

		// Keep player upright
		Quaternion upright = transform.rotation;
		upright.z = 0f;
		transform.rotation = upright;
		Vector3 upright2 = transform.position;
		upright2.z = 0f;
		transform.position = upright2;

		Vector3 temp = transform.position;

		// Allow player movement at the start
		if (Vector3.Distance(transform.position, Vector3.zero) < 2f) {
			canMove = true;
		}

		// Player movement
		if (canMove) {

			// Player movement
			if (!gc.controllerScheme) {
				if (Input.GetKey(KeyCode.A) && !Input.GetKeyDown(KeyCode.D)) { inputSpeed = -1f;  facingRight = true; }
				else if (Input.GetKey(KeyCode.D) && !Input.GetKeyDown(KeyCode.A)) { inputSpeed = 1f;  facingRight = false; }
				else { inputSpeed = 0f; }
			}
			else {
				if ( Mathf.Abs(Input.GetAxis("X Axis Left")) > .05f ) { inputSpeed = Input.GetAxis("X Axis Left"); }
				else { inputSpeed = 0f; }
				if (Input.GetAxis("X Axis Left") > 0f) { facingRight = true; }
				if (Input.GetAxis("X Axis Left") < 0f) { facingRight = false; }
			}
			temp = transform.position;
			temp.x += moveSpeed * inputSpeed * Time.deltaTime;
			transform.position = temp;

			// Crouch
			if (!gc.controllerScheme) {
				if (Input.GetKeyDown(KeyCode.LeftShift)) { moveSpeed *= 0.5f; }
				if (Input.GetKeyUp(KeyCode.LeftShift)) { moveSpeed *= 2f; }
			}
			else {
				if (Input.GetKeyDown(KeyCode.JoystickButton2)) {
					if (moveSpeed == normalSpeed) { moveSpeed *= 0.5f; }
					else 						  { moveSpeed *= 2f; }
				}
			}

			// Jump if on ground
			if (!gc.controllerScheme) {
				if ( Input.GetKeyDown(KeyCode.Space) && !isJumping ) {
					rigidbody.AddForce(new Vector2(0f, jumpHeight), ForceMode.Impulse);
				}
			}
			else {
				if ( Input.GetKeyDown(KeyCode.JoystickButton0) && !isJumping ) {
					rigidbody.AddForce(new Vector2(0f, jumpHeight), ForceMode.Impulse);
				}
			}
		
		} // end of if(canMove)

		// Nudge the player a little bit away from the door
		else {
			if (doorUp) {
				transform.position = Vector3.Lerp(transform.position,
				                                  new Vector3(transform.position.x,
				            						doorGoingThrough.GetComponent<DoorController>().pos1.y, 0f),
				                                  Time.deltaTime);
			}
			if (doorDown) {
				transform.position = Vector3.Lerp(transform.position,
				                                  new Vector3(transform.position.x,
				            						doorGoingThrough.GetComponent<DoorController>().pos2.y, 0f),
				                                  Time.deltaTime);
			}
			if (doorRight) {
				transform.position = Vector3.Lerp(transform.position,
				                                  new Vector3(doorGoingThrough.GetComponent<DoorController>().pos1.x,
				            						transform.position.y, 0f),
				                                  Time.deltaTime);
			}
			if (doorLeft) {
				transform.position = Vector3.Lerp(transform.position,
				                                  new Vector3(doorGoingThrough.GetComponent<DoorController>().pos2.x,
				            						transform.position.y, 0f),
				                                  Time.deltaTime);
			}
		}

		// Player went offscreen
		if (!transform.renderer.isVisible) {
			GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			CameraController cc = null;
			if (mainCamera != null) {
				cc = mainCamera.GetComponent<CameraController>();
			}
			
			// Stop the player from moving until camera is done panning
			canMove = false;
			
			// Player went through a door
			if ( cc != null && (doorUp || doorDown || doorRight || doorLeft) ) {
				cc.panCamera = true;
			}
		}

	} // end of Update() 


	void OnCollisionStay(Collision collisionInfo) {

		// Track when on the ground
		if (collisionInfo.gameObject.tag == "Floor") {
			isJumping = false;
		}
	}


	void OnCollisionEnter(Collision collisionInfo) {

		// Track which door player is about to go through
		if (collisionInfo.gameObject.tag == "UpDownDoor" ||
		    collisionInfo.gameObject.tag == "LeftRightDoor") {
			doorGoingThrough = collisionInfo.gameObject;
		}
	}


	void OnCollisionExit(Collision collisionInfo) {

		// Track when leaving the ground
		if (collisionInfo.gameObject.tag == "Floor") {
			isJumping = true;
		}
	}

} // end of PlayerController.cs
