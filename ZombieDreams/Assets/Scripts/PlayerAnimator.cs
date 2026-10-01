using UnityEngine;

// PlayerAnimator: picks which picture of Isaac Jr. (with Scarlet on his shoulder) to show.
// It checks which way you're moving, turns that into one of 8 directions,
// and flips through the walk frames (or the idle "breathing" frames when you stand still).
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    // All 48 frames from Art/IsaacJr/IsaacJr.png, in sheet order:
    // 8 rows (directions) x 6 columns (idle0, idle1, walk0, walk1, walk2, walk3).
    public Sprite[] frames;

    // How many pictures per second to show.
    public float walkFramesPerSecond = 8f;
    public float idleFramesPerSecond = 2f;

    const int FramesPerDirection = 6;

    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;
    private int direction = 0;   // which row of the sheet; starts facing down (towards the camera)
    private bool wasMoving;
    private float timer;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (frames == null || frames.Length < 8 * FramesPerDirection) return;

        Vector2 input = movement != null ? movement.MoveInput : Vector2.zero;
        bool moving = input.sqrMagnitude > 0.01f;

        // Only change direction while moving, so he keeps facing the last way he walked.
        if (moving) direction = DirectionFromInput(input);

        // Restart the animation when switching between standing and walking.
        if (moving != wasMoving) timer = 0f;
        wasMoving = moving;
        timer += Time.deltaTime;

        int frame;
        if (moving) frame = 2 + (int)(timer * walkFramesPerSecond) % 4;   // walk0..walk3
        else frame = (int)(timer * idleFramesPerSecond) % 2;              // idle0..idle1

        spriteRenderer.sprite = frames[direction * FramesPerDirection + frame];
    }

    // Turns a movement direction into a sheet row:
    // 0 down, 1 down-right, 2 right, 3 up-right, 4 up, 5 up-left, 6 left, 7 down-left
    int DirectionFromInput(Vector2 input)
    {
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;  // 0 = right, 90 = up, -90 = down
        int slice = Mathf.RoundToInt(angle / 45f);                    // -4 .. 4
        switch (slice)
        {
            case 0: return 2;           // right
            case 1: return 3;           // up-right
            case 2: return 4;           // up
            case 3: return 5;           // up-left
            case 4: case -4: return 6;  // left
            case -3: return 7;          // down-left
            case -2: return 0;          // down
            default: return 1;          // -1: down-right
        }
    }
}
