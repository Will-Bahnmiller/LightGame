using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float jumpHeight, moveSpeed, warpSpeed;
	public bool canMove, hasMouseLight, facingRight;
	public bool doorUp, doorDown, doorRight, doorLeft;

	private GameController gc;
	private PositionTracker positionTracker;
	private GameObject doorGoingThrough;
	private float inputSpeed, mySpeed;
	private bool isJumping, isWarping;
	private Vector3 temp, mousePos, warpPos;


	void Start() {

		// Initialize data
		doorUp = false;  doorDown = false;  doorRight = false;  doorLeft = false;
		canMove = true;  isJumping = true;  isWarping = false;  facingRight = true;
		mySpeed = moveSpeed;

		// Once player is loaded, enable tracking of player
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		positionTracker.enabled = true;
	}


	void Update () {

		// Allow player movement at the start
		if (Vector3.Distance(transform.position, Vector3.zero) < 2f) {
			canMove = true;
		}

		// Check for movement
		if (!isWarping && !(doorUp || doorDown || doorLeft || doorRight)) {
			canMove = true;
		}

		// Player movement
		if (canMove) {

			// Player movement
			if (!gc.controllerScheme) {
				if      ( gc.moveLeft )  { inputSpeed = -1f;  facingRight = true; }
				else if ( gc.moveRight ) { inputSpeed = 1f;  facingRight = false; }
				else                     { inputSpeed = 0f; }
			}
			else {
				if ( Mathf.Abs(gc.leftAnalogX) > 0.5f ) { inputSpeed = gc.leftAnalogX; }
				else                                    { inputSpeed = 0f; }
				if (gc.leftAnalogX > 0.5f)  { facingRight = true; }
				if (gc.leftAnalogX < -0.5f) { facingRight = false; }
			}
			transform.Translate(Vector3.right * inputSpeed * mySpeed * Time.deltaTime);

			// Crouch
			if ( gc.crouch ) { mySpeed = moveSpeed / 2f; }
			else 			 { mySpeed = moveSpeed; }


			// Jump if on ground
			if ( gc.jump && !isJumping ) {
				rigidbody.AddForce(new Vector2(0f, jumpHeight), ForceMode.Impulse);
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

		// Warping
		if (isWarping) { Warp(); }

	} // end of Update() 


	void BeginWarp(Vector3 pos) {

		// Set flags to begin warping
		if (!isWarping) {
			rigidbody.detectCollisions = true;
			rigidbody.useGravity = false;
			isWarping = true;
			canMove = false;
			warpPos = pos;
			gameObject.layer = LayerMask.NameToLayer("Light Form");
			gameObject.GetComponent<MeshRenderer>().enabled = false;
			gameObject.GetComponent<Light>().enabled = true;
		}
	}


	void EndWarp() {

		// Set flags to stop warping
		if (isWarping) {
			rigidbody.useGravity = true;
			isWarping = false;
			canMove = true;
			gameObject.layer = LayerMask.NameToLayer("Player");
			gameObject.GetComponent<MeshRenderer>().enabled = true;
			gameObject.GetComponent<Light>().enabled = false;
		}
	}


	void Warp() {
		// Warp movement
		if ( Vector3.Distance(transform.position, warpPos) > 0.5f ) {
			transform.Translate( (warpPos - transform.position).normalized  * warpSpeed * Time.deltaTime);
		}

		// Reached warp position
		else { EndWarp(); }
	}


	void OnCollisionStay(Collision collisionInfo) {

		// Track when on the ground
		if (collisionInfo.gameObject.tag == "Floor") {
			isJumping = false;
		}

		// Cancel warping on collision
		if (isWarping) { EndWarp(); }
	}


	void OnCollisionEnter(Collision collisionInfo) {

		// Track which door player is about to go through
		//Debug.Log ("player collided with " + collisionInfo.transform.name);
		if (collisionInfo.gameObject.tag == "UpDownDoor" ||
		    collisionInfo.gameObject.tag == "LeftRightDoor") {
			doorGoingThrough = collisionInfo.gameObject;
		}

		// Cancel warping on collision
		if (isWarping) { EndWarp();  }
	}


	void OnCollisionExit(Collision collisionInfo) {

		// Track when leaving the ground
		if (collisionInfo.gameObject.tag == "Floor") {
			isJumping = true;
		}
	}

} // end of PlayerController.cs
