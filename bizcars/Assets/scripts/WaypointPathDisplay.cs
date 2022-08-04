using UnityEngine;
using System.Collections.Generic;

public class WaypointPathDisplay : MonoBehaviour {
	
	// Use this for initialization
	void OnDrawGizmos () {
		List<AI_Waypoint> waypoints = new List<AI_Waypoint>();

		Component[] waypointComps = gameObject.GetComponentsInChildren<AI_Waypoint> ();
		foreach (Component comp in waypointComps) {
			waypoints.Add(comp.GetComponent<AI_Waypoint>());
		}

		int wpNum = 0;
		int wpInd = 0;
		Vector3 lastP = Vector3.zero, nextP, firstP = Vector3.zero;
		while (wpNum != waypoints.Count) {
			if (waypoints[wpInd].order == wpNum) {
				if (wpNum == 0) {
					lastP = waypoints[wpInd].transform.position;
					firstP = lastP;
					Gizmos.DrawWireSphere(lastP, waypoints[wpInd].GetComponent<CircleCollider2D>().radius);
				}
				else {
					nextP = waypoints[wpInd].transform.position;
					Gizmos.DrawLine(lastP,nextP);
					lastP = nextP;
					Gizmos.DrawWireSphere(lastP, waypoints[wpInd].GetComponent<CircleCollider2D>().radius);
				}
				wpNum++;
			}
			wpInd = wpInd == waypoints.Count - 1 ? 0 : wpInd + 1;
		}
		Gizmos.DrawLine(lastP,firstP);
	}
}
