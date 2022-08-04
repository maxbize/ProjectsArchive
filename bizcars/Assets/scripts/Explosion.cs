using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    public float blastForce = 250;
	float blastRadius;

	void Start() {
        blastRadius = gameObject.GetComponent<CircleCollider2D>().radius;
		Destroy (gameObject, 0.05f);
	}

    void Update() {

    }

    // The force on the player goes between (0.5 to 1) * blastForce depending on the distance to the blast
	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
            Vector2 toPlayer = other.transform.position - transform.position;
            float force = (blastRadius - toPlayer.magnitude) / 2 / blastRadius * blastForce;
            if (force < 0) {
                force = 0;
            }
            force += blastForce / 2;
            //Debug.Log(force);
            other.GetComponent<Rigidbody2D>().AddForce(toPlayer.normalized * force);
		}
	}
}
