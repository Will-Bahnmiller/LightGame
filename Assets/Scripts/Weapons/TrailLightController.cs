using UnityEngine;
using System.Collections;

public class TrailLightController : MonoBehaviour {

	public float duration, fadeRate, growRate, maxRadius, brightRate, maxIntensity;

	private Light myLight;
	private SphereCollider myCollider;
	private bool explode;


	void Start() {

		// Initialize data
		myLight = gameObject.GetComponent<Light>();
		myCollider = gameObject.GetComponent<SphereCollider>();
		explode = false;
	}


	void Update () {
	
		// Exist for duration seconds
		duration = Mathf.Max (0f, duration - Time.deltaTime);

		// Once duration is over, fade away
		if (!explode && duration == 0f) {
			myLight.intensity = Mathf.Max (0f, myLight.intensity - fadeRate * Time.deltaTime);
		}

		// Once completely faded, clean up
		if (myLight.intensity == 0f) {
			Destroy(gameObject);
		}

		// Explode on collision
		if (explode) {
			
			// Increase light intensity
			myLight.intensity = Mathf.Min (maxIntensity, myLight.intensity + brightRate * Time.deltaTime);

			// Grow collider
			myCollider.radius = Mathf.Min (maxRadius, myCollider.radius + growRate * Time.deltaTime);

			// Once radius reaches max, clean up
			if (myCollider.radius == maxRadius) {
				Destroy(gameObject);
			}

		}
	}


	void OnCollisionEnter(Collision coll) {

		// Detonate on collision
		explode = true;

		// If enemy, damage it

	}

} // end of TrailLightController.cs
