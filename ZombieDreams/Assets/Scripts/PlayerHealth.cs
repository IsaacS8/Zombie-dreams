using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// PlayerHealth: how much damage Isaac Jr. can take before the dream ends.
// Zombies call TakeDamage() when they touch him.
// For now the HP bar is drawn with simple code (OnGUI). A proper HUD comes in build step 7.
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;

    // After a hit, Isaac Jr. can't be hurt again for this many seconds,
    // so a crowd of zombies doesn't melt him instantly.
    public float invulnerableSeconds = 0.6f;

    // When he dies, wait this long, then restart the scene.
    public float restartDelay = 3f;

    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    private SpriteRenderer spriteRenderer;
    private float invulnerableUntil;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead || Time.time < invulnerableUntil) return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        invulnerableUntil = Time.time + invulnerableSeconds;

        if (CurrentHealth <= 0f) Die();
        else StartCoroutine(HitFlash());
    }

    // Blink red and see-through for the invulnerable time, so you can tell you got hit.
    IEnumerator HitFlash()
    {
        float end = invulnerableUntil;
        while (Time.time < end)
        {
            spriteRenderer.color = new Color(1f, 0.45f, 0.45f, 0.6f);
            yield return new WaitForSeconds(0.08f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.08f);
        }
        spriteRenderer.color = Color.white;
    }

    void Die()
    {
        IsDead = true;
        StopAllCoroutines();   // stop the hit blink so it can't undo the tint below
        spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);

        // Stop walking and animating.
        var movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;
        var body = GetComponent<Rigidbody2D>();
        if (body != null) body.linearVelocity = Vector2.zero;

        Invoke(nameof(Restart), restartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // A simple HP bar in the top-left corner (build step 7 replaces this with a real HUD).
    void OnGUI()
    {
        float scale = Screen.height / 540f;
        var back = new Rect(16 * scale, 16 * scale, 220 * scale, 22 * scale);
        var fill = new Rect(back.x + 3 * scale, back.y + 3 * scale,
                            (back.width - 6 * scale) * (CurrentHealth / maxHealth), back.height - 6 * scale);

        GUI.color = new Color(0.1f, 0.05f, 0.15f, 0.85f);
        GUI.DrawTexture(back, Texture2D.whiteTexture);
        GUI.color = new Color(0.95f, 0.35f, 0.45f, 1f);
        GUI.DrawTexture(fill, Texture2D.whiteTexture);
        GUI.color = Color.white;

        var style = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(14 * scale), alignment = TextAnchor.MiddleCenter };
        GUI.Label(back, Mathf.CeilToInt(CurrentHealth) + " / " + Mathf.CeilToInt(maxHealth), style);

        if (IsDead)
        {
            var big = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(48 * scale),
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
            };
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Bad dream...", big);
        }
    }
}
