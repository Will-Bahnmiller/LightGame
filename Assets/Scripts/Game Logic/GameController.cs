using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public bool cutscene, controllerScheme;


	void Start () {

		// Initialize data
		cutscene = false;
	}
	

	void Update () {

		// Switch between controller and keyboard/mouse controls
		if (Input.GetKeyDown(KeyCode.Return)) {
			controllerScheme = !controllerScheme;
		}

	}


	public bool canTakeDamage(string tag) {

		// Determine if the object in question can take damage
		if (tag == "Lamp") {
			return true;
		}

		return false;
	}

} // end of GameController.cs
