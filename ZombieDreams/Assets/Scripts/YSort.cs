using UnityEngine;

// YSort: in a top-down game, things lower on the screen should be drawn IN FRONT of things higher up.
// This sets the sprite's draw order from its height, so Isaac Jr. walks behind a toy block
// when he's above it, and in front of it when he's below it.
// Put it on the player, zombies and toy blocks.
[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    // How far below the sprite's center the feet are. Characters sort by their FEET.
    // (1.5-unit tall sprites: 0.75. The bigger Big Snorer uses 1.0.)
    public float footOffset = 0.75f;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // LateUpdate runs after movement, so the order always matches where we ended up.
    void LateUpdate()
    {
        spriteRenderer.sortingOrder = -Mathf.RoundToInt((transform.position.y - footOffset) * 100f);
    }
}
