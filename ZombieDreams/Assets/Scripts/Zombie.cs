using UnityEngine;

// Zombie: the Sleepwalker. It shuffles in a straight line toward Isaac Jr. (no pathfinding)
// and hurts him when they touch. Later steps add health, getting hit and dropping XP gems.
[RequireComponent(typeof(Rigidbody2D))]
public class Zombie : MonoBehaviour
{
    public float moveSpeed = 1.8f;      // slow! Isaac Jr. walks at 5
    public float contactDamage = 10f;   // taken by the player when touching this zombie

    // Walk frames from Art/Sleepwalker/Sleepwalker.png (the art faces RIGHT).
    public Sprite[] walkFrames;
    public float framesPerSecond = 5f;

    // If the player gets this far away, remove the zombie so we never end up with hundreds.
    public float despawnDistance = 30f;

    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private float animTimer;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        var health = collision.collider.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(contactDamage);
    }
}
