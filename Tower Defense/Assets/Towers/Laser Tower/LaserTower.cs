using UnityEngine;
using System.Collections;

/*
 * Damage:   High (upgradeable)
 * Range:    High (upgradeable)
 * Speed:    Slow 
 * Effect:   None
 * Hits air: No
 * Notes:    - Tower can only hit one unit at a time with laser
 *           - Laser uptime can be upgraded (upgradeable)
 *           - Tower can be rotated when not firing
 */
public class LaserTower : Tower
{
    BuildManager buildManagerRef;
    GameObject cloneableProj;
    GameObject currentProj;
    Vector3 verticalOffset = new Vector3(0, 25, 0);

    // Note that if attackEnabled is set to false, the tower won't charge in the charging state
    public enum state_t { charging, rotating, firing };
    state_t state;

    int attackDurationLevel = 1;
    float attackDuration = 2.0f;

    protected override void Start()
    {
        buildManagerRef = GameObject.Find("Build Manager").GetComponent<BuildManager>();
        cloneableProj = GameObject.Find("Cloneable_LaserProjectile");

        damage = 150; // With this tower, damage = DPS while firing
        attackSpeed = 3.0f;
        minRange = 0.1f;
        maxRange = cloneableProj.transform.localScale.z * 2; // radius to diameter

        generalStr = "A tower that charges a high-powered laser which can only shoot in one direction";
        damageStr = "High";
        speedStr = "Slow";
        rangeStr = "Medium";
        effectStr = "None";

        type = type_t.laserTower;

        base.Start();
    }

    // Laser tower might need to rotate
    // Arrow needs to "animate"
    protected override void Update()
    {
        if (state == state_t.rotating)
        {
            Vector3 pointPos = buildManagerRef.GetMouseHitPos();
            pointPos.y = transform.position.y;
            Quaternion lookRot = new Quaternion();
            lookRot.SetLookRotation(pointPos - transform.position);
            Vector3 tempEuler = new Vector3(0, ((int)lookRot.eulerAngles.y / 45) * 45
                + (lookRot.eulerAngles.y % 45 >= 45.0 / 2 ? 45 : 0), 0);
            transform.eulerAngles = tempEuler;
            if (Input.GetKeyDown(KeyCode.Mouse0)) // Mouse0 = left click
            {
                state = state_t.charging;
                timer = 0;
            }
        }

        else if (state == state_t.charging)
        {
            if (timer > attackSpeed)
            {
                RaycastHit raycastHit;
                Vector3 fwd = transform.TransformDirection(Vector3.forward);
                if (Physics.Raycast(transform.position, fwd, out raycastHit, maxRange, 1 << LayerMask.NameToLayer("Enemies")))
                {
                    Attack(raycastHit.transform.GetComponent<Enemy>());
                    timer = 0;
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        if (state == state_t.firing)
        {
            if (timer < attackDuration && attackEnabled)
            {
                RaycastHit raycastHit;
                Vector3 hitPos = transform.position + transform.forward * maxRange;
                if (Physics.Raycast(transform.position, transform.forward, out raycastHit, maxRange))
                {
                    hitPos = raycastHit.transform.position;
                    Enemy enemy = raycastHit.transform.GetComponent<Enemy>();
                    if (enemy) // There's a chance we could be hitting a tower or projectile
                    {
                        enemy.registerHit(damage * Time.deltaTime);
                    }
                }
                currentProj.transform.localScale = new Vector3(currentProj.transform.localScale.x,
                    currentProj.transform.localScale.y, (transform.position - hitPos).magnitude / 2);
                currentProj.transform.position = transform.position + verticalOffset + transform.forward * currentProj.transform.localScale.z;
                timer += Time.deltaTime;
            }
            else
            {
                Destroy(currentProj);
                timer = 0;
                state = state_t.charging;
            }
        }
    }

    protected override void Attack(Enemy enemy)
    {
        currentProj = (GameObject)GameObject.Instantiate(cloneableProj);
        currentProj.transform.rotation = transform.rotation;
        state = state_t.firing;
    }

    public override void Activate()
    {
        state = state_t.rotating;
        timer = 0;
        base.Activate();
    }

    public void UpgradeUptime()
    {
        attackDuration *= 1.2f;
        attackDurationLevel++;
    }

    public override void PopulateUpgradeMenu(GUIManager manager)
    {
        manager.AddUpgrade("Damage: ", damageLevel, CalcUpgradeCost_Damage(), UpgradeDamage);
        manager.AddUpgrade("Range: ", rangeLevel, CalcUpgradeCost_Range(), UpgradeRange);
        manager.AddUpgrade("Uptime: ", attackDurationLevel, CalcUpgradeCost_Uptime(), UpgradeUptime);
    }

    protected override float CalcDPS()
    {
        return (damage * attackDuration) / (attackDuration + attackSpeed);
    }

    protected int CalcUpgradeCost_Uptime()
    {
        float currentUp = attackDuration;
        float currentScore = CalcScore();
        UpgradeUptime();
        float deltaScore = CalcScore() - currentScore;
        attackDuration = currentUp;
        attackDurationLevel--;
        return ScoreToCost(deltaScore);
    }
}
