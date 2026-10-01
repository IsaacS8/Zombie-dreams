using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// LevelUpUI: when Isaac Jr. levels up, the game PAUSES and offers 3 random upgrade cards.
// Pick one with the mouse, the keys 1 / 2 / 3, or the keyboard arrows / gamepad d-pad + Enter / A button.
// Every card can be picked up to 5 times. (New weapons and cat upgrades join the pool in later steps.)
// For now the cards are drawn with simple code (OnGUI); build step 7 can make them prettier.
public class LevelUpUI : MonoBehaviour
{
    // One upgrade card.
    class Card
    {
        public string name;
        public string description;
        public Action apply;       // what picking it does
        public int picks;          // how many times it has been picked
        public int maxPicks = 5;
    }

    private readonly List<Card> allCards = new List<Card>();
    private readonly List<Card> offered = new List<Card>();   // the 3 cards on screen right now

    private PlayerStats stats;
    private PlayerHealth health;
    private PlayerXP xp;

    private bool showing;
    private readonly Queue<int> pendingLevels = new Queue<int>();   // level-ups waiting for their turn (several can happen at once)
    private int currentLevel;
    private int openedFrame;       // the frame the cards appeared (so a key already held that frame doesn't pick)
    private int selected;          // highlighted card (for keyboard / gamepad)

    public bool IsShowing { get { return showing; } }

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        health = GetComponent<PlayerHealth>();
        xp = GetComponent<PlayerXP>();

        // The five attack stats from the design doc (they help every weapon and Scarlet):
        allCards.Add(new Card { name = "Bad Dream", description = "+15% damage", apply = () => stats.damageMultiplier += 0.15f });
        allCards.Add(new Card { name = "Double Vision", description = "+1 pillow per throw", apply = () => stats.extraProjectiles += 1 });
        allCards.Add(new Card { name = "Sleep Sprint", description = "+15% projectile speed", apply = () => stats.projectileSpeedMultiplier += 0.15f });
        allCards.Add(new Card { name = "Caffeine", description = "+12% attack speed", apply = () => stats.attackSpeedMultiplier += 0.12f });
        allCards.Add(new Card { name = "Big Dreams", description = "+15% attack size", apply = () => stats.attackSizeMultiplier += 0.15f });

        // The three survival cards:
        allCards.Add(new Card { name = "Warm Milk", description = "+20 max HP (and heals 20)", apply = () => health.IncreaseMaxHealth(20f) });
        allCards.Add(new Card { name = "Fuzzy Slippers", description = "+10% move speed", apply = () => stats.moveSpeedMultiplier += 0.10f });
        allCards.Add(new Card { name = "Dreamcatcher", description = "+30% pickup radius", apply = () => stats.pickupRadius *= 1.3f });
    }

    void OnEnable() { xp.LeveledUp += OnLeveledUp; }

    void OnDisable()
    {
        xp.LeveledUp -= OnLeveledUp;
        if (showing) Time.timeScale = 1f;   // never leave the game stuck paused
    }

    void OnLeveledUp(int newLevel)
    {
        pendingLevels.Enqueue(newLevel);
        if (!showing) ShowNext();
    }

    // Pauses the game and picks 3 random cards that haven't been maxed out.
    void ShowNext()
    {
        var available = allCards.FindAll(c => c.picks < c.maxPicks);
        if (available.Count == 0)
        {
            // Everything is maxed: heal a little instead.
            health.Heal(20f);
            pendingLevels.Clear();
            return;
        }

        currentLevel = pendingLevels.Peek();

        offered.Clear();
        for (int i = 0; i < 3 && available.Count > 0; i++)
        {
            int index = UnityEngine.Random.Range(0, available.Count);
            offered.Add(available[index]);
            available.RemoveAt(index);
        }

        selected = 0;
        showing = true;
        openedFrame = Time.frameCount;
        Time.timeScale = 0f;   // pause everything
    }

    void Pick(int index)
    {
        if (!showing || index < 0 || index >= offered.Count) return;

        Card card = offered[index];
        card.picks++;
        card.apply();

        pendingLevels.Dequeue();
        showing = false;
        Time.timeScale = 1f;   // resume

        // Another level-up waiting? Show its cards.
        if (pendingLevels.Count > 0) ShowNext();
    }

    // Update still runs while the game is paused (Time.timeScale = 0).
    void Update()
    {
        if (!showing || Time.frameCount == openedFrame) return;

        var keyboard = Keyboard.current;
        var gamepad = Gamepad.current;
        int count = offered.Count;

        if (keyboard != null)
        {
            if (keyboard.digit1Key.wasPressedThisFrame) Pick(0);
            else if (keyboard.digit2Key.wasPressedThisFrame) Pick(1);
            else if (keyboard.digit3Key.wasPressedThisFrame) Pick(2);
            else if (keyboard.leftArrowKey.wasPressedThisFrame) selected = (selected + count - 1) % count;
            else if (keyboard.rightArrowKey.wasPressedThisFrame) selected = (selected + 1) % count;
            else if (keyboard.enterKey.wasPressedThisFrame || keyboard.spaceKey.wasPressedThisFrame) Pick(selected);
        }
        if (showing && gamepad != null)
        {
            if (gamepad.dpad.left.wasPressedThisFrame) selected = (selected + count - 1) % count;
            else if (gamepad.dpad.right.wasPressedThisFrame) selected = (selected + 1) % count;
            else if (gamepad.buttonSouth.wasPressedThisFrame) Pick(selected);
        }
    }

    void OnGUI()
    {
        if (!showing) return;

        float scale = Screen.height / 540f;

        // Dim the game behind the cards.
        GUI.color = new Color(0.05f, 0.02f, 0.12f, 0.75f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;

        var title = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(36 * scale), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        GUI.Label(new Rect(0, 40 * scale, Screen.width, 50 * scale), "Level " + currentLevel + "!", title);
        var sub = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(16 * scale), alignment = TextAnchor.MiddleCenter };
        GUI.Label(new Rect(0, 92 * scale, Screen.width, 28 * scale), "Pick a dream upgrade", sub);

        float cardWidth = 210 * scale, cardHeight = 270 * scale, gap = 28 * scale;
        float totalWidth = offered.Count * cardWidth + (offered.Count - 1) * gap;
        float left = (Screen.width - totalWidth) / 2f;
        float top = 150 * scale;

        var nameStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(22 * scale), fontStyle = FontStyle.Bold, alignment = TextAnchor.UpperCenter, wordWrap = true };
        var descStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(16 * scale), alignment = TextAnchor.UpperCenter, wordWrap = true };
        var smallStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(13 * scale), alignment = TextAnchor.LowerCenter };

        for (int i = 0; i < offered.Count; i++)
        {
            Card card = offered[i];
            var rect = new Rect(left + i * (cardWidth + gap), top, cardWidth, cardHeight);

            // Card body (the highlighted one is brighter, with a border).
            if (i == selected)
            {
                GUI.color = new Color(1f, 0.9f, 0.5f, 1f);
                GUI.DrawTexture(new Rect(rect.x - 4 * scale, rect.y - 4 * scale, rect.width + 8 * scale, rect.height + 8 * scale), Texture2D.whiteTexture);
            }
            GUI.color = i == selected ? new Color(0.45f, 0.38f, 0.75f, 1f) : new Color(0.3f, 0.25f, 0.5f, 1f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;

            GUI.Label(new Rect(rect.x + 10 * scale, rect.y + 26 * scale, rect.width - 20 * scale, 70 * scale), card.name, nameStyle);
            GUI.Label(new Rect(rect.x + 14 * scale, rect.y + 110 * scale, rect.width - 28 * scale, 90 * scale), card.description, descStyle);
            GUI.Label(new Rect(rect.x, rect.y + 8 * scale, rect.width, 20 * scale), "[" + (i + 1) + "]", smallStyle);
            GUI.Label(new Rect(rect.x, rect.yMax - 32 * scale, rect.width, 24 * scale), "Picked " + card.picks + " / " + card.maxPicks, smallStyle);

            // Clicking anywhere on the card picks it.
            if (GUI.Button(rect, GUIContent.none, GUIStyle.none)) Pick(i);
        }
    }
}
