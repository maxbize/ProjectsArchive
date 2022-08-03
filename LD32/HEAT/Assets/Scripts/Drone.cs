using UnityEngine;
using System.Collections;

// Enemy just just moves across the screen
public class Drone : MonoBehaviour {

    // Set in editor
    public int health;
    public float minSpeed;
    public float maxSpeed;
    public float bounds;
    public GameObject deathExplosion;

    private float speed;

	// Use this for initialization
	void Start () {
        speed = Random.Range(minSpeed, maxSpeed);
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
        GetComponent<AudioSource>().Play();        
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.magnitude > bounds) {
            Destroy(gameObject);
        }
	}

    public void RegisterHit() {
        health--;
        if (health <= 0) {
            Die();
        }
    }

    // TODO: Add onCollisionEnter for each class
    void OnCollisionEnter(Collision other) {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        if (player != null) {
            player.RegisterHit(gameObject);
            Die();
        }
    }

    private void Die() {
        GameObject explosion = (GameObject)Instantiate(deathExplosion, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystemKiller>().velocity = GetComponent<Rigidbody>().velocity / 3;
        Destroy(gameObject);
    }
}
