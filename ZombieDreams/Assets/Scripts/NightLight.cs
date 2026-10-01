using System.Collections.Generic;
using UnityEngine;

// NightLight: a beam of warm lamp light that spins around Isaac Jr. and burns zombies it sweeps over.
// A zombie standing in the beam gets hurt every 0.4 seconds.
[RequireComponent(typeof(PlayerStats))]
public class NightLight : Weapon
{
    // The beam picture (drag Prefabs/Beam here). Its art is 3 world units long and points right.
    public GameObject beamPrefab;

    public float baseLength = 3.2f;     // how far the beam reaches
    public float halfWidth = 0.4f;      // how wide the beam is (each side of the middle line)
    public float tickInterval = 0.4f;   // seconds between hits on the same zombie

    private const float BeamArtLength = 3f;   // the beam sprite is 96 pixels = 3 units long

    private readonly List<Transform> beams = new List<Transform>();
    private readonly Dictionary<Zombie, float> nextHitTime = new Dictionary<Zombie, float>();
    private float angle;
    private float cleanupTimer;

    // What each weapon level gives:
    float BaseDamage { get { return level >= 5 ? 12f : (level >= 2 ? 9f : 6f); } }
    int BaseBeams { get { return 1 + (level >= 3 ? 1 : 0) + (level >= 5 ? 1 : 0); } }
    float BaseSpin { get { return level >= 4 ? 170f : 120f; } }   // degrees per second

    public override string DescribeLevel(int levelToDescribe)
    {
        switch (levelToDescribe)
        {
            case 1: return "A beam of light spins around you";
            case 2: return "+50% damage";
            case 3: return "+1 beam";
            case 4: return "Spins 40% faster";
            default: return "+1 beam, and +33% damage";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        displayName = "Night Light";
    }

    void Update()
    {
        if (!Owned || beamPrefab == null || Dead)
        {
            SetBeamsVisible(false);
            return;
        }

        int count = BaseBeams + stats.extraProjectiles;
        EnsureBeams(count);
        SetBeamsVisible(true);

        angle += BaseSpin * stats.attackSpeedMultiplier * Time.deltaTime;

        float length = baseLength * stats.attackSizeMultiplier;
        float reach = halfWidth * stats.attackSizeMultiplier;
        float damage = BaseDamage * stats.damageMultiplier;
        Vector2 origin = transform.position;

        for (int i = 0; i < count; i++)
        {
            float beamAngle = angle + i * 360f / count;
            Vector2 direction = new Vector2(Mathf.Cos(beamAngle * Mathf.Deg2Rad), Mathf.Sin(beamAngle * Mathf.Deg2Rad));

            // Place and stretch the beam picture.
            Transform beam = beams[i];
            beam.position = origin;
            beam.rotation = Quaternion.Euler(0f, 0f, beamAngle);
            beam.localScale = new Vector3(length / BeamArtLength, stats.attackSizeMultiplier, 1f);

            // Hurt every zombie close to the beam's line. (Going backwards is safe even if a zombie
            // disappears from the list when it is defeated.)
            for (int z = Zombie.All.Count - 1; z >= 0; z--)
            {
                if (z >= Zombie.All.Count) continue;
                Zombie zombie = Zombie.All[z];
                if (zombie.IsDead) continue;

                Vector2 toZombie = (Vector2)zombie.transform.position - origin;
                float along = Vector2.Dot(toZombie, direction);
                if (along < 0f || along > length) continue;
                float sideways = Mathf.Abs(direction.x * toZombie.y - direction.y * toZombie.x);
                if (sideways > reach + zombie.bodyRadius) continue;

                float ready;
                if (nextHitTime.TryGetValue(zombie, out ready) && Time.time < ready) continue;
                nextHitTime[zombie] = Time.time + tickInterval;
                zombie.TakeDamage(damage);
            }
        }

        // Every few seconds forget zombies that are gone.
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

    void EnsureBeams(int count)
    {
        while (beams.Count < count)
        {
            GameObject beam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
            beams.Add(beam.transform);
        }
        while (beams.Count > count)
        {
            Destroy(beams[beams.Count - 1].gameObject);
            beams.RemoveAt(beams.Count - 1);
        }
    }

    void SetBeamsVisible(bool visible)
    {
        foreach (var beam in beams)
            if (beam != null && beam.gameObject.activeSelf != visible) beam.gameObject.SetActive(visible);
    }

    void OnDestroy()
    {
        foreach (var beam in beams)
            if (beam != null) Destroy(beam.gameObject);
    }
}
