using UnityEngine;

// SpriteLoop: flips through a list of sprites forever (a simple looping animation).
// "order" says which frames to show and in what order, e.g. 0,1,2,1 for a hop that goes up and back down.
// If order is empty it just plays frames 0,1,2... in order.
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteLoop : MonoBehaviour
{
    public Sprite[] frames;
    public int[] order;
    public float framesPerSecond = 8f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = Random.value * 10f;   // so several copies aren't in perfect sync
    }

    void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime * framesPerSecond;
        int step = (int)timer;
        int frame = (order != null && order.Length > 0) ? order[step % order.Length] : step % frames.Length;
        spriteRenderer.sprite = frames[Mathf.Clamp(frame, 0, frames.Length - 1)];
    }
}
