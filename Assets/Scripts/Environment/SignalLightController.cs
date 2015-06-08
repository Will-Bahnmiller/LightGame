using UnityEngine;
using System.Collections;

public class SignalLightController : MonoBehaviour {

	public float maxIntensity, minIntensity, growthRate;

	private bool lightGrow;
	private Light myLight;


	void Start() {
		myLight = gameObject.GetComponent<Light>();
		myLight.intensity = minIntensity;
	}


	void Update () {

		// Increase or decrease light intensity
		if (lightGrow) { myLight.intensity = Mathf.Min (maxIntensity, myLight.intensity + growthRate * Time.deltaTime); }
		else {			 myLight.intensity = Mathf.Max (minIntensity, myLight.intensity - growthRate * Time.deltaTime); }

		// Flip at max or min intensity
		if (myLight.intensity == maxIntensity || myLight.intensity == minIntensity) { lightGrow = !lightGrow; }

	}

} // end of SignalLightController.cs
