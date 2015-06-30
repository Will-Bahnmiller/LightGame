using UnityEngine;
using System.Collections;

public class HelixLightCoreController : MonoBehaviour {

	public GameObject childPrefab;
	public float moveSpeed, maxDist, duration, radius;

	private PositionTracker pt;
	private GameObject c1, c2;
	private Vector3 myDirection, childPos1, childPos2;
	private float timer, dist;


	void Start () {

		// Initialize data
		pt = Camera.main.GetComponent<PositionTracker>();
		myDirection = pt.mouseDirection;
		timer = 0f;

		// Spawn the two children radius distance away
		childPos1 = transform.position + Quaternion.Euler(0f, 0f, 90f) * myDirection * radius;
		childPos2 = transform.position + Quaternion.Euler(0f, 0f, -90f) * myDirection * radius;
		c1 = Instantiate(childPrefab, childPos1, Quaternion.identity) as GameObject;
		c2 = Instantiate(childPrefab, childPos2, Quaternion.identity) as GameObject;
		SendData(c1, 1);  SendData(c2, 2);

	}


	void Update () {

		// Update data
		dist = Vector3.Distance(transform.position, pt.playerPosition);
		timer += Time.deltaTime;

		// Keep moving 
		if (dist < maxDist && timer < duration && (c1 != null && c2 != null)) {

			// Move in direction
			transform.Translate(myDirection * moveSpeed * Time.deltaTime);

		}

		// Too far from player or lived too long, so kill
		else {
			Destroy(gameObject);
		}
	
	} // end of Update()


	void SendData(GameObject c, int order) {

		// Assign parent
		c.transform.parent = transform;

		// Pass direction, movespeed, radius, order number
		c.SendMessage("SetDirection", myDirection);
		c.SendMessage("SetRadius", radius);
		c.SendMessage("SetOrder", order);
	}

} // end of HelixLightCoreController.cs
