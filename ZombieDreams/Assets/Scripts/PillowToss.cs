using UnityEngine;

// PillowToss: Isaac Jr.'s starting weapon. It fires on its own, throwing a pillow at the nearest zombie.
// Every number is multiplied by PlayerStats, so upgrade cards make it stronger.
[RequireComponent(typeof(PlayerStats))]
public class PillowToss : Weapon
{
    // The pillow to copy (drag Prefabs/Pillow here).
    public GameObject pillowPrefab;

    public float baseSpeed = 9f;
    public float range = 10f;           // only throws if a zombie is this close

    private float cooldownLeft;

    // What each weapon level gives (level 1 is the starting weapon):
    float BaseDamage { get { return level >= 5 ? 20f : (level >= 2 ? 14f : 10f); } }
    int BaseCount { get { return level >= 3 ? 2 : 1; } }
    float BaseCooldown { get { return level >= 4 ? 0.64f : 0.8f; } }
    int Pierce { get { return level >= 5 ? 1 : 0; } }

    public override string DescribeLevel(int levelToDescribe)
    {
        switch (levelToDescribe)
        {
            case 1: return "Throws a pillow at the nearest zombie";
            case 2: return "+40% damage";
            case 3: return "+1 pillow per throw";
            case 4: return "Throws 25% faster";
            default: return "Pillows go through 1 zombie, and hit harder";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        displayName = "Pillow Toss";
        if (level == 0) level = 1;   // Isaac Jr. starts with this weapon
    }

    void Update()
    {
        if (!Owned || pillowPrefab == null || Dead) return;

        cooldownLeft -= Time.deltaTime;
        if (cooldownLeft > 0f) return;

        Zombie target = FindNearestZombie();
        if (target == null) return;   // nothing to throw at; try again next frame

        Throw(target);
        // Attack speed makes the cooldown shorter.
        cooldownLeft = BaseCooldown / stats.attackSpeedMultiplier;
    }

    // Loops over all zombies and returns the closest one within range (or null).
    Zombie FindNearestZombie()
    {
        Zombie nearest = null;
        float bestDistance = range * range;
        Vector2 origin = stats.FeetPosition;
        foreach (Zombie zombie in Zombie.All)
        {
            if (zombie.IsDead) continue;
            float distance = (zombie.FeetPosition - origin).sqrMagnitude;
            if (distance < bestDistance)
            {
                bestDistance = distance;
                nearest = zombie;
            }
        }
        return nearest;
    }

    void Throw(Zombie target)
    {
        // Throw from his chest. Zombies can only be hit at their FEET (that's where their collider is),
        // so aim there.
        Vector2 start = (Vector2)transform.position + Vector2.up * 0.1f;
        Vector2 aim = (target.FeetPosition - start).normalized;

        int count = BaseCount + stats.extraProjectiles;
        float spreadDegrees = 12f;   // extra pillows fan out a little

        for (int i = 0; i < count; i++)
        {
            // The first pillow goes straight at the target. Extra pillows fan out on alternate sides:
            // 0, +12, -12, +24, -24 degrees...
            float angle = ((i + 1) / 2) * spreadDegrees * (i % 2 == 1 ? 1f : -1f);
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * aim;

            GameObject pillow = Instantiate(pillowPrefab, start, Quaternion.identity);
            pillow.GetComponent<Projectile>().Launch(
                direction,
                BaseDamage * stats.damageMultiplier,
                baseSpeed * stats.projectileSpeedMultiplier,
                stats.attackSizeMultiplier,
                Pierce);
        }
    }
}
