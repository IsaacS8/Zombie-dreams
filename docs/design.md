# Zombie Dreams: Game Design Doc

Version 0.3 · 2026-10-01

## Pitch

A small top-down 2D wave-survival game. You play as **Isaac Jr.**, asleep with the cat **Scarlet** on your shoulder, and zombies invade your dream. Your weapons fire on their own, the cat attacks too, and you pick upgrades as you level up. Survive 10 minutes to wake up.

Inspired by Megabonk and Vampire Survivors, but not a copy. This is a **beginner, vibe-coded project**, so the scope is kept small on purpose.

- Engine: Unity (2D), C#
- Single player only
- Art: pixel art
- Run length: 10 minutes

## What makes it distinct

- **Scarlet, the cat on your shoulder.** She attacks on her own, and some upgrades in the level-up pool are for her. She cannot be killed, but if you die, she goes down with you.
- **Cozy-creepy dream setting.** Sleepy zombies in pajamas, a giant bedroom as the arena, soft pastel colors. Not gory.

## Core loop

1. Move with WASD (or the left stick). Weapons and the cat attack automatically.
2. Zombies drop XP gems when they die. Walk over them to collect them.
3. When the XP bar fills, the game pauses and you pick 1 of 3 upgrades.
4. Zombies get more numerous and tougher every minute.
5. Survive to 10:00 to win ("You woke up!"). If your HP hits 0, the run ends ("Bad dream...").

There is no dash, no special buttons and no meta progression. Moving is the only skill.

## Player: Isaac Jr.

- Survival stats: Max HP, Move Speed, Pickup Radius.
- Attack stats (see Scaling below): Damage, Projectile Count, Projectile Speed, Attack Speed, Attack Size.
- Starts with one weapon (Pillow Toss).
- Can hold up to 4 weapons.

## The cat: Scarlet

- Sits on Isaac Jr.'s shoulder for the whole run.
- Attacks the nearest zombie every couple of seconds (a scratch).
- Cannot take damage or die. When Isaac Jr. dies, Scarlet goes down too and the run ends.
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

## Projectiles

- Pillow Toss fires real projectiles: each one flies toward the nearest zombie, hits it, and disappears (or pierces at higher levels).
- Projectiles are one reusable prefab: a sprite, a trigger collider and a small `Projectile` script with damage, speed, size and lifetime.
- Scarlet's scratch and Counting Sheep use the same stats, so every upgrade below helps them too.

## Scaling

These five attack stats apply to **every** weapon and to Scarlet, so a run can snowball into a screen full of attacks. They show up as stat cards on level-up.

| Stat | Card name | Effect per pick | What it changes |
|---|---|---|---|
| Damage | Bad Dream | +15% | Damage of every hit |
| Projectile Count | Double Vision | +1 | Extra pillows per throw, extra sheep, extra scratches |
| Projectile Speed | Sleep Sprint | +15% | How fast pillows fly |
| Attack Speed | Caffeine | +12% | Weapons and Scarlet attack more often |
| Attack Size | Big Dreams | +15% | Bigger pillows, longer beam, wider shockwave |

Each card can be picked up to 5 times. In code, each weapon reads these from one `PlayerStats` script, for example `cooldown = baseCooldown / attackSpeed`.

## Level-ups

- Each level-up offers 3 random choices from: new weapon, weapon level, cat upgrade, or a stat card (Max HP, Move Speed, Pickup Radius, or one of the five scaling stats).
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

## Effects (juice)

All of these use built-in Unity tools, with no extra packages to buy. **Easy** means a few lines of code or a few settings; **Medium** means a short script plus some setup in the editor.

**Particles** (Unity's Particle System)
| Effect | Difficulty | Description |
|---|---|---|
| Feather burst | Easy | Feathers pop out when a pillow hits |
| Zombie poof | Easy | Zombies vanish in a puff of purple dream smoke |
| XP sparkle | Easy | Gems twinkle, and sparkle when collected |
| Level-up burst | Easy | A ring of stars around Isaac Jr. on level-up |
| Floating Zzz | Easy | Little "Z" letters drift up from Snorers |
| Scratch slash | Easy | A quick claw-mark sprite where Scarlet hits |
| Footstep dust | Easy | Small puffs behind Isaac Jr. while walking |

**Hit feedback**
| Effect | Difficulty | Description |
|---|---|---|
| Hit flash | Easy | Zombies flash white for a split second when hit |
| Screen shake | Easy | Small camera shake when Isaac Jr. takes damage |
| Damage numbers | Medium | Numbers pop up and float away from hits |
| Knockback | Easy | Zombies get pushed back a little when hit |

**Lighting and shading** (URP 2D lights and post-processing)
| Effect | Difficulty | Description |
|---|---|---|
| Night darkness | Easy | Dim the whole scene with a dark global light |
| Glow around Isaac Jr. | Easy | A soft light that follows the player, so the edges are dark and spooky |
| Glowing gems and Night Light | Easy | XP gems and the Night Light beam give off light |
| Bloom | Easy | Bright things glow softly, which looks very dreamy |
| Vignette | Easy | Dark edges around the screen; turns red at low HP |
| Color grading | Easy | One dreamy purple and blue color mood for the whole game |
| Sprite outlines | Medium | An outline shader so Isaac Jr. and Scarlet stay visible in crowds |

**Weather and atmosphere**
| Effect | Difficulty | Description |
|---|---|---|
| Floating dream dust | Easy | Slow glowing specks drift across the screen |
| Falling feathers | Easy | Feathers drift down like snow |
| Dream fog | Medium | Soft scrolling fog sprites over the floor |
| Rain storm | Medium | Rain particles and screen flashes, for example as a "nightmare" event late in a run |

Suggested order: hit flash, zombie poof, XP sparkle, night darkness with a player light, then bloom. Those five give the biggest upgrade in look for the least work.

## Tech notes (Unity)

- Unity 6, **Universal 2D** template (URP), which is needed for 2D lights and bloom. Built-in 2D physics.
- Zombies move straight toward the player; no pathfinding.
- Keep it simple: one script per thing (`PlayerMovement`, `Cat`, `Weapon`, `Zombie`, `Spawner`, `XPGem`, `LevelUpUI`, `GameManager`).
- Free pixel art packs (for example from itch.io or Kenney) are fine for placeholders.

## Build order

1. Player moves around the bedroom; camera follows.
2. Zombies spawn and chase the player; touching them hurts.
3. Pillow Toss kills zombies; zombies drop XP gems.
4. XP bar, the level-up screen and the scaling stat cards.
5. The cat and its scratch attack.
6. The other 3 weapons and the 2 other zombie types.
7. Timer, win screen, game-over screen and main menu.
8. Effects pass, from the suggested order above.

## Not doing

Co-op, multiple biomes, bosses, cat breeds, meta progression, save files, and online features.
