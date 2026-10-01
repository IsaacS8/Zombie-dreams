using UnityEngine;

// AlarmClock: every few seconds a loud ring-ring shockwave bursts out around Isaac Jr.
// and hurts every zombie inside it.
[RequireComponent(typeof(PlayerStats))]
public class AlarmClock : Weapon
{
    // The ring picture (drag Prefabs/Ring here).
    public GameObject ringPrefab;

    private float timer = 1.5f;   // the first ring comes 1.5 seconds after getting the weapon

    // What each weapon level gives:
    float BaseDamage { get { return level >= 5 ? 24f : (level >= 3 ? 18f : 12f); } }
    float BaseRadius { get { return level >= 5 ? 4.2f : (level >= 2 ? 3.4f : 2.8f); } }
    float BaseCooldown { get { return level >= 4 ? 3.2f : 4.5f; } }

    public override string DescribeLevel(int levelToDescribe)
    {
        switch (levelToDescribe)
        {
            case 1: return "A shockwave bursts out around you every few seconds";
            case 2: return "Bigger shockwave";
            case 3: return "+50% damage";
            case 4: return "Rings 40% more often";
            default: return "Even bigger shockwave, and double the original damage";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        displayName = "Alarm Clock";
    }

    void Update()
    {
        if (!Owned || Dead) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        float radius = BaseRadius * stats.attackSizeMultiplier;

        // Only ring when a zombie is inside the blast, so it is never wasted.
        if (!AnyZombieWithin(radius * 0.9f)) return;

        Ring(radius);
        // Attack speed makes it ring more often.
        timer = BaseCooldown / stats.attackSpeedMultiplier;
    }

    bool AnyZombieWithin(float distance)
    {
        Vector2 center = transform.position;
        foreach (Zombie zombie in Zombie.All)
            if (!zombie.IsDead && ((Vector2)zombie.transform.position - center).sqrMagnitude <= distance * distance)
                return true;
        return false;
    }

    void Ring(float radius)
    {
        Vector2 center = transform.position;

        // The ring picture.
        if (ringPrefab != null)
        {
            GameObject ring = Instantiate(ringPrefab, center, Quaternion.identity);
            ring.GetComponent<RingEffect>().Play(radius);
        }

        // Hurt every zombie inside the blast. (Going backwards is safe if a zombie is removed when defeated.)
        float damage = BaseDamage * stats.damageMultiplier;
        for (int z = Zombie.All.Count - 1; z >= 0; z--)
        {
            if (z >= Zombie.All.Count) continue;
            Zombie zombie = Zombie.All[z];
            if (zombie.IsDead) continue;
            if (((Vector2)zombie.transform.position - center).sqrMagnitude <= radius * radius)
                zombie.TakeDamage(damage);
        }
    }
}
