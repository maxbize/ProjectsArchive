using UnityEngine;
using System.Collections.Generic;

public class Missile : MonoBehaviour {

	public float speed = 5f;
    public int carNbToIgnore;

	public Transform explosionPrefab;

    void Start() {
        Vector2 fwd = new Vector2(-Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z),
                           Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z)).normalized;
        GetComponent<Rigidbody2D>().velocity = fwd * speed;
    }

	void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Untagged") {
			explode();
		}
        // Don't explode on the car that spawned the missile
        else if (other.tag == "Player" && other.GetComponent<Car>().playerNb != carNbToIgnore) {
            explode();
        }
        else if (other.tag == "Map Edge") {
            Destroy(gameObject, 1);
        }
        
        
	}

	public void explode() {
		Transform explosion = (Transform)Instantiate (explosionPrefab, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
