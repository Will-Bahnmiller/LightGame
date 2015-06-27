using UnityEngine;
using System.Collections;

public class TrailLightController : MonoBehaviour {

	public float moveSpeed;

	private GameObject target;
	private Vector3 destination;
	private float delayTime;
	private bool hasReachedDoor;


	void Start () {
		delayTime = 0f;
		hasReachedDoor = false;
		destination = (target.transform.position - transform.position).normalized;
	}


	void Update () {

		// Constantly move to target position
		if (!hasReachedDoor) {

			transform.Translate( destination * moveSpeed * Time.deltaTime );

			// When close enough, open door
			if ( Vector3.Distance(transform.position, target.transform.position) < 1f ) {
				hasReachedDoor = true;
			}
		}

		// Wait a bit otherwise
		else {
			delayTime = Mathf.Min (delayTime + Time.deltaTime, 0.5f);
		}

		// After waiting, kill
		if (delayTime == 0.5f) {
			target.GetComponent<DoorController>().isLocked = false;
			Destroy(gameObject);
		}
	
	} // end of Update()


	void SetTarget(GameObject t) {
		target = t;
	}

} // end of TrailLightController.cs
