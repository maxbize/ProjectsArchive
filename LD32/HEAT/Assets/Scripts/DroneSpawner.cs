using UnityEngine;
using System.Collections;

// Periodically spawns drones. Boring
public class DroneSpawner : MonoBehaviour {

    // Set in editor
    public GameObject leftCorner;
    public GameObject rightCorner;

    [HideInInspector]
    public float minSpawnTime;
    [HideInInspector]
    public float maxSpawnTime;

    private float nextSpawnTime;
    private float timeSinceLastSpawn;

    private EntityFactory entityFactory;

	// Use this for initialization
	void Start () {
        entityFactory = FindObjectOfType<EntityFactory>();
        SetNextSpawnTime();
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn > nextSpawnTime) {
            SetNextSpawnTime();
            SpawnDrone();
        }
	}

    private void SetNextSpawnTime() {
        timeSinceLastSpawn = 0;
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnDrone() {
        Vector3 cornerToCorner = leftCorner.transform.position - rightCorner.transform.position;
        Vector3 spawnPoint = rightCorner.transform.position + cornerToCorner.normalized * Random.Range(0, cornerToCorner.magnitude);
        entityFactory.SpawnDrone(spawnPoint, Quaternion.LookRotation(-Vector3.forward));
    }
}
