using UnityEngine;
using System.Collections;
using System.IO;
using System;

/* 
 * This class creates a recording of a single car's play. It records time, position, and orientation.
 */
public class GhostRecorder : MonoBehaviour {

	public static int bytesPerFrame { private set; get; }
	public static int floatsPerFrame {private set; get; }

    int baseSeconds;
    int framesPerSec;

	MemoryStream recording;

	public const int keyFrameRate = 4; // Take a key frame every keyFrameRate frames

	void Start() {
		baseSeconds = 5 * 60; // Base number of minutes to record. Expandable at runtime
		framesPerSec = Mathf.RoundToInt(1.0F / Time.fixedDeltaTime); // Should be an average

		recording = new MemoryStream(FrameState.size * framesPerSec * baseSeconds);
	}

	public void addFrame(FrameState frameState) {
		if (recording != null) {
			recording.Write(frameState.toBuffer(), 0, FrameState.size);
		}
	}

	// We don't want to give someone direct access to the recording stream. Otherwise we might 
	//  inter-mix read and write ops
	public byte[] getRecordingCopy() {
		//Debug.Log (recording.ToArray ().Length);
        return recording.ToArray();
	}

    public void restartRecording() {
        recording.SetLength(0); // Resets the stream

        // Can't do below. It doesn't work like the constructor and we get a bunch of zeros
        //recording.SetLength(bytesPerFrame * framesPerSec * baseSeconds); // Don't want to waste time with dynamic memory allocation
    }
}
