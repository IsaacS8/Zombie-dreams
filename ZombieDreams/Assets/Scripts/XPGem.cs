using System.Collections.Generic;
using UnityEngine;

// XPGem: the "dream shard" a zombie drops when it's defeated.
// It twinkles on the floor. When Isaac Jr. gets close it flies to him and gives him XP.
public class XPGem : MonoBehaviour
{
    // Every gem on the floor right now, oldest first.
    public static readonly List<XPGem> All = new List<XPGem>();

    // Never more than this many gems at once (keeps the game running smoothly in a long run).
    // When there are too many, the OLDEST gem melts into the newest one, so no XP is ever lost.
    public const int MaxGems = 200;

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

    // Makes sure the list starts empty every time the game starts.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetList() { All.Clear(); }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animTimer = Random.value * 4f;   // so gems don't all twinkle in sync
    }

    void OnEnable() { All.Add(this); }
    void OnDisable() { All.Remove(this); }

    void Start()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            stats = playerObject.GetComponent<PlayerStats>();
            xp = playerObject.GetComponent<PlayerXP>();
        }

        // Too many gems on the floor? The oldest one melts into this one.
        if (All.Count > MaxGems && All[0] != this)
        {
            XPGem oldest = All[0];
            value += oldest.value;
            All.RemoveAt(0);
            Destroy(oldest.gameObject);
        }

        // A gem worth more XP is a little bigger.
        transform.localScale = Vector3.one * Mathf.Min(1f + 0.15f * (value - 1), 2f);
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
