using UnityEngine;
using System.Collections;

public class CarController : Car {

	private KeyCode[] keys;

	// player keys, in order : up, right, bottom, left, use item
	private KeyCode[][] playerKeys = new KeyCode[][]{
		new KeyCode[]{ KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow,  KeyCode.Space},
		new KeyCode[]{ KeyCode.W, KeyCode.D, KeyCode.S, KeyCode.A, KeyCode.LeftShift },
		new KeyCode[]{ KeyCode.Y, KeyCode.J, KeyCode.H, KeyCode.G, KeyCode.T },
		new KeyCode[]{ KeyCode.Alpha9, KeyCode.P, KeyCode.O, KeyCode.I, KeyCode.Alpha8 }
	};
	public enum Dir{Up, Right, Down, Left, Fire}

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
		if (controlEnabled) {
			// Forward acceleration
			if (keys != null && Input.GetKey (keys [(int)Dir.Up])) {
					GetComponent<Rigidbody2D>().AddForce (fwd * accelForce * controlMod);
			}
			if (keys != null && Input.GetKey (keys [(int)Dir.Down])) {
					GetComponent<Rigidbody2D>().AddForce (-fwd * accelForce * brakePower * controlMod);
			}

			// Handle turning
			if (Mathf.Abs (GetComponent<Rigidbody2D>().angularVelocity) < maxAngularVel * controlMod) {
					if (keys != null && Input.GetKey (keys [(int)Dir.Left])) {
							GetComponent<Rigidbody2D>().angularVelocity += accelTorque * controlMod;
					} else if (keys != null && Input.GetKey (keys [(int)Dir.Right])) {
							GetComponent<Rigidbody2D>().angularVelocity -= accelTorque * controlMod;
					}
			} else {
					GetComponent<Rigidbody2D>().angularVelocity = maxAngularVel * controlMod * Mathf.Sign (GetComponent<Rigidbody2D>().angularVelocity);
			}

			// Stop turning when: no turn keys are pressed or both are pressed
			if (keys != null && (!Input.GetKey (keys [(int)Dir.Right]) && !Input.GetKey (keys [(int)Dir.Left]))
			    || Input.GetKey (keys [(int)Dir.Right]) && Input.GetKey (keys [(int)Dir.Left])) {
					GetComponent<Rigidbody2D>().angularVelocity = 0F;
			}

			// Use an item
			if (keys != null && Input.GetKeyDown (keys [(int)Dir.Fire])) {
					useNextItem ();
			}
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
