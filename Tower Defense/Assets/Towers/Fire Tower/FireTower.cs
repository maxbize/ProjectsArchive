using UnityEngine;
using System.Collections.Generic;

/*
 * Damage:   Medium (upgradeable)
 * Range:    Short
 * Speed:    Very High (10 Hz)
 * Effect:   Attacks multiple targets (upgradeable)
 * Effect:   Attacks deal damage over time (upgradeable)
 * Hits air: No
 */
public class FireTower : Tower
{

    GameObject cloneableFireProjectile;
    GameObject cloneableFireDOT;
    float fireProjAnimTime;
    float DOTdamage = 20; // DPS of DOT (lasts 3 seconds)
    int maxTargets = 1;
    int DOTlevel = 1;

    // Since the projectile is a particle animation and we attack constantly, rather than spawn a new
    //  system every attack, just keep one per enemy and keep it pointed correctly
    List<Enemy> enemiesInRange = new List<Enemy>();
    List<GameObject> fireProjectiles = new List<GameObject>();

    protected override void Start()
    {
        damage = 10f;
        attackSpeed = 0.0f;
        minRange = 0.1f;
        maxRange = 150f;

        cloneableFireProjectile = GameObject.Find("Cloneable_FireProjectile");
        cloneableFireDOT = GameObject.Find("Cloneable_FireDOT");
        fireProjAnimTime = cloneableFireProjectile.GetComponent<ParticleSystem>().startLifetime;

        generalStr = "Rapidly fires at multiple targets simultaneously";
        damageStr = "Medium";
        speedStr = "Very High";
        rangeStr = "Short";
        effectStr = "Attacks multiple";

        effectMultiplier = 2;

        type = type_t.fireTower;

        base.Start();
    }

    protected override void Update()
    {
        // Remove enemies that have left / died and update fire rotation + attack for those that haven't
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            if (!enemiesInRange[i] || (enemiesInRange[i].transform.position - transform.position).magnitude > maxRange)
            {
                enemiesInRange.RemoveAt(i);
                fireProjectiles[i].GetComponent<ParticleSystem>().emissionRate = 0;
                Destroy(fireProjectiles[i], fireProjAnimTime);
                fireProjectiles.RemoveAt(i);
            }
            else
            {
                Quaternion lookRotation = new Quaternion();
                lookRotation.SetLookRotation(enemiesInRange[i].transform.position - transform.position);
                fireProjectiles[i].transform.rotation = lookRotation;
                Attack(enemiesInRange[i]);
            }
        }

        // Find new enemies to attack and initiate attack
        if (attackEnabled)
        {
            if (timer > attackSpeed)
            {
                for (int i = enemies.Count -1; i >= 0; i--)
                {
                    Enemy enemy = enemies[i];
                    float dist = (enemy.transform.position - transform.position).magnitude;
                    if (dist > minRange && dist < maxRange && enemiesInRange.Count < maxTargets)
                    {
                        Attack(enemy);
                        timer = 0;
                    }
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }
    
    protected override void Attack(Enemy enemy)
    {
        if (!enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
            GameObject fire = (GameObject)GameObject.Instantiate(cloneableFireProjectile);
            Quaternion lookRotation = new Quaternion();
            lookRotation.SetLookRotation(enemy.transform.position - transform.position);
            fire.transform.rotation = lookRotation;
            fire.transform.position = transform.position;
            fireProjectiles.Add(fire);

            // Set the DOT on the enemy: If they already have one, just re-init it. Else add one
            if (enemy.fireDOT)
            {
                enemy.fireDOT.timeAlive = 0;
                if (enemy.fireDOT.dps < DOTdamage)
                {
                    enemy.fireDOT.dps = DOTdamage;
                }
            }
            else
            {
                GameObject fireDOTanim = (GameObject)GameObject.Instantiate(cloneableFireDOT);
                FireDOT fireDOT = fireDOTanim.AddComponent<FireDOT>();
                enemy.fireDOT = fireDOT;
                fireDOT.Init(enemy, DOTdamage, fireDOTanim);
                fireDOTanim.transform.parent = enemy.gameObject.transform;
                fireDOTanim.transform.localPosition = Vector3.zero;
            }
        }
        enemy.registerHit(damage * Time.deltaTime);
    }

    public void UpgradeMaxTargets()
    {
        maxTargets++;
        effectMultiplier += effectPerLevel;
    }

    public void UpgradeDOT()
    {
        DOTdamage *= 1.2f;
        DOTlevel++;
    }

    public override void PopulateUpgradeMenu(GUIManager manager)
    {
        manager.AddUpgrade("Damage: ", damageLevel, CalcUpgradeCost_Damage(), UpgradeDamage);
        manager.AddUpgrade("Targets: ", maxTargets, CalcUpgradeCost_Targets(), UpgradeMaxTargets);
        manager.AddUpgrade("Damage Over Time: ", DOTlevel, CalcUpgradeCost_DoT(), UpgradeDOT);
    }

    protected int CalcUpgradeCost_Targets()
    {
        float currentScore = CalcScore();
        effectMultiplier += effectPerLevel;
        float deltaScore = CalcScore() - currentScore;
        effectMultiplier -= effectPerLevel;
        return ScoreToCost(deltaScore);
    }

    protected int CalcUpgradeCost_DoT()
    {
        float currentScore = CalcScore();
        float deltaScore = CalcScore() - currentScore;
        return ScoreToCost(deltaScore);
    }

    protected override float CalcDPS()
    {
        return (damage * maxTargets);
    }
}
