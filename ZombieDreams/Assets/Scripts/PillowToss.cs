using UnityEngine;

// PillowToss: Isaac Jr.'s starting weapon. It fires on its own, throwing a pillow at the nearest zombie.
// Every number is multiplied by PlayerStats, so upgrade cards make it stronger.
[RequireComponent(typeof(PlayerStats))]
public class PillowToss : MonoBehaviour
{
    // The pillow to copy (drag Prefabs/Pillow here).
    public GameObject pillowPrefab;

    // Level 1 values. (Weapon levels in a later step make these bigger.)
    public float baseCooldown = 0.8f;   // seconds between throws
    public float baseDamage = 10f;
    public float baseSpeed = 9f;
    public int baseCount = 1;           // pillows per throw
    public int pierce = 0;              // extra zombies a pillow can pass through
    public float range = 10f;           // only throws if a zombie is this close

    private PlayerStats stats;
    private PlayerHealth health;
    private float cooldownLeft;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (pillowPrefab == null || (health != null && health.IsDead)) return;

        cooldownLeft -= Time.deltaTime;
        if (cooldownLeft > 0f) return;

        Zombie target = FindNearestZombie();
        if (target == null) return;   // nothing to throw at; try again next frame

        Throw(target);
        // Attack speed makes the cooldown shorter.
        cooldownLeft = baseCooldown / stats.attackSpeedMultiplier;
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

        int count = baseCount + stats.extraProjectiles;
        float spreadDegrees = 12f;   // extra pillows fan out a little

        for (int i = 0; i < count; i++)
        {
            // Spread the pillows evenly around the aim direction.
            float angle = (i - (count - 1) / 2f) * spreadDegrees;
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * aim;

            GameObject pillow = Instantiate(pillowPrefab, start, Quaternion.identity);
            pillow.GetComponent<Projectile>().Launch(
                direction,
                baseDamage * stats.damageMultiplier,
                baseSpeed * stats.projectileSpeedMultiplier,
                stats.attackSizeMultiplier,
                pierce);
        }
    }
}
