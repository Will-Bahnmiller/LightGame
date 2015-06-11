using UnityEngine;
using System.Collections;

public class FreezeLightController : MonoBehaviour {

	public float damage;

	private GameController gc;


	void Start() {
		gc = Camera.main.GetComponent<GameController>();
	}


	void OnTriggerEnter(Collider coll) {

		Debug.Log ("child freeze collided with " + coll.transform.name);

		// If enemy, damage
		if (gc.canTakeDamage(coll.gameObject.tag)) {
			coll.gameObject.SendMessage("TakeDamage", damage);
		}
		
		// Destroy this light
		if (coll.transform.name != "Floor") {
			Destroy(transform.gameObject);
		}
	}

} // end of FreezeLightController.cs
