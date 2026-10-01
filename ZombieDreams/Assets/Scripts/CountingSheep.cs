using System.Collections.Generic;
using UnityEngine;

// CountingSheep: dream sheep that hop in a circle around Isaac Jr. and bonk any zombie they touch.
[RequireComponent(typeof(PlayerStats))]
public class CountingSheep : Weapon
{
    // The sheep to copy (drag Prefabs/Sheep here).
    public GameObject sheepPrefab;

    public float baseRadius = 1.4f;      // how far from Isaac Jr. they circle
    public float hitCooldown = 0.6f;     // seconds before the same zombie can be bonked again
    public float hitDistance = 0.6f;     // how close a sheep must be to a zombie's body to bonk it

    private readonly List<Transform> sheep = new List<Transform>();
    private readonly List<SpriteRenderer> sheepRenderers = new List<SpriteRenderer>();
    private readonly Dictionary<Zombie, float> nextHitTime = new Dictionary<Zombie, float>();
    private float angle;
    private float cleanupTimer;

    // What each weapon level gives:
    float BaseDamage { get { return level >= 5 ? 16f : (level >= 2 ? 12f : 8f); } }
    int BaseSheep { get { return 2 + (level >= 3 ? 1 : 0) + (level >= 5 ? 1 : 0); } }
    float BaseSpin { get { return level >= 4 ? 200f : 140f; } }   // degrees per second

    public override string DescribeLevel(int levelToDescribe)
    {
        switch (levelToDescribe)
        {
            case 1: return "Sheep circle you and bonk zombies";
            case 2: return "+50% damage";
            case 3: return "+1 sheep";
            case 4: return "Circle 40% faster";
            default: return "+1 sheep, and +33% damage";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        displayName = "Counting Sheep";
    }

    void Update()
    {
        if (!Owned || sheepPrefab == null || Dead)
        {
            SetVisible(false);
            return;
        }

        int count = BaseSheep + stats.extraProjectiles;
        EnsureSheep(count);
        SetVisible(true);

        angle += BaseSpin * stats.attackSpeedMultiplier * Time.deltaTime;

        float radius = baseRadius * stats.attackSizeMultiplier;
        float damage = BaseDamage * stats.damageMultiplier;
        float reach = hitDistance * stats.attackSizeMultiplier;
        Vector2 center = transform.position;

        for (int i = 0; i < count; i++)
        {
            float a = (angle + i * 360f / count) * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            sheep[i].position = center + offset * radius;
            sheep[i].localScale = Vector3.one * stats.attackSizeMultiplier;

            // The sheep face the way they are going (the art faces right).
            Vector2 travel = new Vector2(-offset.y, offset.x);
            sheepRenderers[i].flipX = travel.x < 0f;

            // Bonk zombies that are close. (Going backwards is safe if a zombie is removed when defeated.)
            Vector2 sheepPosition = sheep[i].position;
            for (int z = Zombie.All.Count - 1; z >= 0; z--)
            {
                if (z >= Zombie.All.Count) continue;
                Zombie zombie = Zombie.All[z];
                if (zombie.IsDead) continue;
                float bonkDistance = reach + zombie.bodyRadius;   // bigger zombies are easier to hit
                if (((Vector2)zombie.transform.position - sheepPosition).sqrMagnitude > bonkDistance * bonkDistance) continue;

                float ready;
                if (nextHitTime.TryGetValue(zombie, out ready) && Time.time < ready) continue;
                nextHitTime[zombie] = Time.time + hitCooldown;
                zombie.TakeDamage(damage);
            }
        }

        cleanupTimer += Time.deltaTime;
        if (cleanupTimer > 5f)
        {
            cleanupTimer = 0f;
            var gone = new List<Zombie>();
            foreach (var pair in nextHitTime)
                if (pair.Key == null) gone.Add(pair.Key);
            foreach (var zombie in gone) nextHitTime.Remove(zombie);
        }
    }

    void EnsureSheep(int count)
    {
        while (sheep.Count < count)
        {
            GameObject s = Instantiate(sheepPrefab, transform.position, Quaternion.identity);
            sheep.Add(s.transform);
            sheepRenderers.Add(s.GetComponent<SpriteRenderer>());
        }
        while (sheep.Count > count)
        {
            Destroy(sheep[sheep.Count - 1].gameObject);
            sheep.RemoveAt(sheep.Count - 1);
            sheepRenderers.RemoveAt(sheepRenderers.Count - 1);
        }
    }

    void SetVisible(bool visible)
    {
        foreach (var s in sheep)
            if (s != null && s.gameObject.activeSelf != visible) s.gameObject.SetActive(visible);
    }

    void OnDestroy()
    {
        foreach (var s in sheep)
            if (s != null) Destroy(s.gameObject);
    }
}
