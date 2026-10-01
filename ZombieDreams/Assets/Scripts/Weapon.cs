using UnityEngine;

// Weapon: the shared base for all of Isaac Jr.'s weapons (Pillow Toss, Night Light, Counting Sheep, Alarm Clock).
// Every weapon has a level from 0 to 5. Level 0 means "don't have it yet"; the level-up screen can
// offer a new weapon (level 0 -> 1) or a stronger one (level 1 -> 2, and so on).
// Each level of a weapon adds damage, OR one more projectile, OR a faster attack.
// All weapons also read PlayerStats, so the upgrade cards (Bad Dream, Caffeine...) help every weapon.
public abstract class Weapon : MonoBehaviour
{
    public const int MaxLevel = 5;

    public string displayName = "Weapon";
    public int level = 0;

    protected PlayerStats stats;
    protected PlayerHealth health;

    public bool Owned { get { return level > 0; } }

    // True when the player is dead (weapons stop).
    protected bool Dead { get { return health != null && health.IsDead; } }

    protected virtual void Awake()
    {
        stats = GetComponent<PlayerStats>();
        health = GetComponent<PlayerHealth>();
    }

    // What reaching this level gives you (shown on the level-up card).
    public abstract string DescribeLevel(int levelToDescribe);

    public void LevelUp()
    {
        if (level < MaxLevel) level++;
    }
}
