using UnityEngine;

// PlayerXP: collects experience from XP gems and tracks the level.
// For now it just shows a bar under the HP bar. Build step 4 adds the level-up screen
// (the game pauses and you pick 1 of 3 upgrades) by listening to LeveledUp.
public class PlayerXP : MonoBehaviour
{
    public int level = 1;
    public int xp = 0;

    // Other scripts can subscribe to this to find out when a level-up happens.
    public event System.Action<int> LeveledUp;

    // How much XP is needed to get from this level to the next one: 5, 8, 11, 14...
    public int XpNeeded { get { return 5 + 3 * (level - 1); } }

    public void AddXP(int amount)
    {
        var health = GetComponent<PlayerHealth>();
        if (health != null && health.IsDead) return;   // no XP for a dead player

        xp += amount;
        while (xp >= XpNeeded)
        {
            xp -= XpNeeded;
            level++;
            if (LeveledUp != null) LeveledUp(level);
        }
    }

    // A simple XP bar (build step 7 replaces this with a real HUD).
    void OnGUI()
    {
        float scale = Screen.height / 540f;
        var back = new Rect(16 * scale, 44 * scale, 220 * scale, 14 * scale);
        var fill = new Rect(back.x + 2 * scale, back.y + 2 * scale,
                            (back.width - 4 * scale) * ((float)xp / XpNeeded), back.height - 4 * scale);

        GUI.color = new Color(0.1f, 0.05f, 0.15f, 0.85f);
        GUI.DrawTexture(back, Texture2D.whiteTexture);
        GUI.color = new Color(0.4f, 0.85f, 0.9f, 1f);
        GUI.DrawTexture(fill, Texture2D.whiteTexture);
        GUI.color = Color.white;

        var style = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(12 * scale), alignment = TextAnchor.MiddleLeft };
        GUI.Label(new Rect(back.xMax + 8 * scale, back.y - 4 * scale, 80 * scale, 22 * scale), "Lv " + level, style);
    }
}
