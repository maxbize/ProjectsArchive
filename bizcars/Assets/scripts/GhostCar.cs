using UnityEngine;
using System.Collections;
using System.IO;
using System;

/*
 * This class controls a ghost car. It takes a recording and updates the ghost's position 
 * and orientation accordingly.
 */ 
public class GhostCar : MonoBehaviour {

	public Car carToGhost;
	int frameIndex = 0;

	byte[] recording;

	float accumPlayTime = 0;
	Vector2 nextRecordPos;
	float nextRecordAngle;
	Vector2 lastRecordPos;
	float lastRecordAngle;

    FrameState lastFs;
    FrameState nextFs;

	float keyFrame_dt = GhostRecorder.keyFrameRate * Time.fixedDeltaTime;

	// Use this for initialization
    // Can't use start because the recording could get restarted before Start() gets called
	public void Init () {
		recording = carToGhost.GetComponent<GhostRecorder>().getRecordingCopy();

        lastFs = FrameState.fromBuffer(recording, FrameState.size * frameIndex);
        frameIndex++;
        nextFs = FrameState.fromBuffer(recording, FrameState.size * frameIndex);
        frameIndex++;
	}

	void Update () {
        if (frameIndex < recording.Length / FrameState.size) {

			accumPlayTime += Time.deltaTime;
			if (accumPlayTime > keyFrame_dt) {
				accumPlayTime -= keyFrame_dt;
                lastFs = nextFs;
                nextFs = FrameState.fromBuffer(recording, FrameState.size * frameIndex);
                frameIndex++;
            }

            // Lerp through two framestates using timing information
            Vector3 newPos = new Vector3();
			newPos.x = Mathf.Lerp(lastFs.pos_x, nextFs.pos_x, accumPlayTime / keyFrame_dt);
			newPos.y = Mathf.Lerp(lastFs.pos_y, nextFs.pos_y, accumPlayTime / keyFrame_dt);
            newPos.z = -0.1f; // To render above skid marks
			float newAngle = Mathf.LerpAngle(lastFs.angle, nextFs.angle, accumPlayTime / keyFrame_dt);
            gameObject.transform.position = newPos;
            gameObject.transform.eulerAngles = new Vector3(0, 0, newAngle);
        }
        else {
            Destroy(gameObject);
        }
	}

	public static void spawnGhost(Car car) {
		if (car.GetComponent<GhostRecorder> ().enabled) {
			GameObject ghost = (GameObject)GameObject.Instantiate (GameObject.Find ("Cloneable_GhostCar"));
			GhostCar ghostScript = ghost.AddComponent<GhostCar> ();
			ghostScript.carToGhost = car;
			Color carColor = car.GetComponent<SpriteRenderer> ().color;
			ghost.GetComponent<SpriteRenderer> ().color = new Color (carColor.r, carColor.g, carColor.b, 0.5F);
			ghostScript.Init ();
			car.GetComponent<GhostRecorder> ().restartRecording ();
		}
	}
}
