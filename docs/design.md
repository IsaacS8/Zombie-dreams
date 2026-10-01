# Zombie Dreams: Game Design Doc

Version 0.2 · 2026-10-01

## Pitch

A small top-down 2D wave-survival game. You fall asleep with your cat on your shoulder, and zombies invade your dream. Your weapons fire on their own, the cat attacks too, and you pick upgrades as you level up. Survive 10 minutes to wake up.

Inspired by Megabonk and Vampire Survivors, but not a copy. This is a **beginner, vibe-coded project**, so the scope is kept small on purpose.

- Engine: Unity (2D), C#
- Single player only
- Art: pixel art
- Run length: 10 minutes

## What makes it distinct

- **The cat on your shoulder.** It attacks on its own, and some upgrades in the level-up pool are cat upgrades. The cat cannot be killed, but if you die, it goes down with you.
- **Cozy-creepy dream setting.** Sleepy zombies in pajamas, a giant bedroom as the arena, soft pastel colors. Not gory.

## Core loop

1. Move with WASD (or the left stick). Weapons and the cat attack automatically.
2. Zombies drop XP gems when they die. Walk over them to collect them.
3. When the XP bar fills, the game pauses and you pick 1 of 3 upgrades.
4. Zombies get more numerous and tougher every minute.
5. Survive to 10:00 to win ("You woke up!"). If your HP hits 0, the run ends ("Bad dream...").

There is no dash, no special buttons and no meta progression. Moving is the only skill.

## Player

- Stats: Max HP, Move Speed, Damage, Pickup Radius.
- Starts with one weapon (Pillow Toss).
- Can hold up to 4 weapons.

## The cat

- Sits on the player's shoulder the whole run and moves with them.
- Attacks the nearest zombie every couple of seconds (a scratch).
- Cannot take damage or die. When the player dies, the cat is gone too and the run ends.
- Has 3 upgrades in the level-up pool:
  - **Sharper Claws:** more scratch damage.
  - **Zoomies:** scratches more often.
  - **Purr:** slowly heals the player.

## Weapons (4)

| Weapon | Behavior |
|---|---|
| Pillow Toss | Throws a pillow at the nearest zombie |
| Night Light | A beam that spins around the player |
| Counting Sheep | Sheep circle the player and hit zombies |
| Alarm Clock | A shockwave around the player every few seconds |

Each weapon has 5 levels. Each level adds damage, or one more projectile, or a faster attack.

## Level-ups

- Each level-up offers 3 random choices from: new weapon, weapon level, cat upgrade, or a stat boost (Max HP, Move Speed, Damage, Pickup Radius).
- No rarities, rerolls or banishing.

## Zombies (3)

| Zombie | Behavior |
|---|---|
| Sleepwalker | Slow, walks straight at the player |
| Night Terror | Fast, low HP |
| Big Snorer | Slow, lots of HP, appears later |

Zombies spawn just off-screen in a ring around the player. The spawn rate goes up each minute.

## Level

One arena: **the Endless Bedroom**, a big carpet floor with a few toy blocks as obstacles. The floor tiles repeat so the player can roam freely.

## Screens

- Main menu (Play, Quit)
- HUD: HP bar, XP bar, timer
- Level-up screen (3 choice cards)
- Win screen and game-over screen (with a Play Again button)

## Tech notes (Unity)

- Unity 6, 2D template, built-in 2D physics.
- Zombies move straight toward the player; no pathfinding.
- Keep it simple: one script per thing (`PlayerMovement`, `Cat`, `Weapon`, `Zombie`, `Spawner`, `XPGem`, `LevelUpUI`, `GameManager`).
- Free pixel art packs (for example from itch.io or Kenney) are fine for placeholders.

## Build order

1. Player moves around the bedroom; camera follows.
2. Zombies spawn and chase the player; touching them hurts.
3. Pillow Toss kills zombies; zombies drop XP gems.
4. XP bar and the level-up screen.
5. The cat and its scratch attack.
6. The other 3 weapons and the 2 other zombie types.
7. Timer, win screen, game-over screen and main menu.

## Not doing

Co-op, multiple biomes, bosses, cat breeds, meta progression, save files, and online features.
