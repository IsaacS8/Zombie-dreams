using UnityEngine;

// OneShotAnimation: plays a list of sprites once, then removes itself.
// Used for the purple "poof" when a zombie is defeated.
[RequireComponent(typeof(SpriteRenderer))]
public class OneShotAnimation : MonoBehaviour
{
    public Sprite[] frames;
    public float framesPerSecond = 14f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime * framesPerSecond;
        int frame = (int)timer;

        // Done? Remove the effect.
        if (frames == null || frame >= frames.Length)
        {
            Destroy(gameObject);
            return;
        }
        spriteRenderer.sprite = frames[frame];
    }
}
