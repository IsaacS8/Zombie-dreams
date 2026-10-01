using UnityEngine;

// CameraFollow: put this on the Main Camera. It glides after the target
// (Isaac Jr.) instead of snapping, which feels smoother.
public class CameraFollow : MonoBehaviour
{
    // What to follow. Drag the Player here in the Inspector.
    public Transform target;

    // Roughly how many seconds the camera takes to catch up. Smaller = snappier.
    public float smoothTime = 0.15f;

    // Used by SmoothDamp to remember how fast the camera is currently moving.
    private Vector3 velocity;

    // LateUpdate runs after everything else has moved, so the camera never lags a frame behind.
    void LateUpdate()
    {
        if (target == null) return;

        // Follow on X and Y, but keep the camera's own Z (2D cameras sit at z = -10).
        Vector3 goal = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
    }
}
