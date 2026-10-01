using UnityEngine;

// YSort: in a top-down game, things lower on the screen should be drawn IN FRONT of things higher up.
// This sets the sprite's draw order from its height, so Isaac Jr. walks behind a toy block
// when he's above it, and in front of it when he's below it.
// Put it on the player, zombies and toy blocks.
[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // LateUpdate runs after movement, so the order always matches where we ended up.
    void LateUpdate()
    {
        spriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100f);
    }
}
