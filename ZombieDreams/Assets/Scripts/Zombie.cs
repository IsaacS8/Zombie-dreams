using System.Collections.Generic;
using UnityEngine;

// Zombie: the Sleepwalker. It shuffles in a straight line toward Isaac Jr. (no pathfinding),
// hurts him when they touch, and vanishes in a puff of dream smoke (dropping an XP gem) when defeated.
[RequireComponent(typeof(Rigidbody2D))]
public class Zombie : MonoBehaviour
{
    // A list of every zombie that exists right now, so weapons can find the nearest one quickly.
    public static readonly List<Zombie> All = new List<Zombie>();

    // Which kind of zombie this is (the Spawner uses it to count them).
    public string kind = "Sleepwalker";

    public float moveSpeed = 1.8f;      // slow! Isaac Jr. walks at 5
    public float contactDamage = 10f;   // taken by the player when touching this zombie
    public float maxHealth = 20f;       // two pillow hits
    public int xpValue = 1;             // XP the dropped gem gives

    // Walk frames from Art/Sleepwalker/Sleepwalker.png (the art faces RIGHT).
    public Sprite[] walkFrames;
    public float framesPerSecond = 5f;

    // What appears when it's defeated: an XP gem and a puff of smoke (both are prefabs).
    public GameObject gemPrefab;
    public GameObject deathEffectPrefab;

    // If the player gets this far away, remove the zombie so we never end up with hundreds.
    public float despawnDistance = 30f;

    public bool IsDead { get; private set; }

    // Roughly how wide the zombie's body is (from its middle to its edge). Beams and sheep use it.
    public float bodyRadius = 0.3f;

    // How far below the sprite's center the feet are (bigger zombies have a bigger number).
    public float feetOffset = 0.6f;

    // Where the zombie's feet are.
    public Vector2 FeetPosition
    {
        get { return (Vector2)transform.position + Vector2.down * feetOffset; }
    }

    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private float health;
    private float animTimer;

    // Makes sure the list starts empty every time the game starts.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetList() { All.Clear(); }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    void OnEnable() { All.Add(this); }
    void OnDisable() { All.Remove(this); }

    void Start()
    {
        // The player is tagged "Player" in the scene.
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Too far away? Go away.
        if (Vector2.Distance(transform.position, player.position) > despawnDistance)
        {
            Destroy(gameObject);
            return;
        }

        // Face the player (the art faces right, so flip it when the player is on the left).
        spriteRenderer.flipX = player.position.x < transform.position.x;

        // Flip through the walk frames.
        if (walkFrames != null && walkFrames.Length > 0)
        {
            animTimer += Time.deltaTime * framesPerSecond;
            spriteRenderer.sprite = walkFrames[(int)animTimer % walkFrames.Length];
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Walk straight at the player.
        Vector2 direction = ((Vector2)player.position - body.position).normalized;
        body.linearVelocity = direction * moveSpeed;
    }

    // Called every physics step while this zombie is touching something.
    void OnCollisionStay2D(Collision2D collision)
    {
        if (IsDead || !collision.gameObject.CompareTag("Player")) return;   // cheap check first: most touches are zombie-vs-zombie
        var playerHealth = collision.collider.GetComponent<PlayerHealth>();
        if (playerHealth != null) playerHealth.TakeDamage(contactDamage);
    }

    // Weapons call this when they hit the zombie.
    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        health -= amount;
        if (health <= 0f) Die();
    }

    void Die()
    {
        IsDead = true;
        All.Remove(this);   // weapons shouldn't aim at a zombie that's already gone

        // A puff of dream smoke where the zombie was.
        if (deathEffectPrefab != null)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        // An XP gem on the floor at its feet.
        if (gemPrefab != null)
        {
            GameObject gem = Instantiate(gemPrefab, FeetPosition, Quaternion.identity);
            gem.GetComponent<XPGem>().value = xpValue;   // bigger zombies drop gems worth more XP
        }

        Destroy(gameObject);
    }
}
