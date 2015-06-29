using UnityEngine;
using System.Collections;

public class FreezeParticleController : MonoBehaviour {

	public float maxSize, minSize, duration;

	private GameController gc;
	private Light myLight;
	private Vector3 corePos;
	private float timeAlive;
	private int myPositionNum;


	void Start () {

		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		myLight = transform.GetComponent<Light>();
		myLight.range = maxSize;
		corePos = transform.position;
		timeAlive = 0f;

		// Set starting position
		transform.position += Quaternion.Euler(0f, 0f, (120f * myPositionNum)) * Vector3.up * 0.1f;
	}


	void Update () {

		// Drop central position to immitate gravity
		corePos = corePos - new Vector3(0f, 0.5f * Time.deltaTime, 0f);

		// Spin over time
		transform.RotateAround(corePos, Vector3.forward, 720f * Time.deltaTime);

		// Slowly fade over time
		myLight.range = maxSize - (maxSize - minSize) * (timeAlive / duration);

		// If alive for a certain time, kill
		timeAlive += Time.deltaTime;
		if (timeAlive >= duration) {
			Destroy(gameObject);
		}
	}


	void OnCollisionEnter(Collision coll) {
		if ( gc.isEnvironment(coll.transform.tag) ) {
			Destroy(gameObject);
		}
	}


	void SetPos(int pos) {
		myPositionNum = pos;
	}

} // end of FreezeParticleController.cs
