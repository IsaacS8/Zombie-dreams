using UnityEngine;

// XPGem: the "dream shard" a zombie drops when it's defeated.
// It twinkles on the floor. When Isaac Jr. gets close it flies to him and gives him XP.
public class XPGem : MonoBehaviour
{
    public int value = 1;                // how much XP it gives (the zombie sets this)
    public Sprite[] frames;              // the twinkle animation
    public float framesPerSecond = 6f;

    public float flySpeed = 8f;          // speed once it has started flying to the player
    public float collectDistance = 0.3f; // how close it needs to be to count as collected

    private Transform player;
    private PlayerStats stats;
    private PlayerXP xp;
    private SpriteRenderer spriteRenderer;
    private bool flying;
    private float animTimer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animTimer = Random.value * 4f;   // so gems don't all twinkle in sync
    }

    void Start()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            stats = playerObject.GetComponent<PlayerStats>();
            xp = playerObject.GetComponent<PlayerXP>();
        }
    }

    void Update()
    {
        // Twinkle.
        if (frames != null && frames.Length > 0)
        {
            animTimer += Time.deltaTime * framesPerSecond;
            spriteRenderer.sprite = frames[(int)animTimer % frames.Length];
        }

        if (player == null || stats == null) return;

        Vector2 target = stats.FeetPosition;
        float distance = Vector2.Distance(transform.position, target);

        // Close enough? Start flying to the player (and keep flying even if he walks away).
        if (distance <= stats.pickupRadius) flying = true;

        if (flying)
        {
            // Speeds up the longer it flies, so it can never be outrun.
            flySpeed += 12f * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) <= collectDistance)
            {
                if (xp != null) xp.AddXP(value);
                Destroy(gameObject);
            }
        }
    }
}
