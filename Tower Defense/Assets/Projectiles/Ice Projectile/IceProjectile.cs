using UnityEngine;
using System.Collections;


public class IceProjectile : MonoBehaviour
{
    public float damage;
    float speed;
    Enemy targetEnemy;

    public void Init(float speed, Enemy target, float damage)
    {
        this.damage = damage;
        this.targetEnemy = target;
        this.speed = speed;
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetEnemy)
        {
            rigidbody.velocity = (targetEnemy.transform.position - transform.position).normalized * speed;
            Quaternion rotation = new Quaternion(); // Apparently theres some Unity bug that doesn't allow you to setLookRotation directly
            rotation.SetLookRotation(targetEnemy.transform.position - transform.position);
            transform.rotation = rotation;
        }
        if (transform.position.magnitude > 1000)// || transform.position.y < -40)
        {
            print("Projectile passed boundaries!");
            Destroy(gameObject);
        }
    }
     
    void OnTriggerEnter(Collider collider)
    {
        transform.position = new Vector3(0, -39, 0);
        speed = 0;
        rigidbody.velocity = Vector3.zero; // Even though we just set the speed to zero, if the enemy has died the Update will not stop the projectile
        Invoke("DelayedDestroy", 2.0f);
        if (collider.isTrigger && collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Enemy collidedEnemy = collider.transform.parent.GetComponent<Enemy>();
            if (collidedEnemy)
            {
                collidedEnemy.registerHit(damage);
            }
        }
    }

    void DelayedDestroy()
    {
        Destroy(gameObject);
    }
}
