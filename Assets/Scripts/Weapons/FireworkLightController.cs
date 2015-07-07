using UnityEngine;
using System.Collections;

public class FireworkLightController : MonoBehaviour {

	public float damage, duration, maxSize;
	
	private GameController gc;
	private Light myLight;
	private Vector3 direction;
	private float timer, power;
	
	
	void Start() {

		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		myLight = gameObject.GetComponent<Light>();
		myLight.color = new Color(Random.Range(150f, 255f)/255f,
		                                                   Random.Range(150f, 255f)/255f,
		                                                   Random.Range(150f, 255f)/255f);
		myLight.range = maxSize;

		// Move away from center a bit
		rigidbody.AddForce(direction * power, ForceMode.Impulse);
	}
	
	
	void Update () {

		// Update light
		myLight.range = maxSize * Mathf.Abs(duration - timer);

		// Update timer
		if (timer > duration) {
			Destroy(gameObject);
		}
		timer += Time.deltaTime;
	}
	
	
	void OnCollisionEnter(Collision coll) {
		
		Debug.Log ("Firework collided with " + coll.transform.name);
		
		// If enemy, damage and apply burn
		if (gc.canTakeDamage(coll.gameObject.tag)) {
			coll.gameObject.SendMessage("TakeDamage", damage);
			//coll.gameObject.SendMessage("ApplyBurn");
		}
		
		// Destroy this light
		Destroy(transform.gameObject);
		
	}

	void OnCollisionStay(Collision coll) {
		OnCollisionEnter(coll);
	}


	void SetDirection(Vector3 d) { direction = d; }
	void SetPower(float p) { power = 0.9f * p + 0.1f; }

} // end of FireworkLightController.cs
