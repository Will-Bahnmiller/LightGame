using UnityEngine;
using System.Collections;

public class CrawlEnemyController : MonoBehaviour {

	public float moveSpeed;
	public Vector3[] path;

	private Vector3 goalPos;
	private int currentPath;


	void Start () {

		// Initialize data
		currentPath = 0;
	}


	void Update () {

		// Follow path
		goalPos = path[currentPath];
		if (Vector3.Distance(transform.position, goalPos) > 0.1f) {
			transform.Translate((goalPos - transform.position).normalized * moveSpeed * Time.deltaTime);
		}
		else {
			currentPath = (currentPath + 1) % path.Length;
		}
	
	}

} // end of CrawlEnemyController.cs
