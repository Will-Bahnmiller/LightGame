using UnityEngine;
using System.Collections;

public class CleanUp : MonoBehaviour {

	private float duration, timer;


	void Start () {
		timer = 0f;
	}


	void Update () {
		if (timer > duration) {
			Destroy(gameObject);
		}
		timer += Time.deltaTime;
	}


	public void SetDuration(float d) { duration = d; }

} // end of CleanUp.cs
