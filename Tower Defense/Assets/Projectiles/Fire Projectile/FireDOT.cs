using UnityEngine;
using System.Collections;

public class FireDOT : MonoBehaviour
{
    float burnTime = 3.0f;
    public float timeAlive;
    public float dps;

    ParticleSystem fireAnimPS;
    Enemy target;

    // Sets the target and initializes the countdown
    public void Init(Enemy target, float dps, GameObject fireAnim)
    {
        this.target = target;
        this.dps = dps;
        timeAlive = 0f;
        this.fireAnimPS = fireAnim.GetComponentInChildren<ParticleSystem>();

        this.transform.localScale = Vector3.one; // Should scale to parent (test this)
    }

    // Update is called once per frame
    void Update()
    {
        if (timeAlive < burnTime)
        {
            timeAlive += Time.deltaTime;

            if (target)
            {
                target.registerHit(dps * Time.deltaTime);
            }

            if (timeAlive > burnTime)
            {
                fireAnimPS.emissionRate = 0;
                Destroy(gameObject, fireAnimPS.startLifetime);
            }
        }
    }
}
