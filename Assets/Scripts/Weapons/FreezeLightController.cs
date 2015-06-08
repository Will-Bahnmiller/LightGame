using UnityEngine;
using System.Collections;

public class FreezeLightController : MonoBehaviour {

	public float damage;

	void OnCollisionEnter(Collision coll) {

		Debug.Log ("child freeze collided with " + coll.transform.name);

		// If enemy, take damage and freeze/slow
		
		
		// Destroy this light
		Destroy(transform.gameObject);
		
	}

} // end of FreezeLightController.cs
