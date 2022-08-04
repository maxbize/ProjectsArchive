using UnityEngine;
using System.Collections;

public class StartingPosition : MonoBehaviour {

	public int carNum = -1;

	// Use this for initialization
	void Start () {
		if (carNum == -1) {
			Debug.LogError("Error: SpawnPoint created without a number assigned");
		}
	}
}
