using UnityEngine;
using System.Collections;

public class EntityFactory : MonoBehaviour {

    // Set in editor
    public GameObject rocketPrefab;
    public GameObject dronePrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnRocket(Vector3 origin, Quaternion rotation, bool attackHero) {
        GameObject rocket = (GameObject)Instantiate(rocketPrefab, origin, rotation);
        rocket.GetComponent<Rocket>().Init(attackHero);
    }

    public void SpawnDrone(Vector3 origin, Quaternion rotation) {
        Instantiate(dronePrefab, origin, rotation);
    }
}
