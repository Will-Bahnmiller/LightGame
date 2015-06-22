using UnityEngine;
using System.Collections;

public class BubbleLightController : MonoBehaviour {

	public float duration, damage, spread, power;
	
	private GameController gc;
	private Vector3 corePos, myDirection;
	private float angle, timer;
	
	
	void Start() {
		
		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		corePos = transform.position;
		timer = 0f;
		
		// Provide initial position and angle
		transform.position = corePos + new Vector3( Random.Range(-0.1f, 0.1f) , 0.5f, 0f);
		angle = Random.Range(-spread + 90f, spread + 90f);
		
		// Toss flame at this angle with some force
		myDirection = new Vector3( Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin (angle * Mathf.Deg2Rad), 0f );
		transform.GetComponent<Rigidbody>().AddForce( myDirection * power, ForceMode.Impulse );
	}
	
	
	void FixedUpdate () {

		// Update timer
		timer = Mathf.Min (timer + Time.deltaTime, duration);

		// Slowly accelerate
		rigidbody.AddForce(myDirection * power, ForceMode.Impulse);

		// If timer expires, kill bubble
		if (timer == duration) {
			Destroy(gameObject);
		}
	}
	
	
	void OnCollisionEnter(Collision coll) {
		
		Debug.Log ("Bubble collided with " + coll.transform.name);
		
		// If enemy, damage
		if (gc.canTakeDamage(coll.gameObject.tag)) {
			coll.gameObject.SendMessage("TakeDamage", damage);
		}
		
		// Destroy this light
		Destroy(transform.gameObject);
		
	}
	
} // end of BubbleLightController.cs
