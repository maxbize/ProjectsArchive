using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public bool isStuck;
    public float radius;

    // For brick hit rotations
    private Vector3 oldVelocity;

    // PaddleManager reference for paddle positions
    private PaddleManager padManagerRef;

    // LevelManager reference for assembly removal
    private LevelManager levelManagerRef;

    void Start()
    {
        isStuck = true;

        padManagerRef = GameObject.Find("Paddle").GetComponent<PaddleManager>();
        levelManagerRef = GameObject.Find("Level Manager").GetComponent<LevelManager>();

        Rigidbody rb = (Rigidbody)gameObject.AddComponent("Rigidbody");
        rb.useGravity = false;
        rb.angularDrag = 0.25f;

        SphereCollider sc = (SphereCollider)gameObject.AddComponent("SphereCollider");
        sc.material = (PhysicMaterial)Resources.Load("Ball PMat");

        gameObject.renderer.material.shader = Shader.Find("Specular");
        gameObject.renderer.material.color = Color.black;

        radius = gameObject.GetComponent<SphereCollider>().radius;
    }

    void Update() 
    {
        if (isStuck)
        {
            gameObject.transform.position = padManagerRef.GetBallStuckPosition(radius);
            gameObject.transform.rotation = padManagerRef.transform.rotation;
            if (Input.GetButtonDown("Fire1")) // Fire1 should have good keymapping for what we want
            {
                isStuck = false;
                gameObject.rigidbody.velocity = padManagerRef.GetCenterToCentroid().normalized 
                    * LevelManager.GetLevelSpeed();
                oldVelocity = gameObject.rigidbody.velocity;
            }
        }
        if ((transform.position - LevelManager.GetCentroid()).magnitude > 
            padManagerRef.GetCenterToCentroid().magnitude + padManagerRef.thickness * 10)
        {
            isStuck = true;
        }
    }

    // FixedUpdate is used for Physics stuff
    //void FixedUpdate()
    //{
    //    rigidbody.AddForce(Vector3.Cross(rigidbody.angularVelocity, rigidbody.velocity) / 30f);
    //}

    void OnCollisionEnter(Collision collision)
    {
        Vector3 unitForward = padManagerRef.GetCenterToCentroid().normalized;
        if (collision.gameObject.name.IndexOf("Paddle") != -1)
        {
            gameObject.rigidbody.velocity = unitForward * LevelManager.GetLevelSpeed();
            rigidbody.angularVelocity = padManagerRef.rotationVect * 10f;
        }
        else if (collision.gameObject.name.IndexOf("Brick") != -1)
        {
            gameObject.rigidbody.velocity = gameObject.rigidbody.velocity.normalized * LevelManager.GetLevelSpeed();
            levelManagerRef.RemoveAssembly(collision.gameObject.GetComponent<Brick>().levelPosition);
        }
    }
}
