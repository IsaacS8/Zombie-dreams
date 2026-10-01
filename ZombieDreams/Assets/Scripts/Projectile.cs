using System.Collections.Generic;
using UnityEngine;

// Projectile: one reusable thing that flies in a straight line and hurts zombies it touches.
// Pillows use it now; Scarlet's scratch and the other weapons can reuse it later.
// The prefab needs: a sprite, a trigger collider and a kinematic Rigidbody2D (so touches are detected).
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float lifetime = 2f;      // seconds before it disappears if it hits nothing
    public float spinDegreesPerSecond = 540f;

    private float damage;
    private int pierce;              // how many EXTRA zombies it can go through (0 = stops at the first)
    private int hits;
    private readonly HashSet<Zombie> alreadyHit = new HashSet<Zombie>();

    // The weapon calls this right after creating the projectile.
    public void Launch(Vector2 direction, float damage, float speed, float size, int pierce)
    {
        this.damage = damage;
        this.pierce = pierce;
        transform.localScale = Vector3.one * size;

        var body = GetComponent<Rigidbody2D>();
        body.linearVelocity = direction.normalized * speed;
        body.angularVelocity = spinDegreesPerSecond;   // spin for looks (done by physics so movement stays smooth)

        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Already used up (Destroy happens at the end of the frame, so more touches can still arrive).
        if (hits > pierce) return;

        var zombie = other.GetComponentInParent<Zombie>();
        if (zombie == null || zombie.IsDead || !alreadyHit.Add(zombie)) return;

        zombie.TakeDamage(damage);
        hits++;
        if (hits > pierce) Destroy(gameObject);
    }
}
