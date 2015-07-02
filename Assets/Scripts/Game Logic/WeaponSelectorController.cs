using UnityEngine;
using System.Collections;

public class WeaponSelectorController : MonoBehaviour {

	public Sprite[] weaponImages;
	public GameObject imagePrefab;
	public float radius;
	public int currentWeapon;

	private GameController gc;
	private PositionTracker pt;
	private GameObject[] selections;
	private Vector3 choicePos;
	private float minDistance;
	private int tempSelection, minDistIndex;


	void Start () {
	
		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		pt = Camera.main.GetComponent<PositionTracker>();
		selections = new GameObject[weaponImages.Length];
		currentWeapon = 0;  minDistance = 999f;

		// Dynamically create one GameObject per weapon
		for (int i = 0; i < weaponImages.Length; i++) {

			// Determine position
			choicePos = (Quaternion.Euler(0f, 0f, -(float)i * 360f / weaponImages.Length) * Vector3.up) * radius;
			choicePos.z = -3f;

			// Create the object
			selections[i] = Instantiate(imagePrefab, choicePos, Quaternion.identity) as GameObject;
			selections[i].transform.parent = transform;
			selections[i].transform.name = i.ToString();

			// Set the sprite
			selections[i].GetComponent<SpriteRenderer>().sprite = weaponImages[i];
		}

		// Start invisible
		changeVisibility(false);
	}
	

	void Update () {
	
		// Only appear when user wants to select weapon
		if ( gc.weaponSelect ) {

			// Become visible if not already
			if (!transform.GetComponent<SpriteRenderer>().enabled) {
				changeVisibility(true);
			}

			// Take note of closest weapon to mouse location
			minDistance = 999f;
			foreach (GameObject w in selections) {
				findMinDist(w, pt.mousePosition);
			}
			tempSelection = minDistIndex;

			// Enable light of closest selection
			foreach (GameObject w in selections) {
				if (int.Parse(w.transform.name) != tempSelection) {
					w.GetComponent<SpriteRenderer>().color = Color.black;
				}
				else {
					w.GetComponent<SpriteRenderer>().color = Color.white;
				}
			}
		}
		
		// Become invisible if not already
		else {
			if (transform.GetComponent<SpriteRenderer>().enabled) {
				changeVisibility(false);
				currentWeapon = tempSelection;
			}
		}

		// Pass weapon choice to game logic
		gc.currentWeapon = currentWeapon;

	} // end of Update()


	void findMinDist(GameObject w, Vector3 mousePos) {
		float dist = Vector3.Distance(w.transform.position, mousePos);
		if (dist < minDistance) {
			minDistance = dist;
			minDistIndex = int.Parse( w.transform.name );
		} 
	}


	void changeVisibility(bool v) {
		transform.GetComponent<SpriteRenderer>().enabled = v;
		foreach (GameObject w in selections) {
			w.GetComponent<SpriteRenderer>().enabled = v;
		}
	}

} // end of WeaponSelectorController.cs
