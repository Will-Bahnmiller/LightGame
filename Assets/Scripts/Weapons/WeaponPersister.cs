using UnityEngine;
using System.Collections;

public class WeaponPersister : MonoBehaviour {

	private bool collided;
	private float timer, waitTime;


	void Start () {
		collided = false;
		timer = gameObject.GetComponent<TrailRenderer>().time;
		waitTime = 0f;
	}


	void Update () {
		if (collided) {
			if (waitTime >= timer) {
				Destroy(gameObject);
			}
			waitTime += Time.deltaTime;
		}
	}


	void SetPersist(bool p) {  collided = p;  }

} // end of WeaponPersister.cs
