using UnityEngine;
using System.Collections;

public class ChargeLightParticleController : MonoBehaviour {

	public float moveSpeed;

	private GameController gc;
	private PositionTracker pt;
	private Vector3 myDirection, parentPos;
	private float dist;


	void Start () {
		gc = Camera.main.GetComponent<GameController>();
		pt = Camera.main.GetComponent<PositionTracker>();
	}

	void Update () {
	
		// Update direction to move
		parentPos = transform.parent.transform.position;
		myDirection = (parentPos - transform.position).normalized;
		transform.rotation = transform.parent.rotation;

		// Move in that direction
		transform.Translate(myDirection * moveSpeed * Time.deltaTime);

		// Determine if reached destination or not charging anymore
		dist = Vector3.Distance(transform.position, parentPos);
		if ( dist < 0.5f || !playerIsCharging() ) {
			Destroy(gameObject);
		}

	} // end of Update()


	private bool playerIsCharging() {

		// Charging conditions
		if (gc.shootWeapon && pt.player.GetComponent<PlayerController>().canMove) {
			return true;
		}

		// No longer charging
		else {  return false;  }
	}

} // end of ChargeLightParticleController.cs
