using UnityEngine;
using System.Collections;

// Handles actions related to the player attacking
public class PlayerAttack : MonoBehaviour {

    // Set in editor
    public float attackSpeed;
    public float rightOffset;
    public float forwardOffset;

    private float timeSinceLastAttack = 0;
    private bool attackRight;
    [HideInInspector]
    public bool attackSelf = false;
    [HideInInspector]
    public bool canAttack = true;

    private EntityFactory entityFactory;

	// Use this for initialization
	void Start () {
        entityFactory = FindObjectOfType<EntityFactory>();
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLastAttack += Time.deltaTime;

        if (Input.GetMouseButton(0) && timeSinceLastAttack > attackSpeed && canAttack) {
            Attack();
        }
	}

    // Public for StoryManager
    public void Attack() {
        Vector3 spawn = transform.position + transform.forward * forwardOffset;
        spawn += transform.right * rightOffset * (attackRight ? 1 : -1);
        attackRight = !attackRight;
        entityFactory.SpawnRocket(spawn, transform.rotation, attackSelf);
        timeSinceLastAttack = 0;
    }
}
