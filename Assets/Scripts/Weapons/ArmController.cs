﻿using UnityEngine;
using System.Collections;

public class ArmController : MonoBehaviour {
	
	public float turnSpeed;
	public float[] weaponCooldowns;
	public GameObject[] missilePrefabs;
	public bool instantRotate, hasFlashLight, flashLightOn;

	private GameController gc;
	private PositionTracker positionTracker;
	private ArrayList missiles;
	private bool shooted;
	private float[] timers;
	private int activeWeapon, weaponCount;
		

	void Start() {

		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		flashLightOn = false;  shooted = false;
		missiles = new ArrayList();
		timers = (float[])weaponCooldowns.Clone();
		activeWeapon = 0;  weaponCount = missilePrefabs.Length;
	}


	void Update () {

		// Turn light towards mouse
		float angle = positionTracker.angle;

		// Turn light depending on settings
		if (!gc.controllerScheme) {
			if (instantRotate) {
				transform.rotation = Quaternion.Euler(new Vector3(angle, 90f, 0f));
			}
			else {
				transform.rotation = Quaternion.Slerp(transform.rotation,
				                                      Quaternion.Euler (angle, 90f, 0f),
				                                      turnSpeed * Time.deltaTime);
			}
		}
		else {
			if (Mathf.Abs(Input.GetAxis("Y Axis Right")) > .1f || Mathf.Abs(Input.GetAxis("X Axis Right")) > .1f) {
				transform.rotation = Quaternion.Euler(new Vector3(Mathf.Atan2(Input.GetAxis("Y Axis Right"),
			                                          Input.GetAxis ("X Axis Right")) * Mathf.Rad2Deg, 90f, 0f));
			}
			else {
				if (transform.parent.GetComponent<PlayerController>().facingRight == true) {
					transform.rotation = Quaternion.Euler(0f, 90f, 0f);
				}
				else {
					transform.rotation = Quaternion.Euler(180f, 90f, 0f);
				}
			}
		}

		// Weapon cooldown
		for (int i = 0; i < timers.Length; i++) {
			timers[i] = Mathf.Min (timers[i] + Time.deltaTime, weaponCooldowns[i]);
		}

		// Light on/off
		if (!gc.controllerScheme) {
			if (Input.GetKeyDown(KeyCode.F) && hasFlashLight) {
				flashLightOn = !flashLightOn;
			}
		}
		else {
			if (Input.GetKeyDown(KeyCode.JoystickButton3)) {
				flashLightOn = !flashLightOn;
			}
		}
		if (flashLightOn) { gameObject.GetComponent<Light>().enabled = true; }
		else 			  { gameObject.GetComponent<Light>().enabled = false; }

		// Switch weapon
		if (!gc.controllerScheme) {
			if (Input.GetKeyDown(KeyCode.E) && !Input.GetKey(KeyCode.Mouse0)) {
				activeWeapon = (activeWeapon+1) % weaponCount;
			}
		}
		else {
			if (Input.GetKeyDown(KeyCode.JoystickButton1) && Input.GetAxis("Right Trigger") > -0.5f) {
				activeWeapon = (activeWeapon+1) % weaponCount;
			}
		}

		// Shoot active weapon
		if ( shooted == false && ( (!gc.controllerScheme && Input.GetKeyDown(KeyCode.Mouse0)) ||
		                          (gc.controllerScheme && Input.GetAxis("Right Trigger") < -0.5f) )
		    				&& positionTracker.player.GetComponent<PlayerController>().canMove) {

			// The weapon has been shot
			shooted = true;

			// Fire the missile
			if (timers[activeWeapon] == weaponCooldowns[activeWeapon]) {
				missiles.Add ( Instantiate (missilePrefabs[activeWeapon], transform.position, Quaternion.identity) );
				timers[activeWeapon] = 0f;
			}
/*
			// Basic weapon
			if (activeWeapon == 0) {
				if (timer == weaponCooldown) {
					missiles.Add ( Instantiate (basicLight, transform.position, Quaternion.identity) as GameObject );
					timer = 0f;
				}
			}

			// Beam weapon
			if (activeWeapon == 1) {
				missiles.Add ( Instantiate (beamLight, transform.position, Quaternion.identity) as GameObject );
			}

			// Freeze weapon
			if (activeWeapon == 2) {
				if (timer == weaponCooldown) {
					missiles.Add ( Instantiate (freezeLight, transform.position, Quaternion.identity) as GameObject );
					timer = 0f;
				}
			}

			// Flame weapon
			if (activeWeapon == 3) {
				missiles.Add ( Instantiate (flameLight, transform.position, Quaternion.identity) as GameObject );
			}

			// Bounce weapon
			if (activeWeapon == 4) {
				if (timer == weaponCooldown) {
					missiles.Add ( Instantiate (bounceLight, transform.position, Quaternion.identity) as GameObject );
					timer = 0f;
				}
			}

			// Rebound weapon
			if (activeWeapon == 5) {
				if (timer == weaponCooldown) {
					missiles.Add ( Instantiate (reboundLight, transform.position, Quaternion.identity) as GameObject );
					timer = 0f;
				}
			}*/

		} // end of shoot

		// Re-enable weapon firing
		if ( shooted == true && ( (!gc.controllerScheme && Input.GetKeyUp(KeyCode.Mouse0)) ||
		                          (gc.controllerScheme && Input.GetAxis("Right Trigger") >= -.5f) )
		    && positionTracker.player.GetComponent<PlayerController>().canMove) {
			shooted = false;
		}

	} // end of Update()

} // end of ArmController.cs
