using UnityEngine;
using System.Collections;

public class WaveLightController : MonoBehaviour {

	public float damage, duration, maxSize, minSpeed, maxSpeed, radius, freq;
	
	private GameController gc;
	private Light myLight;
	private Vector3 direction, outDir;
	private float timer, power, mySpeed, angle;
	
	
	void Start() {

		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		myLight = gameObject.GetComponent<Light>();
		outDir = Vector3.Cross(Vector3.forward, direction).normalized * radius;
		mySpeed = minSpeed + (maxSpeed - minSpeed) * power;
		Debug.Log (mySpeed + " " + power);
		angle = 0f;  timer = 0f;
	}
	
	
	void Update () {

		// Update light range
		myLight.range = maxSize * Mathf.Abs(duration - timer);

		// Move light away from origin
		transform.Translate( (outDir * Mathf.Sin(angle * Mathf.Deg2Rad) + direction).normalized * mySpeed * Time.deltaTime  );
		angle = (angle + freq * Time.deltaTime) % 360f;

		// Update timer
		if (timer > duration) {
			Destroy(gameObject);
		}
		timer += Time.deltaTime;
	}
	
	
	void OnCollisionEnter(Collision coll) {
		
		Debug.Log ("Wave collided with " + coll.transform.name);
		
		// If enemy, damage
		if (gc.canTakeDamage(coll.gameObject.tag)) {
			coll.gameObject.SendMessage("TakeDamage", damage);
		}
		
		// Destroy this light
		Destroy(transform.gameObject);
		
	}

	void OnCollisionStay(Collision coll) {
		OnCollisionEnter(coll);
	}


	void SetDirection(Vector3 d) { direction = d; }
	void SetPower(float p) { power = p; }

} // end of FireworkLightController.cs
