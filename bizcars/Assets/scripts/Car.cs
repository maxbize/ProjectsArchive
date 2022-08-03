using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
	
	public float accelForce = 8F;
	public float accelTorque = 20F;
	public float maxAngularVel = 150F;
	public float slipForceMult = 2F;
	public float fwdDragCoef = 0.4F;
	public float frictionCoef = 0.2F;
	public float brakePower = 0.5F;
	
	public float phase { get; protected set; }
	public float skidPhaseThreshold = 35F;
	public float skidVelocityThreshold = 0.2F;
	bool markedSkid = false;
	
	protected float controlMod;
	
	public GameObject trailClone;
	GhostRecorder myRecorder;
	public int playerNb;
	
	int framesSinceKey = GhostRecorder.keyFrameRate;

	protected Vector2 fwd;
	protected Vector2 right;

	// Use this for initialization
	virtual protected void Start () {
		myRecorder = gameObject.GetComponent<GhostRecorder>();
	}
	
	// Update is called once per frame
	virtual protected void FixedUpdate () {
		
		fwd = new Vector2 (-Mathf.Sin (Mathf.Deg2Rad * transform.eulerAngles.z), 
		                           Mathf.Cos (Mathf.Deg2Rad * transform.eulerAngles.z)).normalized;
		right = new Vector2 (-Mathf.Sin (Mathf.Deg2Rad * (transform.eulerAngles.z - 90F)), 
		                             Mathf.Cos (Mathf.Deg2Rad * (transform.eulerAngles.z - 90F))).normalized;
		
		// Keep a record of the current position
		framesSinceKey++;
		if (framesSinceKey >= GhostRecorder.keyFrameRate) {
			recordKeyFrame();
		}
		
		if (rigidbody2D.velocity.magnitude > 0) {
			// Add a slip force depending on phase difference between velocity and orientation
			phase = Vector2.Angle (rigidbody2D.velocity, fwd);
			float slipForce = Mathf.Sin (Mathf.Deg2Rad * phase) * slipForceMult * rigidbody2D.velocity.magnitude * controlMod;
			float slipForceDir = 1F;
			if (Vector3.Cross(fwd, rigidbody2D.velocity).z < 0) {
				slipForceDir = -1F;
			}
			rigidbody2D.AddForce (right * slipForce * slipForceDir);
			
			// There should always be some friction (const) + drag (v^2) opposing motion
			rigidbody2D.AddForce (-rigidbody2D.velocity * rigidbody2D.velocity.magnitude * fwdDragCoef);
			rigidbody2D.AddForce (-rigidbody2D.velocity.normalized * frictionCoef * controlMod);
		}
		
		// Add a special effect =)
		if (phase > skidPhaseThreshold 
		    && phase < 90
		    && !markedSkid 
		    && rigidbody2D.velocity.magnitude > skidVelocityThreshold) {
			markedSkid = true;
			MakeSkid(SkidMark.side.left);
			MakeSkid(SkidMark.side.right);
		}
		else if (phase < skidPhaseThreshold) {
			markedSkid = false;
		}
		
		// Make some noise!
		float speed = Mathf.Min (rigidbody2D.velocity.magnitude, 4f);
		AudioSource audio = GetComponent<AudioSource> ();;
		audio.pitch = speed / 3f;
	}
	
	void MakeSkid(SkidMark.side side) {
		GameObject skidMark = (GameObject)Instantiate(trailClone);
		skidMark.transform.parent = gameObject.transform;
		TrailRenderer skidMarkTr = skidMark.GetComponent<TrailRenderer> ();
		SkidMark skidMarkScript = skidMark.GetComponent<SkidMark> ();
		skidMarkScript.Init (skidMarkTr, this, side);
	}
	
	public void UpdateRoadControl (float newControl) {
		controlMod = newControl;
	}
	
	public void recordKeyFrame() {
		FrameState state = new FrameState (transform.position.x, transform.position.y, transform.eulerAngles.z);
		myRecorder.addFrame (state);
		framesSinceKey = 0;
	}
}
