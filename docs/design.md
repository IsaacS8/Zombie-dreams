# Zombie Dreams: Game Design Doc

Version 0.1 · 2026-10-01

## Pitch

A top-down 2D wave-survival game. You fall asleep, and your cat comes with you into your dreams. Each night, the dream is invaded by zombies. You walk, dodge and pick your build; your weapons fire on their own, and the cat on your shoulder fights alongside you. Survive until morning to wake up.

Inspired by Megabonk and the wider "bullet heaven" genre (Vampire Survivors, Brotato), but built around one idea none of them have: **a companion you build together with**.

- Engine: Unity (2D, URP), C#
- Platforms: PC first (keyboard/mouse and gamepad), Steam Deck friendly
- Session length: one run (a "Night") lasts about 15 minutes

## What makes it distinct

1. **The cat is half your build.** The cat levels up separately, has its own upgrade pool, and can leave your shoulder to act on its own. Choices about the cat are as important as choices about your weapons.
2. **Dreams bend the rules.** Each biome has a "dream logic" twist that changes how the arena works (gravity of a room tilts, the floor loops, lights decide what is real). This is the main source of variety between runs, rather than just new enemy skins.
3. **Lucidity meter.** Instead of a plain timer, a Lucidity meter rises as you fight. At full Lucidity you can trigger a short Lucid Moment: time slows and you can redraw a small part of the arena (see Lucidity below).
4. **Waking up is the win state.** A run ends at dawn with a final "Nightmare" boss. Dying means you "wake in a sweat" and keep some progress (Dream Shards) for meta upgrades.
5. **Cozy-creepy tone.** Soft pastel dreamscapes, sleepy zombies in pajamas and office clothes, pillow-feather particles, lo-fi music that warps as danger rises. Not gore.

## Core loop

**Moment to moment (seconds)**
- Move with WASD or the left stick. Weapons fire automatically at the nearest or prioritized targets.
- One active input: **Dash** (short dodge with brief invulnerability, cooldown).
- One context input: **Cat command** (send the cat to a spot, or call it back).
- Zombies drop **Dream Motes** (XP). Walk over them or let the cat fetch them.

**Within a run (minutes)**
- Fill the XP bar to level up. Pick 1 of 3 offered upgrades (weapon, passive, or cat).
- Waves escalate every minute. Elite zombies appear every 3 minutes and drop chests.
- Mini-boss at 5 and 10 minutes. The Nightmare boss arrives at 14:00 and must be beaten by dawn (15:00).

**Between runs (meta)**
- Spend Dream Shards in the **Bedroom** hub: unlock new weapons, cat breeds, biomes, and small permanent stat boosts.
- Decorate the bedroom with unlocked trinkets (cosmetic, gives a sense of progress).

## The player

- Stats: Max HP, Move Speed, Armor, Pickup Radius, Cooldown, Area, Damage, Luck.
- Starts with one weapon chosen in the Bedroom before sleeping.
- Up to **6 weapon slots** and **6 passive slots**.

## The cat

The cat starts each run perched on the player's shoulder. It is never killed, but it can be **knocked off** and **stunned** (it naps for a few seconds) if the player takes a heavy hit.

**States**
- **Perched:** rides on the shoulder, uses its perched ability, and gives the player a small passive bonus.
- **Prowling:** sent out with the Cat command. Roams to a target point, attacks, and fetches motes. Returns automatically after a few seconds or when called.
- **Napping:** stunned or resting. No abilities; the player can pick it up by walking over it.

**Cat level and upgrades**
- The cat has its own XP bar, filled by kills the cat makes and motes it fetches.
- When the cat levels up, you pick 1 of 3 **cat cards**, separate from player level-ups.
- Cat cards fall into three families:
  - **Claws** (offense): pounce damage, scratch combos, bleed, chain pounce.
  - **Purr** (support): heal-over-time aura while perched, shield on dash, slow aura.
  - **Curiosity** (utility): faster fetch, mote magnet, reveals hidden chests, steals elite buffs.

**Cat breeds** (unlocked in meta, each with a different starting ability)
| Breed | Perched ability | Prowling behavior |
|---|---|---|
| Tabby (starter) | Hisses at the nearest zombie, briefly stunning it | Pounces on clusters |
| Black Cat | Crits on the player's attacks deal bonus damage | Teleports between shadows, high single-target damage |
| Siamese | Yowl that knocks back enemies in a ring | Taunts zombies away from the player |
| Fluffy Persian | Small damage shield that regenerates | Slow but very tanky, blocks paths |

**Synergy hooks:** some player upgrades evolve only if the cat has a matching card. Example: *Pillow Toss* + cat card *Kneading* evolves into *Featherstorm*.

## Weapons and passives

Weapons auto-fire. Each has 5 levels, then can **evolve** when paired with a specific passive (or cat card) and a boss chest is opened.

**Starting weapon pool (prototype target: 6)**
| Weapon | Behavior | Evolves with |
|---|---|---|
| Pillow Toss | Lobbed pillows, AoE on landing | Kneading (cat) → Featherstorm |
| Night Light | Beam that rotates around the player | Glow Charm → Sunrise Lamp |
| Counting Sheep | Sheep orbit the player and bump enemies | Lullaby → Stampede |
| Alarm Clock | Periodic shockwave around the player | Snooze (cat) → Doomsday Bell |
| Dream Catcher | Fires bouncing thread projectiles | Web of Fate → Constellation |
| Toothbrush Rapier | Fast forward stabs | Minty Fresh → Floss Whip |

**Passives (prototype target: 6):** Warm Socks (move speed), Weighted Blanket (armor), Lullaby (cooldown), Glow Charm (area), Big Pockets (pickup radius), Lucky Penny (luck).

## Level-up system

- XP curve: early levels fast (every 10–20 seconds), slowing so a full run reaches about level 40.
- Each level-up pauses the game and offers **3 cards**. Rarity: Common, Rare, Epic, Legendary (weighted by Luck).
- Options per pick: **Reroll** (limited charges), **Banish** (remove a card from this run's pool), **Skip** (gain a small heal).
- Cards can be: new weapon, weapon level, new passive, passive level, or stat boost when slots are full.
- Chests from elites give 1 random upgrade; boss chests can trigger an evolution if conditions are met.

## Lucidity

- The Lucidity meter fills from kills, dashing through attacks (near-misses), and cat fetches.
- At full, press the Lucid button for a **Lucid Moment** (about 4 seconds of slowed time). During it, pick one effect:
  - **Erase:** delete all regular zombies in a circle.
  - **Rewrite:** turn a patch of floor into a healing bed or a spike trap.
  - **Wake Call:** pull every mote on screen to the player.
- Gives the player a skill-based "panic button" that feels thematic and is unique to this game.

## Enemies

All zombies are dream figures: sleepwalkers, not gory undead.

| Zombie | Role |
|---|---|
| Sleepwalker | Basic slow chaser, comes in big crowds |
| Snorer | Emits slow-moving Z projectiles |
| Night Terror | Fast, low HP, leaps at the player |
| Sheep Herder | Buffs nearby zombies' speed |
| Pillow Fort | Tanky, blocks paths, splits into smaller zombies on death |
| Elite (any type) | Larger, glowing outline, drops a chest |

**Waves:** a spawn director increases the budget each minute and mixes types by biome. Spawns happen off-screen in a ring around the player. Cap on live enemies (for example 400) for performance, using pooling.

## Dream biomes

Each biome is one Night. The prototype ships with one; the rest come later.

1. **Endless Bedroom** (prototype). A giant version of your own bedroom: carpet plains, toy-block walls. *Dream logic:* the room slowly rotates, and the furniture (obstacles) shifts every few minutes. Boss: **The Thing Under the Bed**.
2. **Flooded School.** Hallways and classrooms half underwater. *Dream logic:* forgot-your-homework events spawn surprise tests (timed challenges with rewards). Boss: **The Substitute**.
3. **Upside-down City.** Rooftops and stairs to nowhere. *Dream logic:* the arena wraps around (exit one edge, enter the other). Boss: **Rush Hour**.
4. **Falling Forest.** Trees and lanterns. *Dream logic:* lights decide what is solid; dark zones hide zombies but boost your damage. Boss: **The Owl Who Never Sleeps**.
5. **The Nightmare.** Unlocked after beating all four. Mixes every rule. Boss: **Insomnia**.

## Difficulty and scoring

- Difficulty tiers ("Restless" sleep levels) unlock after the first dawn. Each adds a modifier (faster zombies, fewer heals, cursed cards).
- End-of-run screen: time survived, kills, cat MVP stats, Dream Shards earned.

## Art, audio and feel

- **Art:** pastel palette with deep blue night shadows. Pixel art or clean vector, decided in the art pass. Readable silhouettes: player and cat must stay visible in crowds (outline shader).
- **Juice:** hit flash, small screen shake, damage numbers (toggle), feather and mote particles, cat purr vibration on gamepad.
- **Audio:** lo-fi bedroom music that adds layers as waves grow and detunes when HP is low. Cat meows on level-up.

## Controls

| Action | Keyboard/Mouse | Gamepad |
|---|---|---|
| Move | WASD | Left stick |
| Dash | Space | A / Cross |
| Cat command | Right click (aim with mouse) | Right trigger (aim with right stick) |
| Lucid Moment | E | Y / Triangle |
| Pause | Esc | Start |

## Technical plan (Unity)

- **Unity 6 LTS**, 2D URP, new Input System.
- **Structure:** `Assets/Scripts/{Player, Cat, Enemies, Weapons, Upgrades, Waves, UI, Core}`.
- **Data-driven content:** weapons, passives, cat cards, enemies and waves are **ScriptableObjects**, so new content is added without code changes.
- **Performance:** object pooling for enemies, projectiles and motes; simple steering toward the player (no NavMesh needed for open arenas, flow field later if obstacles demand it); spatial hashing for collision queries if physics gets expensive.
- **Save:** JSON save for meta progress in `Application.persistentDataPath`.

## Prototype scope (next step)

Goal: one playable Night in the Endless Bedroom, about 10 minutes, ugly art allowed.

- Player movement and dash
- Tabby cat with perched hiss and prowling pounce, plus the Cat command
- 3 weapons (Pillow Toss, Night Light, Counting Sheep) and 3 passives
- 3 zombie types (Sleepwalker, Snorer, Night Terror) with a spawn director and an elite
- XP motes, level-up screen with 3 choices, cat level-up with 3 cat cards
- Basic HUD (HP, XP bar, cat XP bar, timer) and a game-over screen

Out of scope for the prototype: Lucidity, evolutions, meta progression, other biomes, bosses.

## Open questions

- Pixel art or vector art?
- Should the cat ever be able to die (hardcore mode)?
- Local co-op with a second player controlling the cat?
