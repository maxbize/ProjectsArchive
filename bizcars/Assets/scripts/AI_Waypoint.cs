using UnityEngine;
using System.Collections;

public class AI_Waypoint : MonoBehaviour {

	public int order = -1;

	// Use this for initialization
	void Start () {
		if (order == -1) {
			Debug.LogError("ERROR: AI Waypoint order not set!");
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		AI_CarController AI = other.GetComponent<AI_CarController> ();
		if (AI) {
			AI.registerWaypoint(order);
		}
	}
}
