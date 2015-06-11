using UnityEngine;
using System.Collections;

public class LampController : MonoBehaviour {

	public float minIntensity, maxIntensity, health, restoreRate;
	public float fadeRate, flickerFreq, flickerRate;

	private Light myLight;
	private bool isLit, flickering, fgrow;
	private float myHealth, cooldown;


	void Start () {

		// Initialize data
		myLight = transform.GetComponent<Light>();
		myLight.intensity = minIntensity;
		isLit = false;
		flickering = false;
		fgrow = false;
		myHealth = health;
		cooldown = 0f;
	}


	void Update () {
	
		// Flicker if unlit and untouched
		if (!isLit && flickering == false && myLight.intensity == minIntensity) {

			// Wait before flickering again
			cooldown += flickerRate * Time.deltaTime;

			// After time lapse, begin to flicker
			if (cooldown >= flickerFreq) {
				cooldown = 0f;
				flickering = true;
				fgrow = true;
			}
		}

		// If in the state of flickering
		if (!isLit && flickering == true) {

			// Grow or fade the flicker
			if (fgrow) {  myLight.intensity = Mathf.Min (myLight.intensity + 1f, maxIntensity / 2f); }
			else {        myLight.intensity = Mathf.Max (minIntensity, myLight.intensity - 1f); }

			// Flip at maximum
			if (fgrow && myLight.intensity == maxIntensity / 2f) {
				fgrow = false;
			}

			// End flicker at minimum
			if (!fgrow && myLight.intensity == minIntensity) {
				flickering = false;
			}
		}

		// Check if lamp took enough damage to be lit
		if (myHealth == 0f) {
			isLit = true;
		}

		// Otherwise, fade away at a slow rate
		else {
			myHealth = Mathf.Min (health, myHealth + restoreRate * Time.deltaTime);
		}

		// Adjust light to match remaining health
		if (!flickering) {
			myLight.intensity = ((health - myHealth) / health) * (maxIntensity - minIntensity) + minIntensity;
		}

	} // end of Update()


	void TakeDamage(float damage) {
		myHealth = Mathf.Max (0f, myHealth - damage);
	}

} // end of LampController.cs
