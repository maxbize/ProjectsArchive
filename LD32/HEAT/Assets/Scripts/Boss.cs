using UnityEngine;
using System.Collections.Generic;
using System;

public class Boss : MonoBehaviour {

    // Set in editor
    public int health;
    public float timeBetweenAttacks;
    public float attackStallTime;
    public float acceleration;
    public float centerAttackSpinRate;
    public float centerAttackDuration;
    public float sprayAttackDuration;
    public float sprayRandomness;
    public float timeBetweenSprayDrones;
    public float timeBetweenCenterDrones;
    public float timeBetweenSideDrones;
    public GameObject TrCorner;
    public GameObject TlCorner;
    public GameObject BrCorner;
    public GameObject BlCorner;
    public float timeBetweenHeroMeleeDamage;
    public GameObject damageEffect;
    public GameObject healthEffect;
    public int numHealthEffects;
    public GameObject explosionEffect;
    public float spinRate;

    private enum AttackMode
    {
        SIDE,   // Flies down one of the sides shooting drones straight at a right angle
        CENTER, // Sits in the center, spinning and shooting drones straight out of the corners
        SPRAY   // Goes to a random spot and shoots a bunch of drones towards the user (slightly random angle)
    }
    private AttackMode currentAttackMode;

    public enum Mode
    {
        MOVING_TO_ATTACK_TARGET,
        ATTACKING,
        ATTACK_DONE,
        STORY
    }
    public Mode currentMode = Mode.STORY; // Public for storyManager

    public Vector3 moveTarget; // Public for storyManager
    public Vector3 lookTarget; // public for storyManager
    private float timeSinceLastAttack;
    private float timeSinceAttackStart;
    private bool sideAttackFirstCornerReached;
    private float timeSinceLastDroneSent;
    private float shootOffsetMultiplier = 0.6f;
    private float accelStore;
    public int healthRemaining; // Public for sotryManager
    private float hurtHeroTimer;
    private List<GameObject> healthEffects = new List<GameObject>();

    private Rigidbody myRb;
    private EntityFactory entityFactory;
    private GameObject hero;
    
	// Use this for initialization
	void Start () {
        myRb = GetComponent<Rigidbody>();
        entityFactory = FindObjectOfType<EntityFactory>();
        hero = FindObjectOfType<PlayerMovement>().gameObject;
        Init();
	}

    public void Init() {
        healthRemaining = health;
        accelStore = acceleration;
        timeSinceLastDroneSent = 0;
        sideAttackFirstCornerReached = false;
        timeSinceAttackStart = 0;
        timeSinceLastAttack = 0;
        lookTarget = Vector3.forward;
        foreach (GameObject effect in healthEffects) {
            Destroy(effect);
        }
        healthEffects.Clear();
    }
	
	// Update is called once per frame
	void Update () {
        hurtHeroTimer += Time.deltaTime;
        switch (currentMode) {
            case Mode.ATTACK_DONE:
                timeSinceLastAttack += Time.deltaTime;
                if (timeSinceLastAttack > timeBetweenAttacks + attackStallTime) {
                    timeSinceLastAttack = 0;
                    SelectNextAttackMode();
                    UpdateTargetPosition();
                    currentMode = Mode.MOVING_TO_ATTACK_TARGET;
                }
                break;
            case Mode.MOVING_TO_ATTACK_TARGET:
                Move();
                if (ReachedTarget()) {
                    currentMode = Mode.ATTACKING;
                }
                break;
            case Mode.ATTACKING:
                timeSinceAttackStart += Time.deltaTime;
                timeSinceLastDroneSent += Time.deltaTime;
                HandleAttackMode();
                Move();
                break;
            case Mode.STORY:
                Move();
                break;
        }
	}

    private void HandleAttackMode() {
        switch (currentAttackMode) {
            case AttackMode.CENTER:
                lookTarget = Quaternion.AngleAxis(Time.deltaTime * centerAttackSpinRate, Vector3.up) * lookTarget;
                ShootDronesDiagonally();
                if (timeSinceAttackStart > centerAttackDuration) {
                    timeSinceAttackStart = 0;
                    currentMode = Mode.ATTACK_DONE;
                }
                break;
            case AttackMode.SIDE:
                if (ReachedTarget()) {
                    if (!sideAttackFirstCornerReached) {
                        sideAttackFirstCornerReached = true;
                        UpdateTargetPosition();
                        SetLookTargetToMoveTarget();
                        acceleration = 0;
                    }
                    else {
                        sideAttackFirstCornerReached = false;
                        timeSinceAttackStart = 0;
                        currentMode = Mode.ATTACK_DONE;
                    }
                }
                else if (sideAttackFirstCornerReached) {
                    ShootDronesSideways();
                }
                if (timeSinceAttackStart > attackStallTime) {
                    acceleration = accelStore;
                }
                break;
            case AttackMode.SPRAY:
                lookTarget = hero.transform.position - transform.position;
                ShootDronesSpray();
                if (timeSinceAttackStart > sprayAttackDuration + attackStallTime) {
                    timeSinceAttackStart = 0;
                    currentMode = Mode.ATTACK_DONE;
                }
                break;
        }
    }

    private void ShootDronesSideways() {
        if (timeSinceLastDroneSent > timeBetweenSideDrones && timeSinceAttackStart > attackStallTime) {
            timeSinceLastDroneSent = 0;
            int direction = 1;
            if (Vector3.Cross(moveTarget, transform.position).y > 0) {
                direction = -1;
            }
            entityFactory.SpawnDrone(transform.position + transform.right * transform.localScale.x * direction * shootOffsetMultiplier,
                Quaternion.LookRotation(transform.right * direction));
        }
    }

    private void ShootDronesSpray() {
        if (timeSinceLastDroneSent > timeBetweenSprayDrones && timeSinceAttackStart > attackStallTime) {
            timeSinceLastDroneSent = 0;
            Vector3 droneDir = Quaternion.AngleAxis(Time.deltaTime * centerAttackSpinRate, Vector3.up) * transform.forward;
            droneDir += transform.right * UnityEngine.Random.Range(-sprayRandomness, sprayRandomness);
            entityFactory.SpawnDrone(transform.position + droneDir + transform.forward * transform.localScale.x * shootOffsetMultiplier,
                Quaternion.LookRotation(droneDir));
        }
    }

    private void ShootDronesDiagonally() {
        if (timeSinceLastDroneSent > timeBetweenCenterDrones && timeSinceAttackStart > attackStallTime) {
            timeSinceLastDroneSent = 0;
            Vector3 droneDir = transform.forward + transform.right;
            for (int i = 0; i < 4; i++) {
                droneDir = Quaternion.AngleAxis(90, Vector3.up) * droneDir;
                entityFactory.SpawnDrone(transform.position + droneDir * transform.localScale.x * shootOffsetMultiplier,
                    Quaternion.LookRotation(droneDir));
            }
        }
    }

    private bool ReachedTarget() {
        // Really terrible way to check
        if ((moveTarget - transform.position).magnitude < 1) {
            return true;
        }
        return false;
    }

    // Translation & rotation
    private void Move() {
        if (!ReachedTarget()) {
            Vector3 force = (moveTarget - transform.position).normalized * acceleration;
            myRb.AddForce(force);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookTarget), spinRate);
    }

    private void SelectNextAttackMode() {
        //Array enums = Enum.GetValues(typeof(AttackMode));
        //currentAttackMode = (AttackMode)enums.GetValue(UnityEngine.Random.Range(0, enums.Length));
        
        // Cheap way to get some weighted randoms
        List<AttackMode> enums = new List<AttackMode>() { AttackMode.CENTER, AttackMode.CENTER, AttackMode.SPRAY, AttackMode.SPRAY, AttackMode.SIDE };
        currentAttackMode = enums[UnityEngine.Random.Range(0, enums.Count)];
    }

    private void UpdateTargetPosition() {
        switch (currentAttackMode) {
            case AttackMode.CENTER:
                moveTarget = Vector3.zero;
                break;
            case AttackMode.SIDE:
                if (sideAttackFirstCornerReached) {
                    PickSecondSideAttackCorner();
                }
                else {
                    PickFirstSideAttackCorner();
                }
                break;
            case AttackMode.SPRAY:
                moveTarget = UnityEngine.Random.insideUnitSphere * TrCorner.transform.position.x;
                moveTarget.y = 0;
                break;
        }
        SetLookTargetToMoveTarget();
    }

    private void SetLookTargetToMoveTarget() {
        lookTarget = moveTarget - transform.position;
    }

    private void PickFirstSideAttackCorner() {
        List<Vector3> saneCorners = new List<Vector3>() { TrCorner.transform.position, TlCorner.transform.position,
            BrCorner.transform.position, BlCorner.transform.position };
        Vector3 closest = TrCorner.transform.position;
        foreach (Vector3 corner in saneCorners) {
            if ((corner - transform.position).sqrMagnitude <
                (closest - transform.position).sqrMagnitude) {
                closest = corner;
            }
        }
        saneCorners.Remove(closest);
        moveTarget = saneCorners[UnityEngine.Random.Range(0, saneCorners.Count)];
    }

    private void PickSecondSideAttackCorner() {
        List<Vector3> corners = new List<Vector3>() { TrCorner.transform.position, TlCorner.transform.position,
            BrCorner.transform.position, BlCorner.transform.position };
        Vector3 closest = TrCorner.transform.position;
        Vector3 farthest = TrCorner.transform.position;
        foreach (Vector3 corner in corners) {
            if ((corner - transform.position).sqrMagnitude >
                (farthest - transform.position).sqrMagnitude) {
                    farthest = corner;
            }
            else if ((corner - transform.position).sqrMagnitude <
                (closest - transform.position).sqrMagnitude) {
                    closest = corner;
            }
        }
        corners.Remove(closest);
        corners.Remove(farthest);
        moveTarget = corners[UnityEngine.Random.Range(0, corners.Count)];
    }

    void OnCollisionEnter(Collision other) {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        if (player != null && hurtHeroTimer > timeBetweenHeroMeleeDamage) {
            hurtHeroTimer = 0;
            player.RegisterHit(gameObject);
        }
        Rocket rocket = other.gameObject.GetComponent<Rocket>();
        if (rocket != null && rocket.activated) {
            Instantiate(damageEffect, other.contacts[0].point, Quaternion.LookRotation(other.contacts[0].point - transform.position));
            RegisterHit();
        }
    }

    // This is bad code. Boss and Drone are very similar! Don't repeat my hacks :)
    public void RegisterHit() {
        healthRemaining--;
        if ((float)(health - healthRemaining) / (float)health * (float)numHealthEffects > (healthEffects.Count + 1)) {
            SpawnHealthEffect(1);
        }
        if (healthRemaining <= 0) {
            Die();
        }
    }

    private void SpawnHealthEffect(float lifetimeMultiplier) {
        Vector3 pos = UnityEngine.Random.onUnitSphere * transform.localScale.x * 0.6f + transform.position;
        if (pos.y < 0) {
            pos.y = 0;
        }
        Quaternion rot = Quaternion.LookRotation(pos - transform.position);
        GameObject effect = (GameObject)Instantiate(healthEffect, pos, rot);
        effect.GetComponent<ParticleSystem>().startLifetime *= lifetimeMultiplier;
        effect.transform.parent = transform;
        healthEffects.Add(effect);
    }

    private void Die() {
        for (int i = 0; i < 5; i++) {
            SpawnHealthEffect(2);
        }
        //Destroy(gameObject); // Let sotryManager handle it
    }

    public void Explode() {
        foreach (GameObject effect in healthEffects) {
            effect.transform.parent = null;
            effect.GetComponent<ParticleSystem>().loop = false;
            effect.AddComponent<ParticleSystemKiller>();
        }
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }
}
