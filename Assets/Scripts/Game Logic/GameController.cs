using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public bool cutscene, controllerScheme;
	public bool moveLeft, moveRight, crouch, jump;
	public bool weaponSelect, shootWeapon, flashlight;
	public float leftAnalogX, leftAnalogY, rightAnalogX, rightAnalogY, triggerRight;
	public int currentWeapon;



	void Start () {

		// Initialize data
		cutscene = false;
		controllerScheme = false;
	}
	

	void Update () {

		// Switch between controller and keyboard/mouse controls
		if (Input.GetKeyDown(KeyCode.Return)) {
			controllerScheme = !controllerScheme;
		}

		// Check for keyboard input
		if (!controllerScheme) {

			// Move left
			if ( Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) ) { moveLeft = true; }
			else 					     							   { moveLeft = false; }

			// Move right
			if ( Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) ) { moveRight = true; }
			else 					     							   { moveRight = false; }

			// Crouch
			if ( Input.GetKeyDown(KeyCode.LeftShift) ) { crouch = true; }
			if ( Input.GetKeyUp(KeyCode.LeftShift) )   { crouch = false; }

			// Jump
			if ( Input.GetKeyDown(KeyCode.Space) ) { jump = true; }
			else 								   { jump = false; }

			// Weapon select
			if ( Input.GetKeyDown(KeyCode.E) ) { weaponSelect = true; }
			if ( Input.GetKeyUp(KeyCode.E) )   { weaponSelect = false; }

			// Shoot active weapon
			if ( Input.GetKey(KeyCode.Mouse0) ) { shootWeapon = true; }
			else 							    { shootWeapon = false; }

			// Flashlight
			if ( Input.GetKeyDown(KeyCode.F) ) { flashlight = !flashlight; }

		} // end of keyboard input

		// Check for controller input
		else {

			// Non-binary input
			leftAnalogX = Input.GetAxis("X Axis Left");
			leftAnalogY = Input.GetAxis("Y Axis Left");
			rightAnalogX = Input.GetAxis("X Axis Right");
			rightAnalogY = Input.GetAxis("Y Axis Right");
			triggerRight = Input.GetAxis("Right Trigger");

			// Move left
			if ( leftAnalogX >= 0.5f ) { moveLeft = true; }
			else 				       { moveLeft = false; }

			// Move right
			if ( leftAnalogX <= -0.5f ) { moveRight = true; }
			else 				        { moveRight = false; }

			// Crouch
			if ( Input.GetKeyDown(KeyCode.JoystickButton8) ) { crouch = !crouch; }

			// Jump
			if 		( Input.GetKeyDown(KeyCode.JoystickButton4) ) { jump = true; }
			else if ( Input.GetKeyDown(KeyCode.JoystickButton0) ) { jump = true; }
			else 											      { jump = false; }

			// Weapon select
			if ( Input.GetKeyDown(KeyCode.JoystickButton5) ) { weaponSelect = true; }
			if ( Input.GetKeyUp(KeyCode.JoystickButton5) )   { weaponSelect = false; }

			// Shoot active weapon
			if ( triggerRight < -0.5f ) { shootWeapon = true; }
			else 						{ shootWeapon = false; }

			// Flashlight
			if ( Input.GetKeyDown(KeyCode.JoystickButton3) ) { flashlight = !flashlight; }

		} // end of controller input
	
	} // end of Update()


	public bool canTakeDamage(string tag) {
		if (tag == "Lamp") {
			return true;
		}
		return false;
	}


	public bool isEnvironment(string tag) {
		if (tag == "Wall" || tag == "Floor" || tag == "Lamp" || tag == "Left-Right Door" || tag == "Up-Down Door") {
			return true;
		}
		return false;
	}

} // end of GameController.cs
