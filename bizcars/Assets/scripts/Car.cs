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

	protected bool controlEnabled = false;
	float controlBlockTimer = 0;
	
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

    Queue inventory = new Queue();
    int maxInventorySlots = 10;

    public Transform missilePrefab;

	AudioSource engineAudio;

	// Use this for initialization
	virtual protected void Start () {
		myRecorder = gameObject.GetComponent<GhostRecorder>();
		engineAudio = GetComponent<AudioSource> ();
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
		
		if (GetComponent<Rigidbody2D>().velocity.magnitude > 0) {
			// Add a slip force depending on phase difference between velocity and orientation
			phase = Vector2.Angle (GetComponent<Rigidbody2D>().velocity, fwd);
			float slipForce = Mathf.Sin (Mathf.Deg2Rad * phase) * slipForceMult * GetComponent<Rigidbody2D>().velocity.magnitude * controlMod;
			float slipForceDir = 1F;
			if (Vector3.Cross(fwd, GetComponent<Rigidbody2D>().velocity).z < 0) {
				slipForceDir = -1F;
			}
			GetComponent<Rigidbody2D>().AddForce (right * slipForce * slipForceDir);
			
			// There should always be some friction (const) + drag (v^2) opposing motion
			GetComponent<Rigidbody2D>().AddForce (-GetComponent<Rigidbody2D>().velocity * GetComponent<Rigidbody2D>().velocity.magnitude * fwdDragCoef);
			GetComponent<Rigidbody2D>().AddForce (-GetComponent<Rigidbody2D>().velocity.normalized * frictionCoef * controlMod);
		}
		
		// Add a special effect =)
		if (phase > skidPhaseThreshold 
		    && phase < 90
		    && !markedSkid 
		    && GetComponent<Rigidbody2D>().velocity.magnitude > skidVelocityThreshold) {
			markedSkid = true;
			MakeSkid(SkidMark.side.left);
			MakeSkid(SkidMark.side.right);
		}
		else if (phase < skidPhaseThreshold) {
			markedSkid = false;
		}
		
		// Make some noise!
		float speed = Mathf.Min (GetComponent<Rigidbody2D>().velocity.magnitude, 4f);
		engineAudio.pitch = speed / 3f;

		if (!controlEnabled) {
			controlBlockTimer -= Time.fixedDeltaTime;
			if (controlBlockTimer < 0) {
				controlEnabled = true;
			}
		}
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
	
	public virtual void recordKeyFrame() {
		if (myRecorder.enabled) {
			FrameState state = new FrameState (transform.position.x, transform.position.y, transform.eulerAngles.z);
			myRecorder.addFrame (state);
		}
		framesSinceKey = 0;
	}

    public void addItem(Item.type itemType) {
        if (inventory.Count < maxInventorySlots) {
            inventory.Enqueue(itemType);
        }
    }

    protected void useNextItem() {
        if (inventory.Count > 0) {
            Item.type item = (Item.type)inventory.Dequeue();
            switch (item) {
                case Item.type.missile:
                    fireMissile();
                    break;
                default:
                    break;
            }
        }
    }

    protected void fireMissile() {
        Transform missile = (Transform)Instantiate(missilePrefab, transform.position, transform.rotation);
        missile.GetComponent<Missile>().carNbToIgnore = playerNb;
    }

	public void disableControl(float time) {
		controlEnabled = false;
		controlBlockTimer = time;
	}
}
