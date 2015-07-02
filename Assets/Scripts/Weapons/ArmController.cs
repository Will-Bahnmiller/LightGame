using UnityEngine;
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
			if (Mathf.Abs(gc.rightAnalogY) > .1f || Mathf.Abs(gc.rightAnalogX) > .1f) {
				transform.rotation = Quaternion.Euler(
					new Vector3(Mathf.Atan2(gc.rightAnalogY, gc.rightAnalogX) * Mathf.Rad2Deg, 90f, 0f));
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
		if (hasFlashLight) { flashLightOn = gc.flashlight; }
		else 			   { flashLightOn = false; }
		if (flashLightOn) { gameObject.GetComponent<Light>().enabled = true; }
		else 			  { gameObject.GetComponent<Light>().enabled = false; }

		// Weapon selected determined by script
		activeWeapon = gc.currentWeapon;

		// Shoot active weapon
		if ( shooted == false && gc.shootWeapon && !gc.weaponSelect
		    				 && positionTracker.player.GetComponent<PlayerController>().canMove) {

			// The weapon has been shot
			shooted = true;

			// Fire the missile
			if (timers[activeWeapon] == weaponCooldowns[activeWeapon]) {
				missiles.Add ( Instantiate (missilePrefabs[activeWeapon], transform.position, Quaternion.identity) );
				timers[activeWeapon] = 0f;
			}

		} // end of shoot

		// Re-enable weapon firing
		if ( shooted == true && !gc.shootWeapon
		    				&& positionTracker.player.GetComponent<PlayerController>().canMove) {
			shooted = false;
		}

	} // end of Update()

} // end of ArmController.cs
