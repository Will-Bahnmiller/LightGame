using UnityEngine;
using System.Collections;

public class OldBeamLightController : MonoBehaviour {

	private Vector3 mousePos, playerPos, direction;
	public float damage, missileSpeed, maxLength;
	private float length, angle;
	private bool stopped;
	private LineRenderer myLineRenderer;
	
	void Start() {
		stopped = false;
		length = 0f;
	}
	
	void Update () {

		// Update mouse and players position
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		direction = playerPos - mousePos;
		direction.z = 0f;
		direction.Normalize();
		angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		angle = angle + 270f;

		// Turn beam in direction of mouse
		transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
			
		// Grow the beam, provided it can grow
		if (!stopped) {
			length = Mathf.Min (length+missileSpeed, maxLength);
			transform.position = playerPos - (length * direction);
			transform.localScale = new Vector3(1f, length, 1f);
		}

		// Once player lifts mouse, destroy this light beam
		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			Destroy(transform.gameObject);
		}

	} // end of Update()

	void OnCollisionEnter(Collision coll) {
		Debug.Log ("Beam collided with " + coll.gameObject.name);
		stopped = true;
	}

	void OnCollisionExit(Collision coll) {
		Debug.Log ("Beam uncollided with " + coll.gameObject.name);
		stopped = false;
	}

}
