using UnityEngine;

// RingEffect: the Alarm Clock's shockwave ring. It grows from small to the full size of the blast,
// changing through its frames as it goes, then removes itself.
[RequireComponent(typeof(SpriteRenderer))]
public class RingEffect : MonoBehaviour
{
    public Sprite[] frames;
    public float duration = 0.5f;

    // The ring in the art is about this many world units across at scale 1.
    public const float RingUnits = 1.75f;

    private SpriteRenderer spriteRenderer;
    private float timer;
    private float finalScale = 1f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // The weapon calls this right after creating the ring. radius = how far the blast reaches (world units).
    public void Play(float radius)
    {
        finalScale = radius * 2f / RingUnits;
        transform.localScale = Vector3.one * finalScale * 0.2f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        // Grows fast at first, then slows down.
        float eased = 1f - (1f - t) * (1f - t);
        transform.localScale = Vector3.one * Mathf.Lerp(finalScale * 0.2f, finalScale, eased);

        if (frames != null && frames.Length > 0)
            spriteRenderer.sprite = frames[Mathf.Min((int)(t * frames.Length), frames.Length - 1)];

        if (t >= 1f) Destroy(gameObject);
    }
}
