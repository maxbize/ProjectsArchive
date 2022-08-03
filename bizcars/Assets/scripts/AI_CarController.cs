using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_CarController : Car {

	List<AI_Waypoint> waypoints = new List<AI_Waypoint>();

	int nextWp = 0;

	float lastSide = 0;

	// Use this for initialization
	override protected void Start () {
		getWaypoints ();
		base.Start ();
	}
	
	// Update is called once per frame
	override protected void FixedUpdate () {

		// Turn towards next waypoint
		Vector2 toWp = waypoints [nextWp].transform.position - transform.position;
		float side = (Vector3.Cross(toWp, fwd).z) * 2;
		Debug.Log (side);

		if (Mathf.Abs(rigidbody2D.angularVelocity) < maxAngularVel * controlMod) {
			rigidbody2D.angularVelocity -= accelTorque * controlMod * side;
		}
		else {
			rigidbody2D.angularVelocity = maxAngularVel * controlMod * Mathf.Sign(rigidbody2D.angularVelocity);
		}

		
		// Forward accel. Need to not accelerate if we can't make it to the next checkpoint
		rigidbody2D.AddForce(fwd * accelForce * controlMod);


		lastSide = side;

		base.FixedUpdate ();
	}

	void getWaypoints() {
		GameObject waypointParentObj = GameObject.Find ("AI_Waypoints");
		Component[] waypointComps = waypointParentObj.GetComponentsInChildren<AI_Waypoint> ();
		foreach (Component comp in waypointComps) {
			waypoints.Add(comp.GetComponent<AI_Waypoint>());
		}
	}

	public void registerWaypoint(int order) {
		if (nextWp == order) {
			nextWp = (nextWp == waypoints.Count - 1 ? 0 : nextWp + 1);
		}
	}
}
