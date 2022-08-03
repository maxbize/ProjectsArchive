using UnityEngine;
using System.Collections.Generic;
using System;

public class Tower : MonoBehaviour
{
    protected List<Enemy> enemies;

    public int baseCost;
    public int upgradeCost { get; protected set; }
    protected int numKills;
    public float damage;
    protected float attackSpeed;
    public float minRange;
    public float maxRange;

    protected float timer = 0;
    protected bool attackEnabled = false;

    public enum type_t { none, iceTower, laserTower, bombTower, fireTower }
    public type_t type;
    public string generalStr, damageStr, speedStr, rangeStr, effectStr; // Used to describe tower in build preview

    // Need to keep track of how many times we've upgraded. Note there's a fourth upgrade per tower (special)
    protected int damageLevel = 1;
    protected int rangeLevel = 1;
    protected int speedLevel = 1;

    // This multiplier affects the cost of the tower based on the special ability
    protected float effectMultiplier = 1.0f; 

    // How much the upgrades increase per level (multiple)
    protected float rangePerLevel = 1.2f;
    protected float speedPerLevel = 1.2f;
    protected float damagePerLevel = 1.2f;
    protected float effectPerLevel = 1.2f;

    // Weight of each attribute when calculating tower score
    protected float rangeScoreWeight = 0.85f;
    protected float DPSScoreWeight = 1.15f;
    protected float effectWeight = 1.0f;
    
    // Use this for initialization
    protected virtual void Start()
    {
        enemies = GameObject.Find("Level Manager").GetComponent<LevelManager>().enemies;

        baseCost = ScoreToCost(CalcScore());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (attackEnabled)
        {
            if (timer > attackSpeed)
            {
                foreach (Enemy enemy in enemies)
                {
                    float dist = (enemy.transform.position - transform.position).magnitude;
                    if (dist > minRange && dist < maxRange)
                    {
                        Attack(enemy);
                        timer = 0;
                        break;
                    }
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    // Every tower has a unique attack that must be defined
    protected virtual void Attack(Enemy enemy)
    {
        throw new NotSupportedException(" *** ERROR: Virtual function must be overriden");
    }

    // Call this when the tower is built
    public virtual void Activate()
    {
        attackEnabled = true;
    }

    public virtual void UpgradeDamage()
    {
        damage *= damagePerLevel;
        damageLevel++;
    }
    public virtual void UpgradeSpeed()
    {
        attackSpeed /= speedPerLevel;
        speedLevel++;
    }
    public virtual void UpgradeRange()
    {
        maxRange *= rangePerLevel;
        rangeLevel++;
    }

    public virtual void PopulateUpgradeMenu(GUIManager manager)
    {
        throw new NotSupportedException(" *** ERROR: Virtual function must be overriden");
    }

    protected virtual float CalcDPS()
    {
        return damage / attackSpeed;
    }

    protected virtual float CalcScore()
    {
        return Mathf.Pow(maxRange, rangeScoreWeight) * Mathf.Pow(CalcDPS(), DPSScoreWeight) * Mathf.Pow(effectMultiplier, effectWeight);
    }

    protected virtual int CalcUpgradeCost_Damage()
    {
        return UpgradeCost(damagePerLevel, DPSScoreWeight);
    }
    protected virtual int CalcUpgradeCost_Speed()
    {
        return UpgradeCost(speedPerLevel, DPSScoreWeight);
    }
    protected virtual int CalcUpgradeCost_Range()
    {
        return UpgradeCost(rangePerLevel, rangeScoreWeight);
    }
    // The effect will vary by tower but the cost will follow the same formula for most
    protected virtual int CalcUpgradeCost_Effect()
    {
        return UpgradeCost(effectPerLevel, effectWeight);
    }

    protected int UpgradeCost(float attrMultiplier, float attrWeight)
    {
        float deltaScore = CalcScore() * (Mathf.Pow(attrMultiplier, attrWeight) - 1);
        return ScoreToCost(deltaScore);
    }

    // Every tower has a unique set of upgrades
    protected virtual void UpdateUpgradeCost()
    {
        //throw new NotSupportedException(" *** ERROR: Virtual function must be overriden");
    }
    public virtual void Upgrade()
    {
        //throw new NotSupportedException(" *** ERROR: Virtual function must be overriden");
    }

    // Master cost function. Round down to nearest 5
    protected int ScoreToCost(float score)
    {
        return Mathf.FloorToInt((score / 10) / 5) * 5;
    }
}
