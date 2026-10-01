using UnityEngine;
using UnityEngine.InputSystem;

// PlayerMovement: moves Isaac Jr. around the bedroom.
// Reads WASD / arrow keys / gamepad left stick using Unity's new Input System,
// then pushes the Rigidbody2D in that direction so walls and toy blocks stop us.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // How fast Isaac Jr. walks, in world units per second. Tweak it in the Inspector!
    public float moveSpeed = 5f;

    // The "Move" action. Its key/stick bindings are set up in Awake below.
    private InputAction moveAction;
    private Rigidbody2D body;
    private Vector2 moveInput;

    // Other scripts (like PlayerAnimator) can read which way the player is pushing the stick/keys.
    public Vector2 MoveInput => moveInput;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();

        // Create a Move action that gives us a direction (a Vector2).
        moveAction = new InputAction("Move", InputActionType.Value);

        // WASD keys.
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        // Arrow keys too, because why not.
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");

        // Gamepad left stick.
        moveAction.AddBinding("<Gamepad>/leftStick");
    }

    // Actions must be switched on to work, and off when we're disabled.
    void OnEnable() { moveAction.Enable(); }
    void OnDisable()
    {
        moveAction.Disable();
        moveInput = Vector2.zero;   // so the walk animation stops when movement is switched off
    }

    void Update()
    {
        // Read input every frame. ClampMagnitude stops diagonals from being faster.
        moveInput = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);
    }

    void FixedUpdate()
    {
        // Physics happens in FixedUpdate. Setting the velocity moves the player.
        body.linearVelocity = moveInput * moveSpeed;
    }
}
