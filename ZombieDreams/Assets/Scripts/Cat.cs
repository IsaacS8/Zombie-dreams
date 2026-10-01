using UnityEngine;

// Cat: Scarlet, who rides on Isaac Jr.'s shoulder.
// Every couple of seconds she scratches the nearest zombie(s) close to her.
// She can't be hurt. If Isaac Jr. goes down, she goes down with him (the run ends).
// Her scratch uses the same PlayerStats as the pillows, so Bad Dream, Double Vision, Caffeine
// and Big Dreams help her too. She also has 3 upgrade cards of her own (see LevelUpUI).
[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerHealth))]
public class Cat : MonoBehaviour
{
    // The claw-mark effect shown on a zombie she scratches (a prefab).
    public GameObject slashPrefab;

    public float baseCooldown = 1.6f;    // seconds between scratches
    public float baseDamage = 10f;
    public float range = 3f;           // she can only reach zombies this close to Isaac Jr.

    // Her own upgrades (changed by the cat cards in LevelUpUI):
    public float sharperClawsBonus = 0f;       // "Sharper Claws": extra scratch damage
    public float zoomiesMultiplier = 1f;       // "Zoomies": scratches more often
    public float purrHealPerSecond = 0f;       // "Purr": slowly heals Isaac Jr.

    private PlayerStats stats;
    private PlayerHealth health;
    private float cooldownLeft = 1f;           // her first scratch comes a second after the game starts
    private float purrBuffer;                  // heal builds up here until it's worth a whole point

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (health != null && health.IsDead) return;

        // Purr: heal a little every second.
        if (purrHealPerSecond > 0f)
        {
            purrBuffer += purrHealPerSecond * Time.deltaTime;
            if (purrBuffer >= 1f)
            {
                health.Heal(1f);
                purrBuffer -= 1f;
            }
        }

        cooldownLeft -= Time.deltaTime;
        if (cooldownLeft > 0f) return;

        if (Scratch())
        {
            // Attack speed and Zoomies both make her faster.
            cooldownLeft = baseCooldown / (stats.attackSpeedMultiplier * zoomiesMultiplier);
        }
        // If there was nothing to scratch, she just tries again next frame.
    }

    // Scratches the nearest zombie(s). Returns true if she hit anything.
    bool Scratch()
    {
        int count = 1 + stats.extraProjectiles;   // Double Vision gives extra scratches too
        float damage = (baseDamage + sharperClawsBonus) * stats.damageMultiplier;
        Vector2 origin = stats.FeetPosition;
        bool hitSomething = false;

        // Scratch the nearest zombies one after another, never the same one twice.
        var alreadyHit = new System.Collections.Generic.List<Zombie>();
        for (int i = 0; i < count; i++)
        {
            Zombie target = FindNearestZombie(origin, alreadyHit);
            if (target == null) break;

            alreadyHit.Add(target);
            hitSomething = true;

            // Show the claw marks first (the zombie might vanish when hit).
            if (slashPrefab != null)
            {
                GameObject slash = Instantiate(slashPrefab, target.transform.position, Quaternion.identity);
                slash.transform.localScale = Vector3.one * stats.attackSizeMultiplier;
            }
            target.TakeDamage(damage);
        }
        return hitSomething;
    }

    Zombie FindNearestZombie(Vector2 origin, System.Collections.Generic.List<Zombie> skip)
    {
        Zombie nearest = null;
        float bestDistance = range * range;
        foreach (Zombie zombie in Zombie.All)
        {
            if (zombie.IsDead || skip.Contains(zombie)) continue;
            float distance = (zombie.FeetPosition - origin).sqrMagnitude;
            if (distance < bestDistance)
            {
                bestDistance = distance;
                nearest = zombie;
            }
        }
        return nearest;
    }
}
