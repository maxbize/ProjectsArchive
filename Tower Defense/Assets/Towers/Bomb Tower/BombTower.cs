using UnityEngine;
using System.Collections.Generic;

/*
 * Damage:   Medium (upgradable)
 * Range:    Very High (but also has a very high min range)
 * Speed:    Medium (upgradeable)
 * Effect:   Splash
 * Hits air: Yes
 * Notes:    - Accuracy is low (upgradeable)
 *           - Tower can attack anywhere on map, but must see target in correct range first
 */
public class BombTower : Tower
{

    float splash = 100f;
 //   float inaccuracy = 200f;
    int accuracyLevel = 1;
    float theta = 70 * Mathf.Deg2Rad;

    // For graphics
    ParticleSystem[] launchEffects;

    // Use this for initialization
    protected override void Start()
    {
        launchEffects = GetComponentsInChildren<ParticleSystem>();
        damage = 90f;
        attackSpeed = 4.0f;
        minRange = 400f;
        maxRange = 500f;

        generalStr = "Shoots bombs at the enemies from far away";
        damageStr = "Medium";
        speedStr = "Medium";
        rangeStr = "Very High";
        effectStr = "Splash";

        effectMultiplier = 0.5f;
        effectPerLevel = 1.2f;

        type = type_t.bombTower;

        UpdateUpgradeCost();

        base.Start();
    }

    protected override void Update()
    {
        // Face the enemy for visual effect
        // This might seem like a waste, but base will only parse through the enemies
        //   when the tower can attack, so it's not actually that bad
        foreach (Enemy enemy in enemies)
        {
            float dist = (enemy.transform.position - transform.position).magnitude;
            if (dist > minRange && dist < maxRange)
            {
                Vector3 to = enemy.transform.position - transform.position;
                float angle = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg + 90;
                Vector3 newAngles = new Vector3(0, angle, 90 - theta * Mathf.Rad2Deg);
                transform.eulerAngles = newAngles;
                break;
            }
        }

        base.Update();
    }

    protected override void Attack(Enemy enemy)
    {
        GameObject proj = (GameObject)GameObject.Instantiate(GameObject.Find("Cloneable_BombProjectile"));
        proj.transform.position = transform.position + new Vector3(0, 20f, 0) - transform.right * 5;

        BombProjectile obj = proj.AddComponent<BombProjectile>();
        obj.Init(splash, solveProjectileVelocity(enemy), damage, enemies);

        foreach (ParticleSystem effect in launchEffects)
        {
            effect.Play();
        }

        if (enemy.transform.position == Vector3.zero)
        {
            print(" *** BUG!!! This bug was fixed by making LevelManager LateUpdate");
        }
    }

    // Since the tower does splash, factor in a bonus to the DPS
    protected override float CalcDPS()
    {
        float splashWeight = 2.0f;
        return splashWeight * base.CalcDPS();
    }

    // Solve the velocity for the trajectory
    // Note that this algorithm assumes that the bomb will hit the target in the downward portion of the trajectory
    Vector3 solveProjectileVelocity(Enemy enemy)
    {
        //Vector3 offset = Random.insideUnitSphere * inaccuracy;
        //offset.y = 0;
        float w; // Distance along xy plane
        float t = 0f; // Time to travel
        float g = Physics.gravity.magnitude; // y acceleration
        float vx, vy, vz; // x,y,z components of velocity
        Vector3 target = enemy.transform.position;
        Vector3 to = Vector3.zero; // Vector from origin to target

        /*
         * Iterate:
         *   - Get time to launch to target
         *   - Forecast enemy posisition for that timestep
         *   - Make the forecast position the new target
         */
        for (int i = 0; i < accuracyLevel; i++)
        {
            to = target - transform.position;
            w = Mathf.Sqrt(to.x * to.x + to.z * to.z);
            t = Mathf.Sqrt((2 / (-g)) * (-to.y - w * Mathf.Tan(theta)));
            Vector3 forecast = enemy.forecastPosition(t);
            if ((forecast - target).magnitude < 1f)
            {
                break; // No point in continuing, the error is so mall already...
            }
            target = forecast;
        }

        vx = to.x / t;
        vy = (g * t * t + 2 * to.y) / (2 * t);
        vz = to.z / t;

        return new Vector3(vx, vy, vz);
    }

    public void UpgradeAccuracy()
    {
        accuracyLevel += 1;
        effectMultiplier *= effectPerLevel;
    }

    public override void PopulateUpgradeMenu(GUIManager manager)
    {
        manager.AddUpgrade("Damage: ", damageLevel, CalcUpgradeCost_Damage(), UpgradeDamage);
        manager.AddUpgrade("Speed: ", speedLevel, CalcUpgradeCost_Speed(), UpgradeSpeed);
        manager.AddUpgrade("Accuracy: ", accuracyLevel, CalcUpgradeCost_Effect(), UpgradeAccuracy);
    }

    // 1/23/14: let's streamline stuff. Combine the three upgrades into one
    public override void Upgrade()
    {
        UpgradeDamage();
        UpgradeSpeed();
        UpgradeAccuracy();
        UpdateUpgradeCost();
    }
    protected override void UpdateUpgradeCost()
    {
        upgradeCost = CalcUpgradeCost_Damage() + CalcUpgradeCost_Speed() + CalcUpgradeCost_Effect();
    }
    
}
