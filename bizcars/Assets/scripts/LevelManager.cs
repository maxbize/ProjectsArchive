using UnityEngine;
using System.Collections.Generic;

class LevelProgress
{
	public int lap;
	public int nextCheckPoint;
	public LevelProgress() {
		lap = 1;
		nextCheckPoint = 1;
	}
}

public class LevelManager : MonoBehaviour {

    List<ColoredCheckpoint> checkpoints = new List<ColoredCheckpoint>();

	// Dict of form (car, nextCheckpoint) 
	Dictionary<Car, LevelProgress> carsProgress = new Dictionary<Car, LevelProgress>();

	// Use this for initialization
	void Start () {
        GameObject[] cpObjs = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (GameObject cpObj in cpObjs) {
            checkpoints.Add(cpObj.GetComponent<ColoredCheckpoint>());
        }
	}

    // Call this when a car has passed a checkpoint. Will return true if this was the expected checkpoint.
    //  Note: current implementation purposely returns false when checkpoint is the last one or that one will just stay on!
	public bool recordCheckpoint(int checkpointID, Car car) {
		if (!carsProgress.ContainsKey (car)) {
			carsProgress.Add(car, new LevelProgress());
		}

		if (carsProgress [car].nextCheckPoint == checkpointID) {
			if (checkpointID == checkpoints.Capacity) {
				carsProgress[car].nextCheckPoint = 1;
				carsProgress[car].lap++;
                foreach(ColoredCheckpoint cp in checkpoints) {
                    cp.resetLight(car.playerNb);
                }
				GhostCar.spawnGhost(car); // debugging
				car.recordKeyFrame(); // debugging
			}
			else {
				carsProgress[car].nextCheckPoint = checkpointID + 1;
                return true;
			}
		}
        return false;
	}


}
