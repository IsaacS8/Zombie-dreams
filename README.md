# Zombie Dreams

A small top-down 2D wave-survival game made in Unity as a beginner project. You are **Isaac Jr.**, asleep with the cat **Scarlet** on your shoulder, and sleepy pajama zombies invade your dream. Your weapons fire on their own, you pick upgrades as you level up, and you keep walking to survive. Pixel art, single player.

## Status: finished as a beginner project

It runs and plays. Playtested to level 32.

**Built (design doc build steps 1 to 6):**
- Isaac Jr. walks around a big bedroom (WASD or gamepad) with a smooth follow camera and 8-direction animated pixel art
- Three zombie kinds that chase you: Sleepwalker, Night Terror (fast, from minute 1) and Big Snorer (huge, from minute 3)
- Four auto weapons with 5 levels each: Pillow Toss, Night Light, Counting Sheep, Alarm Clock
- Scarlet the cat scratches zombies on her own, with 3 upgrade cards of her own
- XP gems, a level-up screen with 3 random upgrade cards (11 stat/cat cards plus weapon cards)
- HP bar, XP bar, "Bad dream..." and a restart when you go down

**Not built (left out on purpose):**
- Step 7: the 10-minute timer, the "You woke up!" win screen, a proper game-over screen and the main menu. For now the game runs until you die, then restarts.
- Step 8: effects (screen shake, lights, bloom, particles) and sound
- Ideas if you ever come back: sound from a free CC0 pack, card icons, a bigger difficulty curve late in the run, the design doc's lighting and weather

The full plan is in [docs/design.md](docs/design.md), and everything left to do, in order, is in **[docs/TODO.md](docs/TODO.md)** (start there if you come back).

## Getting this onto GitHub's main branch

The work was built as stacked branches (`claude/step1-player-movement` up to `claude/step6-weapons-zombies`), each one on top of the last. The last branch contains everything, so you only need ONE pull request: `claude/step6-weapons-zombies` into `main`. You can ignore the five smaller ones.

The design doc is in [docs/design.md](docs/design.md).

## Opening the game

1. Open **Unity Hub**, click **Add** > **Add project from disk**, and pick the `ZombieDreams` folder inside this repo (on Isaac's PC: `D:\claude\Zombie\Zombie-dreams\ZombieDreams`).
2. Open it with **Unity 6000.6.3f1**. The first open takes a few minutes while Unity rebuilds its `Library` folder.
3. In the **Project** window, open `Assets/Scenes/Bedroom`.
4. Press **Play**. Move Isaac Jr. with WASD, the arrow keys, or a gamepad's left stick.

## What's in the project (steps 1-6)

| File | What it does |
|---|---|
| `Assets/Scripts/PlayerMovement.cs` | Reads WASD / arrows / left stick with the new Input System and moves Isaac Jr.'s Rigidbody2D. Change `Move Speed` in the Inspector. |
| `Assets/Scripts/CameraFollow.cs` | On the Main Camera. Smoothly glides after the player. Lower `Smooth Time` for a snappier camera. |
| `Assets/Scenes/Bedroom.unity` | The arena: a 100x100 tiled carpet floor, 8 toy blocks you bump into, Isaac Jr., and the camera. |
| `Assets/Scripts/Zombie.cs` | One script for all three zombies (the numbers are on each prefab): **Sleepwalker** (speed 1.8, 20 HP), **Night Terror** (fast: speed 3.4, 10 HP) and **Big Snorer** (slow: speed 1, 100 HP, hits for 20). They walk straight at Isaac Jr., hurt him on touch, and leave a poof and an XP gem. |
| `Assets/Scripts/Weapon.cs` | The shared base for all four weapons: a level from 0 (not owned) to 5, a name, and a description of what each level gives. |
| `Assets/Scripts/PillowToss.cs` | Isaac Jr.'s starting weapon. Throws a pillow at the nearest zombie every 0.8 s, all by itself. Levels add damage, a second pillow, speed and piercing. |
| `Assets/Scripts/NightLight.cs` | Weapon: beams of light spin around Isaac Jr. and hurt zombies they sweep over. Levels add damage, beams and spin speed. |
| `Assets/Scripts/CountingSheep.cs` | Weapon: sheep hop in a circle around Isaac Jr. and bonk zombies. Levels add damage, sheep and speed. |
| `Assets/Scripts/AlarmClock.cs` | Weapon: a shockwave bursts out every few seconds and hurts every zombie inside it. Levels add size, damage and frequency. |
| `Assets/Scripts/SpriteLoop.cs`, `RingEffect.cs` | Small helpers: a looping sprite animation (sheep hop, beam flicker) and the growing shockwave ring. |
| `Assets/Scripts/Projectile.cs` | The reusable flying attack (pillows now; Scarlet and other weapons later): flies straight, spins, hurts zombies it touches. |
| `Assets/Scripts/PlayerStats.cs` | All the upgradable numbers in one place (damage, projectile count, projectile speed, attack speed, attack size, pickup radius). Step 4's level-up cards change these. |
| `Assets/Scripts/XPGem.cs` | The "dream shard" a defeated zombie drops. Flies to Isaac Jr. when he gets close and gives him XP. |
| `Assets/Scripts/PlayerXP.cs` | Collects XP, tracks the level, shows a simple XP bar. |
| `Assets/Scripts/LevelUpUI.cs` | The level-up screen: the game pauses and offers 3 random upgrade cards (pick with the mouse, keys 1-3, or arrows/d-pad + Enter/A). Stat cards (the 5 attack stats, Max HP, Move Speed, Pickup Radius), Scarlet's 3 cards, each up to 5 times, plus weapon cards: a new weapon, or the next level of one you have. |
| `Assets/Scripts/Cat.cs` | Scarlet. Every 1.6 s she scratches the nearest zombie within 3 units (claw-mark effect), using the same attack stats as the pillows. She can't be hurt. |
| `Assets/Scripts/OneShotAnimation.cs` | Plays the purple "poof" puff once and removes it. |
| `Assets/Scripts/Spawner.cs` | Spawns zombies on a ring just off-screen. Starts at 0.5 per second and speeds up by 0.3 per second every minute (max 150 alive). Its `Entries` list says which zombies appear and when: Night Terrors from minute 1, Big Snorers from minute 3 (at most 4 at once). All numbers are in the Inspector. |
| `Assets/Scripts/PlayerHealth.cs` | 100 HP, a short invulnerable blink after each hit, a simple HP bar, and "Bad dream..." then a restart at 0 HP. (The real HUD and game-over screens come in step 7.) |
| `Assets/Scripts/YSort.cs` | Draws things lower on screen in front of things higher up, so you can walk behind toy blocks and zombies. |
| `Assets/Prefabs/` | `Sleepwalker` (the zombie, the Spawner copies it), `NightTerror`, `BigSnorer`, `Pillow`, `Beam`, `Sheep`, `Ring`, `XPGem`, `Poof` and `Slash` (Scarlet's claw marks). |
| `Assets/Scripts/PlayerAnimator.cs` | Picks which picture of Isaac Jr. to show: 8 facing directions, 2 idle frames, 4 walk frames. |
| `Assets/Art/IsaacJr/IsaacJr.png` | Isaac Jr. (pajamas, no sword) with Scarlet the cat on his shoulder: 8 directions x 6 frames, 48x48 each. Generated, see below. |
| `Assets/Art/` | Placeholder toy block and carpet tile. |
| `art-source/nightterror/`, `art-source/bigsnorer/` | The two new zombies (`python build.py` in each). |
| `art-source/items/` | The pillow, XP gem, poof, slash, beam, sheep and ring sprites. `python build.py` there rebuilds their sheets in `Assets/Art/`. |
| `art-source/sleepwalker/` | The Sleepwalker zombie, same text-grid method. `python build.py` there rebuilds `Assets/Art/Sleepwalker/Sleepwalker.png`. |
| `art-source/isaac-jr/` | The source for Isaac Jr. and Scarlet: text-grid sprites in `grids/` (one letter = one pixel; the shared 30-color palette is `palette.py`), `layout.json` (which view and where Scarlet sits per direction). |

![Step 1 preview](docs/step1-preview.png)

## Rebuilding Isaac Jr.'s sprite sheet

Needs Python with Pillow (`pip install pillow`). From `art-source/isaac-jr`:

```
python build.py          # full build once every frame is painted
python build_rough.py    # fills any missing frame with a stand-in, so it always works
```

Both write `ZombieDreams/Assets/Art/IsaacJr/IsaacJr.png`. Unity re-imports it automatically.
