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
		if (tag == "Lamp") {
			return true;
		}
		return false;
	}


	public bool isEnvironment(string tag) {
		if (tag == "Wall" || tag == "Floor" || tag == "Lamp" || tag == "Left-Right Door" || tag == "Up-Down Door") {
			return true;
		}
		return false;
	}

} // end of GameController.cs
