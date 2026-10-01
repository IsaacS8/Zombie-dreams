using UnityEngine;

// Spawner: creates zombies just off-screen in a ring around Isaac Jr.
// The longer you survive, the faster they spawn.
public class Spawner : MonoBehaviour
{
    // The zombie to copy (drag Prefabs/Sleepwalker here).
    public GameObject zombiePrefab;

    // Zombies per second at the start, and how many MORE per second you get every minute.
    public float startSpawnsPerSecond = 0.7f;
    public float extraSpawnsPerSecondPerMinute = 0.6f;

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
        if (player == null || zombiePrefab == null) return;

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
            if (FindObjectsByType<Zombie>(FindObjectsSortMode.None).Length < maxZombies)
                SpawnOne();
        }
    }

    void SpawnOne()
    {
        // A random spot on a ring just outside the camera's view.
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        float radius = Mathf.Sqrt(halfWidth * halfWidth + halfHeight * halfHeight) + 2f;

        Vector2 offset = Random.insideUnitCircle.normalized * radius;
        Vector2 position = (Vector2)player.position + offset;
        Instantiate(zombiePrefab, position, Quaternion.identity);
    }
}
