using UnityEngine;
using System.Collections.Generic;

public class BombProjectile : MonoBehaviour
{

    float splash;
    float damage;
    List<Enemy> enemies; // Needed for splash damage

    float deathDelay;

    public void Start()
    {
        deathDelay = 0f;
        ParticleSystem[] partSystems = (ParticleSystem[])GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in partSystems)
        {
            if (ps.duration > deathDelay)
            {
                deathDelay = ps.duration;
            }
        }
    }

    public void Init(float splash, Vector3 velocity, float damage, List<Enemy> enemies)
    {
        this.splash = splash;
        this.damage = damage;
        this.enemies = enemies;

        GetComponent<Rigidbody>().velocity = velocity;
        GetComponent<Rigidbody>().useGravity = true;

        GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 500); // Visual effect
    }

    void Update()
    {
        if (transform.position.magnitude > 1000 || transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter()
    {
        GameObject explosion = (GameObject)GameObject.Instantiate(GameObject.Find("Cloneable_Explosion"));
        explosion.transform.position = transform.position;
        explosion.AddComponent<AutoDestroy>();

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemies[i];
            if ((enemy.transform.position - transform.position).magnitude < splash)
            {
                enemy.registerHit(damage);
            }
        }

        transform.position = new Vector3(0, -80, 0);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().useGravity = false;

        Invoke("DelayedDestroy", deathDelay);
    }

    void DelayedDestroy()
    {
        Destroy(gameObject);
    }

}
