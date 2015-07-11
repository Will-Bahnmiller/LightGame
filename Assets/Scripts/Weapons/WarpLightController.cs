using UnityEngine;
using System.Collections;

public class WarpLightController : MonoBehaviour {

	public float cooldown, missileSpeed, slowSpeed, maxDist, maxSize, slowPercent;

	private GameController gc;
	private PositionTracker pt;
	private Light myLight;
	private Vector3 playerPos, mouseDir;
	private float distToPlayer, mySpeed, timer, cooldownTimer;
	private bool isWarping, warpToPos;


	void Start () {

		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		pt = Camera.main.GetComponent<PositionTracker>();
		myLight = gameObject.GetComponent<Light>();
		myLight.range = maxSize;
		mySpeed = missileSpeed;  timer = 0f;  cooldownTimer = cooldown;
		isWarping = false;  warpToPos = false;
		rigidbody.detectCollisions = false;
	}


	void Update () {

		// Update input if not on cooldown
		if (cooldownTimer == cooldown) {
			isWarping = isWarping || gc.warpLight;
		}

		// Move when signaled
		if (isWarping) {  

			// Enable light
			myLight.enabled = true;

			// Check for next signal
			if (timer > 0.1f) {  warpToPos = gc.warpLight;  }
			timer += Time.deltaTime;

			// Only move when not warped yet
			if (!warpToPos) {

				// Move in mouse direction
				distToPlayer = Vector3.Distance(transform.position, playerPos);
				if ( distToPlayer <= maxDist ) {
					transform.Translate(mouseDir * mySpeed * Time.deltaTime);
				}

				// At max distance, return to player
				else {
					ReturnToPlayer();
				}

				// Fade and slow near max distance
				if (distToPlayer/maxDist >= slowPercent) {
					myLight.range = maxSize - maxSize * (distToPlayer/maxDist - slowPercent) / (1-slowPercent);
					mySpeed = missileSpeed - (missileSpeed - slowSpeed) * (distToPlayer/maxDist - slowPercent) / (1-slowPercent);
				}
				else {
					myLight.range = maxSize;
					mySpeed = missileSpeed;
				}
			}

			// Signal to warp
			else {
				pt.player.rigidbody.detectCollisions = false;
				pt.player.SendMessage("BeginWarp", transform.position);
				ReturnToPlayer();
			}

		}

		// Otherwise update info
		else {
			cooldownTimer = Mathf.Min(cooldownTimer + Time.deltaTime, cooldown);
			myLight.enabled = false;
			playerPos = pt.playerPosition;
			mouseDir = pt.mouseDirection;
			transform.position = new Vector3(pt.playerPosition.x, pt.playerPosition.y, 0.7f);
		}

	} // end of Update()


	void ReturnToPlayer() {
		timer = 0f;  cooldownTimer = 0f;
		isWarping = false;
		warpToPos = false;
		myLight.enabled = false;
		myLight.range = maxSize;
		mySpeed = missileSpeed;
		transform.position = new Vector3(pt.playerPosition.x, pt.playerPosition.y, 0.7f);
	}


	/*bool playerCanGoHere() {

		bool test = false;
		RaycastHit hitInfo;

		// Enable collisions first
		rigidbody.detectCollisions = true;

		// Sweep around warp light
		for (int i = 0; i < 360; i+= 10) {
			test = test || rigidbody.SweepTest( Quaternion.Euler(0f, 0f, i) * Vector3.up, out hitInfo, 0.5f );
			Debug.Log ("good");
			if (test) {
				Debug.Log ("not good " + hitInfo.transform.name + " " + hitInfo.point);
				rigidbody.detectCollisions = false;
				return false;
			}
		}

		// No collisions
		rigidbody.detectCollisions = false;
		return true;
	}*/

} // end of WarpLightController.cs
