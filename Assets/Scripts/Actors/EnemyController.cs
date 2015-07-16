using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float health, damage;

	private float myHealth;


	void Start () {

		// Initialize data
		myHealth = health;
	}


	void TakeDamage(float d) {
		myHealth = Mathf.Max (0f, myHealth - d);
		if (myHealth == 0f) {
			// play death animation, which destroys when done
			Destroy(gameObject);
		}
	}


	void OnCollisionEnter(Collision coll) {
		if (coll.transform.tag == "Player") {
			coll.gameObject.SendMessage("TakeDamage", damage);
		}
	}


	float getMyHealth() { return myHealth; }

} // end of EnemyController.cs
