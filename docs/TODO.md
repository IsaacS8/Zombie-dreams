# Zombie Dreams: to-do list

Where things stand: the game **runs and plays** (steps 1 to 6 of the build order in [design.md](design.md) are done and tested). This list is everything left, in the order I'd do it. Tick the boxes as you go.

## How to pick this back up

1. Open **Unity Hub**, **Add > Add project from disk**, pick the `ZombieDreams` folder in this repo (Unity **6000.6.3f1**). Open `Assets/Scenes/Bedroom` and press Play to see where things are.
2. Make sure you're on the branch with all the work: `claude/step6-weapons-zombies` (or `main` once that is merged).
3. Open Claude Code in this folder and say what you want from the list below, for example **"do step 7"**. Everything it needs is in [README.md](../README.md) (what each script does) and this file.
4. Tip for a beginner: do one section at a time, press Play after each, and commit when it works.

## Housekeeping (5 minutes)

- [ ] Open ONE pull request from `claude/step6-weapons-zombies` into `main` and merge it: https://github.com/IsaacS8/Zombie-dreams/compare/main...claude/step6-weapons-zombies?expand=1 (it has everything from steps 1 to 6; ignore the smaller stacked PRs).
- [ ] Optional: delete the old `claude/step1...step5` branches on GitHub after merging.

## Step 7: timer, win screen, game over, main menu (the biggest missing piece)

Right now the game runs until you die and then restarts. Step 7 turns it into a real run you can win.

- [ ] **10:00 timer.** A `GameManager` script that counts the run time and shows it at the top of the screen. (`Spawner` already reads `Time.timeSinceLevelLoad`, so the difficulty curve already follows the clock.)
- [ ] **Win screen "You woke up!"** at 10:00: stop spawning, clear or freeze the zombies, show the message and a **Play Again** button.
- [ ] **Game-over screen "Bad dream..."** with a **Play Again** button. Today `PlayerHealth.Die()` shows plain text and calls `Restart()` after 3 seconds; replace that.
- [ ] **Main menu scene** (Play, Quit): a new scene `MainMenu`, added first in **File > Build Profiles / Build Settings > Scenes In Build**. Play loads `Bedroom`.
- [ ] **Real HUD.** The HP bar, XP bar and "Lv" label are drawn with simple code in `PlayerHealth.cs` and `PlayerXP.cs` (`OnGUI`). Replace them with a proper uGUI Canvas (bars plus the timer), and the level-up cards in `LevelUpUI.cs` too if you want them prettier (icons for each card).
- [ ] **Pause menu** (Esc): optional, easy once the Canvas exists. Remember `LevelUpUI` already uses `Time.timeScale = 0`.

Heads-up for whoever does it: `Time.timeScale` stays 0 if a screen pauses the game and you then load a scene. `PlayerHealth.Restart()` already resets it; do the same for new screens.

## Step 8: effects ("juice") from the design doc

The design doc's effects list is in [design.md](design.md#effects-juice); its suggested order is below. Already done: the zombie **poof**, the gem **twinkle**, Scarlet's **scratch slash**.

- [ ] **Hit flash:** zombies flash white for a split second when hit (a tiny shader or swapping the sprite color).
- [ ] **Night darkness + a light around Isaac Jr.:** a dark Global Light 2D and a soft Point Light 2D on the player (the project already uses the URP 2D renderer).
- [ ] **Glowing gems and Night Light:** give the XP gem and the beam a light.
- [ ] **Bloom, vignette (red at low HP), dreamy purple color grading:** one Global Volume with post-processing.
- [ ] **Screen shake** when Isaac Jr. takes damage (`CameraFollow`).
- [ ] **Knockback** when zombies are hit; **damage numbers** (a bit harder).
- [ ] **Feather burst** when a pillow hits, **footstep dust** while walking, **floating Zzz** from Big Snorers, **level-up star burst**.
- [ ] **Weather and mood:** floating dream dust, falling feathers; fog and a "rain storm" nightmare event late in the run (harder).
- [ ] **Sprite outlines** so Isaac Jr. and Scarlet stay visible in crowds (needs a small shader).

## Sound and music (nothing exists yet)

- [ ] Sound effects: pillow throw/hit, zombie poof, gem pickup, level-up, hurt, win, lose, button clicks.
- [ ] A calm, dreamy music loop (and a tenser one for later in the run).
- [ ] Free sources worth checking (each pack has its own license, so read it and credit if required): Kenney.nl (CC0, no credit needed), OpenGameArt.org, Freesound.org, itch.io free assets. Pixel fonts: Google Fonts (Press Start 2P, Silkscreen).
- [ ] How: an `AudioManager` script with a method like `Play("pillow")`, called from `PillowToss`, `Zombie.Die`, `XPGem`, `LevelUpUI`, `PlayerHealth`.

## Balance and polish ideas

- [ ] **Difficulty:** at level 32 the weapons outgrew the zombies. Once the timer exists, make it tense: `Spawner` (spawn rate and its `Entries` list), zombie HP growing over time, more Big Snorers late. All the numbers are in the Inspector.
- [ ] **Weapon feel:** compare weapons and tune the numbers inside each weapon script (`PillowToss`, `NightLight`, `CountingSheep`, `AlarmClock`; each level's numbers are at the top of the file).
- [ ] **Scarlet:** her back-view art is a bit blobby; more cat upgrades; maybe let her catch things.
- [ ] **Art:** the Night Terror's airborne run frames still read like a standing stride; some of the draft art (slash, ring, beam) could be nicer. Sprite sources are in `art-source/`; see the README for how to rebuild sheets (`python build.py`).
- [ ] **More content from the original idea:** more weapons, more zombie types, a second arena (the design doc lists what we decided NOT to do: co-op, bosses, biomes, save files).

## Known quirks (not bugs you need to fix)

- The HP and XP bars use the older `OnGUI` drawing, so they don't show in camera screenshots (they are in the game).
- The player build step of Unity can change three settings files by itself (`UniversalRP.asset`, `UniversalRenderPipelineGlobalSettings.asset`, `ProjectSettings/Packages/com.unity.learn.iet-framework/Settings.json`); that is harmless.
- All the pixel art uses one 30-color palette (`art-source/isaac-jr/palette.py`); change a color there and rebuild the sprites.
