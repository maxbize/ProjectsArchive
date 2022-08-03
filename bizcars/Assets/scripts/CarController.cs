using UnityEngine;
using System.Collections;

public class CarController : Car {

	private KeyCode[] keys;

	// player keys, in order : up, right, bottom, left
	private KeyCode[][] playerKeys = new KeyCode[][]{
		new KeyCode[]{ KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow },
		new KeyCode[]{ KeyCode.W, KeyCode.D, KeyCode.S, KeyCode.A },
		new KeyCode[]{ KeyCode.Y, KeyCode.J, KeyCode.H, KeyCode.G },
		new KeyCode[]{ KeyCode.Alpha9, KeyCode.P, KeyCode.O, KeyCode.I }
	};
	public enum Dir{Up, Right, Down, Left}

	// Use this for initialization
	override protected void Start () {
		base.Start ();
	}

	public void SetPlayerInputNb(int playerNb){
		this.playerNb = playerNb;
		if (playerNb < playerKeys.Length) {
			this.keys = playerKeys[playerNb];
		}
	}

	// Update is called once per frame
	override protected void FixedUpdate () {

		// Forward acceleration
		if (keys != null && Input.GetKey(keys[(int)Dir.Up])) {
			rigidbody2D.AddForce(fwd * accelForce * controlMod);
		}
		if (keys != null && Input.GetKey(keys[(int)Dir.Down])) {
			rigidbody2D.AddForce(-fwd * accelForce * brakePower * controlMod);
		}

		// Handle turning
		if (Mathf.Abs(rigidbody2D.angularVelocity) < maxAngularVel * controlMod) {
			if (keys != null && Input.GetKey(keys[(int)Dir.Left])) {
				rigidbody2D.angularVelocity += accelTorque * controlMod;
			}
			else if (keys != null && Input.GetKey(keys[(int)Dir.Right])) {
				rigidbody2D.angularVelocity -= accelTorque * controlMod;
			}
		}
		else {
			rigidbody2D.angularVelocity = maxAngularVel * controlMod * Mathf.Sign(rigidbody2D.angularVelocity);
		}

		// Stop turn on key up
		if (keys != null && !Input.GetKey(keys[(int)Dir.Right]) && !Input.GetKey(keys[(int)Dir.Left])) {
			rigidbody2D.angularVelocity = 0F;
		}
	
		base.FixedUpdate ();
		
	}

	void MakeSkid(SkidMark.side side) {
		GameObject skidMark = (GameObject)Instantiate(trailClone);
		skidMark.transform.parent = gameObject.transform;
		TrailRenderer skidMarkTr = skidMark.GetComponent<TrailRenderer> ();
		SkidMark skidMarkScript = skidMark.GetComponent<SkidMark> ();
		skidMarkScript.Init (skidMarkTr, this, side);
	}

}
