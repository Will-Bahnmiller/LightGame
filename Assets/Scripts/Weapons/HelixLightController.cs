using UnityEngine;
using System.Collections;

public class HelixLightController : MonoBehaviour {

	public Color color1, color2;
	public float freq, damage;

	private GameController gc;
	private Vector3 direction, outDir;
	private float radius, timer, angle;
	private int order;
	private bool hasCollided;


	void Start () {

		// Initialize data
		gc = Camera.main.GetComponent<GameController>();
		outDir = Vector3.Cross(Vector3.forward, direction).normalized * radius;
		hasCollided = false;
		timer = 0f;
		if (order == 1) { angle = 90f;  transform.GetComponent<Light>().color = color1; }
		else 			{ angle = 270f; transform.GetComponent<Light>().color = color2; }
	}


	void Update () {
	
		// Only move if no collision yet
		if (!hasCollided) {

			// Determine how to move based on child order number
			if (order == 1) {  angle = (angle + freq * Time.deltaTime) % 360f;  }
			else 			{  angle = (angle - freq * Time.deltaTime) % 360f;  }

			// Move
			transform.position = transform.parent.transform.position
								 + (outDir * Mathf.Sin(angle * Mathf.Deg2Rad));

		}

		// When collided, wait for trail
		else {
			timer += Time.deltaTime;
			if (timer > 1f) {
				Destroy(gameObject);
			}
		}

	} // end of Update()


	void OnCollisionEnter(Collision coll) {

		Debug.Log ("Helix collided with " + coll.transform.name);
		
		// If enemy, damage
		if (!hasCollided && gc.canTakeDamage(coll.gameObject.tag)) {
			coll.gameObject.SendMessage("TakeDamage", damage);
		}
		
		// When hitting the environment, set flag
		if ( !hasCollided && gc.isEnvironment(coll.transform.tag) ) {
			transform.parent = null;
			transform.GetComponent<Light>().enabled = false;
			hasCollided = true;
			rigidbody.isKinematic = true;
		}
	}


	void SetDirection(Vector3 d) { direction = d; }
	void SetRadius(float r) { radius = r; }
	void SetOrder(int o) { order = o; }

} // end of HelixLightController.cs
