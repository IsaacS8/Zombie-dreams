using UnityEngine;

// Spawner: creates zombies just off-screen in a ring around Isaac Jr.
// The longer you survive, the faster they spawn, and tougher kinds join in
// (Night Terrors after a minute or so, Big Snorers after a few minutes).
public class Spawner : MonoBehaviour
{
    // One kind of zombie the spawner can make.
    [System.Serializable]
    public class SpawnEntry
    {
        public string kind;            // must match the zombie's Kind (used to count how many are alive)
        public GameObject prefab;      // the zombie to copy
        public float startMinute;      // doesn't appear before this many minutes have passed
        public float weight = 1f;      // bigger = picked more often
        public int maxAlive;           // never more than this many at once (0 = no limit)
    }

    // The basic zombie (drag Prefabs/Sleepwalker here). Used if the list below is empty.
    public GameObject zombiePrefab;

    // All the kinds of zombie, with when they start appearing.
    public SpawnEntry[] entries;

    // Zombies per second at the start, and how many MORE per second you get every minute.
    public float startSpawnsPerSecond = 0.5f;
    public float extraSpawnsPerSecondPerMinute = 0.3f;

    // Never have more than this many zombies alive at once (keeps the game running smoothly).
    public int maxZombies = 150;

    private Transform player;
    private float spawnProgress;   // builds up over time; each time it passes 1, we spawn a zombie

    void Start()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Stop spawning once the player is dead.
        var health = player.GetComponent<PlayerHealth>();
        if (health != null && health.IsDead) return;

        // How many minutes since the dream started?
        float minutes = Time.timeSinceLevelLoad / 60f;
        float rate = startSpawnsPerSecond + extraSpawnsPerSecondPerMinute * minutes;

        spawnProgress += rate * Time.deltaTime;
        while (spawnProgress >= 1f)
        {
            spawnProgress -= 1f;
            if (Zombie.All.Count < maxZombies)
                SpawnOne(minutes);
        }
    }

    // Picks which kind to spawn: only kinds that have started appearing and aren't at their limit.
    GameObject PickPrefab(float minutes)
    {
        if (entries == null || entries.Length == 0) return zombiePrefab;

        float total = 0f;
        foreach (var entry in entries)
            if (IsAvailable(entry, minutes)) total += entry.weight;
        if (total <= 0f) return zombiePrefab;

        float roll = Random.value * total;
        foreach (var entry in entries)
        {
            if (!IsAvailable(entry, minutes)) continue;
            roll -= entry.weight;
            if (roll <= 0f) return entry.prefab;
        }
        return zombiePrefab;
    }

    bool IsAvailable(SpawnEntry entry, float minutes)
    {
        if (entry.prefab == null || minutes < entry.startMinute) return false;
        if (entry.maxAlive > 0)
        {
            int alive = 0;
            foreach (Zombie zombie in Zombie.All)
                if (zombie.kind == entry.kind) alive++;
            if (alive >= entry.maxAlive) return false;
        }
        return true;
    }

    void SpawnOne(float minutes)
    {
        GameObject prefab = PickPrefab(minutes);
        if (prefab == null) return;

        // A random spot on a ring just outside the camera's view.
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        float radius = Mathf.Sqrt(halfWidth * halfWidth + halfHeight * halfHeight) + 2f;

        Vector2 offset = Random.insideUnitCircle.normalized * radius;
        Vector2 position = (Vector2)player.position + offset;
        Instantiate(prefab, position, Quaternion.identity);
    }
}
