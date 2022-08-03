using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {

    // Set in editor
    public float moveAcceleration;
    public float lookAcceleration;
    public float launchVelocity;
    public float timeToActivation;
    public float explosionRadius;
    public float explosionForce;
    public float bounds;
    public GameObject deathExplosion;
    public AudioClip moveSound;

    private AudioSource myAs;
    private Rigidbody myRb;
    private GameObject target;
    private Vector3 moveInput = Vector3.zero;
    [HideInInspector]
    public bool activated = false;

    private float timeSinceSpawned = 0;
    private bool attackHero;

	// Use this for initialization
	void Start () {
        myRb = GetComponent<Rigidbody>();
        myAs = GetComponent<AudioSource>();
        myRb.velocity = transform.forward * launchVelocity + FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody>().velocity;
        moveInput = transform.forward * launchVelocity * 10;
        PlaySound(false);
	}

    public void Init(bool attackHero) {
        this.attackHero = attackHero;
    }

    private void FindTarget() {
        if (attackHero) {
            PlayerMovement hero = FindObjectOfType<PlayerMovement>();
            if (hero) {
                target = hero.gameObject;
            }
            else {
                target = null;
            }
        }
        else {
            GameObject closestEnemy = null;
            foreach (Drone enemy in FindObjectsOfType<Drone>()) {
                if (closestEnemy == null) {
                    closestEnemy = enemy.gameObject;
                }
                else if ((enemy.transform.position - transform.position).sqrMagnitude <
                    (closestEnemy.transform.position - transform.position).sqrMagnitude) {
                        closestEnemy = enemy.gameObject;
                }
            }
            target = closestEnemy;
        }
        if (target != null) {
            activated = true;
            GetComponentInChildren<ParticleSystem>().loop = true;
            GetComponentInChildren<ParticleSystem>().Play();
            PlaySound(false);
        }
    }

	// Update is called once per frame
	void Update () {
        timeSinceSpawned += Time.deltaTime;

        SeekTarget();
        Move();
        Rotate();

        if (transform.position.magnitude > bounds) {
            Destroy(gameObject);
        }
	}

    private void SeekTarget() {
        if (timeSinceSpawned < timeToActivation) {
            return;
        }

        if (target == null || !target.activeSelf) {
            FindTarget();
        }
    }

    private void Move() {
        if (target != null) {
            moveInput = transform.forward * moveAcceleration;
        }
        else if (!activated) {
            moveInput = transform.forward * moveAcceleration / 5;
        }
        myRb.AddForce(moveInput);
    }

    private void Rotate() {
        if (target != null) {
            Vector3 lookTarget = target.transform.position - transform.position;
            myRb.rotation = Quaternion.RotateTowards(myRb.rotation, Quaternion.LookRotation(lookTarget, transform.up), lookAcceleration);
        }
    }

    // TODO: Add onCollisionEnter for each class
    void OnCollisionEnter(Collision other) {
        if (activated) {
            Drone drone = other.gameObject.GetComponent<Drone>();
            if (drone != null) {
                drone.RegisterHit();
            }
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            if (player != null) {
                player.RegisterHit(gameObject);
            }
            Explode();
        }
    }

    private void Explode() {
        // This explosion feels very meh...
        /*
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders) {
            if (hit != null) {
                if (hit.GetComponent<Rigidbody>() != null) {
                    hit.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 3.0F);
                }
                if (hit.GetComponent<Drone>() != null) {
                    hit.GetComponent<Drone>().RegisterHit();
                }
            }
        }
         */
        GameObject explosion = (GameObject)Instantiate(deathExplosion, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystemKiller>().velocity = GetComponent<Rigidbody>().velocity / 3;
        Destroy(gameObject);
    }

    private void PlaySound(bool loop) {
        myAs.loop = loop;
        myAs.pitch = Random.Range(0.8f, 1.0f);
        myAs.Play();
    }
}
