using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_CarController : Car {

	List<AI_Waypoint> waypoints = new List<AI_Waypoint>();

	int nextWp = 0;
	float lastSide = 0;
    const float maxV = 4.3f; // This is hard coded from a debug log

	// Use this for initialization
	override protected void Start () {
		waypoints = getAIWaypoints ();
		base.Start ();
	}
	
	// Update is called once per frame
	override protected void FixedUpdate () {

		if (controlEnabled) {
			// Turn towards next waypoint
	        Vector2 toWp = waypoints[nextWp].transform.position - transform.position;
	        float side = Mathf.Sign(Vector3.Cross(toWp, fwd).z);

			if (side != lastSide) {
				GetComponent<Rigidbody2D>().angularVelocity = 0;
			}

			// Find the radius of a best-fit circle to the two points. Use V = omega * r to find omega
			float alpha = Mathf.Deg2Rad * Vector2.Angle (toWp, right);
			//float turnRadius = toWp.magnitude * Mathf.Sin (alpha) / Mathf.Sin (Mathf.PI - 2 * alpha); // cuz math. Isoscoles triangle + Sin law
			float turnRadius = 1.2f; // Roughly the turn radius of max speed max turn turn
			float omega = GetComponent<Rigidbody2D>().velocity.magnitude / turnRadius;

			if (Mathf.Abs(GetComponent<Rigidbody2D>().angularVelocity) < maxAngularVel * controlMod) {
				GetComponent<Rigidbody2D>().angularVelocity -= accelTorque * controlMod * side;
				//rigidbody2D.angularVelocity -= omega;
			}
			else {
				GetComponent<Rigidbody2D>().angularVelocity = maxAngularVel * controlMod * Mathf.Sign(GetComponent<Rigidbody2D>().angularVelocity);
				//rigidbody2D.angularVelocity -= omega;
			}

	        // Forward accel. 
	        // If we do a full turn at max speed, can we make the next waypoint? Only accelerate if we cam
	        Vector2 center = (Vector2)transform.position + (right * turnRadius * side);
	        float centerToWp = ((Vector2)waypoints[nextWp].transform.position - center).magnitude;
	        if (centerToWp > turnRadius) {
	            GetComponent<Rigidbody2D>().AddForce(fwd * accelForce * controlMod);
	        }
	        else if (centerToWp < turnRadius / 2f) {
	            GetComponent<Rigidbody2D>().AddForce(-fwd * accelForce * brakePower * controlMod);
	        }
			lastSide = side;
		}
		base.FixedUpdate ();
	}

    // Make this a public static so that other classes can easily get valid pathing information (i.e. for item placement)
	public static List<AI_Waypoint> getAIWaypoints() {
		GameObject waypointParentObj = GameObject.Find ("AI_Waypoints");
		Component[] waypointComps = waypointParentObj.GetComponentsInChildren<AI_Waypoint> ();
        List<AI_Waypoint> wp = new List<AI_Waypoint>();
		foreach (Component comp in waypointComps) {
			wp.Add(comp.GetComponent<AI_Waypoint>());
		}
        return wp;
	}

	public void registerWaypoint(int order) {
		if (nextWp == order) {
			nextWp = (nextWp == waypoints.Count - 1 ? 0 : nextWp + 1);
		}
	}
}
