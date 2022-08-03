using UnityEngine;
using System.Collections;

public class LaserProjectile : MonoBehaviour
{
    float damage;
    float lifespan; // indicates that the object has not been copied from the pool yet
    float timeAlive;

    public void Init(float damage, float lifespan)
    {
        this.damage = damage;
        this.lifespan = lifespan;
        timeAlive = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /* Goals:
         * - Expand until full size (quickly)
         * - Check for collisions against enemies and damage them 
         * - Wait until beam duration has passed
         * - Shrink until zero size (quickly and die)
         * - Handle special effects
         */
        if (timeAlive > lifespan)
        {
            Destroy(gameObject);
        }
        timeAlive += Time.deltaTime;
    }

    void OnTriggerStay(Collider collider)
    {
        Enemy collidedEnemy = collider.transform.parent.GetComponent<Enemy>();
        if (collidedEnemy)
        {
            collidedEnemy.registerHit(damage * Time.deltaTime);
        }
        else
        {
            print("Laser Projectile hit NULL enemy!!");
        }
    }
}
