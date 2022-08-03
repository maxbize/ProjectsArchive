using UnityEngine;
using System.Collections;

/*
 * Damage:   Low
 * Range:    Medium (upgradeable)
 * Speed:    Medium (upgradeable)
 * Effect:   Slow (upgradeable)
 * Hits air: Yes
 * Notes:    - None
 */
public class IceTower : Tower
{
    float projectileSpeed = 500f; // units / update ?
    float towerHeight = 90f;

    float slowPercent = 0.10f;
    int slowLevel = 1;

    protected override void Start()
    {
        damage = 80;
        attackSpeed = 2.0f;
        minRange = 0.1f;
        maxRange = 200f;

        generalStr = "An average tower that slows enemies";
        damageStr = "Low";
        speedStr = "Medium";
        rangeStr = "Medium";
        effectStr = "Slows enemies";

        effectMultiplier = 1.1f;
        effectPerLevel = 1.1f;
        effectWeight = 0.8f;

        type = type_t.iceTower;

        base.Start();
    }

    protected override void Attack(Enemy enemy)
    {
        GameObject proj = (GameObject)GameObject.Instantiate(GameObject.Find("Cloneable_IceProjectile"));
        proj.transform.position = transform.position + new Vector3(0, towerHeight, 0);

        IceProjectile obj = proj.AddComponent<IceProjectile>();
        obj.Init(projectileSpeed, enemy, damage);

        if (enemy.transform.position == Vector3.zero)
        {
            print(" *** BUG!!! This bug was fixed by making LevelManager LateUpdate");
        }
    }

    public void UpgradeSlow()
    {
        slowPercent += 0.05f;
        effectMultiplier *= effectPerLevel;
        slowLevel++;
    }

    public override void PopulateUpgradeMenu(GUIManager manager)
    {
        manager.AddUpgrade("Range: ", rangeLevel, CalcUpgradeCost_Range(), UpgradeRange);
        manager.AddUpgrade("Speed: ", speedLevel, CalcUpgradeCost_Speed(), UpgradeSpeed);
        manager.AddUpgrade("Slow: ", slowLevel, CalcUpgradeCost_Effect(), UpgradeSlow);
    }
}
