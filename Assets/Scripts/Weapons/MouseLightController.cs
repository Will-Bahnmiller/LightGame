using UnityEngine;
using System.Collections;

public class MouseLightController : MonoBehaviour {

	public bool hasMouseLight;
	public float drainRate, restoreRate, maxLightIntensity;

	private PositionTracker positionTracker;
	private Light myLight;


	void Start () {
		positionTracker = Camera.main.GetComponent<PositionTracker>();
		myLight = gameObject.GetComponent<Light>();
		maxLightIntensity = myLight.intensity;
	}


	void Update () {
		
		// Move light to mouse position
		myLight.transform.position = positionTracker.mousePosition;

		// Turn light on/off with mouse
		if (Input.GetKeyDown(KeyCode.Mouse1) && hasMouseLight) {
			myLight.enabled = true;
		}
		if (Input.GetKeyUp(KeyCode.Mouse1) && hasMouseLight)  {
			myLight.enabled = false;
		}

		// Determine drain or restore
		if (myLight.enabled == true) {
			myLight.intensity = Mathf.Max ( 0f, myLight.intensity - drainRate );
		}
		else {
			myLight.intensity = Mathf.Min ( maxLightIntensity, myLight.intensity + restoreRate );
		}
	}

} // end of MouseLightController.cs
