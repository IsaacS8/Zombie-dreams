using UnityEngine;

// PlayerStats: all of Isaac Jr.'s upgradable numbers live here, in ONE place.
// Weapons and pickups read these, so a single upgrade card helps everything at once.
// (Build step 4 adds the level-up cards that change these numbers.)
public class PlayerStats : MonoBehaviour
{
    // The five attack stats from the design doc. They affect every weapon (and Scarlet later).
    public float damageMultiplier = 1f;          // "Bad Dream":     +15% damage per pick
    public int extraProjectiles = 0;             // "Double Vision": +1 pillow per throw per pick
    public float projectileSpeedMultiplier = 1f; // "Sleep Sprint":  +15% projectile speed per pick
    public float attackSpeedMultiplier = 1f;     // "Caffeine":      +12% attack speed per pick
    public float attackSizeMultiplier = 1f;      // "Big Dreams":    +15% attack size per pick

    // Survival stats.
    public float moveSpeedMultiplier = 1f;       // "Fuzzy Slippers": +10% walking speed per pick

    // How close an XP gem has to be before it flies to Isaac Jr.
    public float pickupRadius = 1.5f;

    // Where Isaac Jr.'s feet are on the ground. Gems and zombies measure distance from here.
    // (The sprite's center is about 0.6 units above his feet.)
    public Vector2 FeetPosition
    {
        get { return (Vector2)transform.position + Vector2.down * 0.6f; }
    }
}
