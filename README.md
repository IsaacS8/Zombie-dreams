# Zombie-dreams
test game on unity vibe coding

The design doc is in [docs/design.md](docs/design.md).

## Opening the game

1. Open **Unity Hub**, click **Add** > **Add project from disk**, and pick the `ZombieDreams` folder inside this repo.
2. Open it with **Unity 6000.6.3f1**. The first open takes a few minutes while Unity rebuilds its `Library` folder.
3. In the **Project** window, open `Assets/Scenes/Bedroom`.
4. Press **Play**. Move Isaac Jr. with WASD, the arrow keys, or a gamepad's left stick.

## What's in the project (step 1)

| File | What it does |
|---|---|
| `Assets/Scripts/PlayerMovement.cs` | Reads WASD / arrows / left stick with the new Input System and moves Isaac Jr.'s Rigidbody2D. Change `Move Speed` in the Inspector. |
| `Assets/Scripts/CameraFollow.cs` | On the Main Camera. Smoothly glides after the player. Lower `Smooth Time` for a snappier camera. |
| `Assets/Scenes/Bedroom.unity` | The arena: a 100x100 tiled carpet floor, 8 toy blocks you bump into, Isaac Jr., and the camera. |
| `Assets/Scripts/PlayerAnimator.cs` | Picks which picture of Isaac Jr. to show: 8 facing directions, 2 idle frames, 4 walk frames. |
| `Assets/Art/IsaacJr/IsaacJr.png` | Isaac Jr. (pajamas, no sword) with Scarlet the cat on his shoulder: 8 directions x 6 frames, 48x48 each. Generated, see below. |
| `Assets/Art/` | Placeholder toy block and carpet tile. |
| `art-source/isaac-jr/` | The source for Isaac Jr. and Scarlet: text-grid sprites in `grids/` (one letter = one pixel, colors in `palette.py`), `layout.json` (which view and where Scarlet sits per direction). |

![Step 1 preview](docs/step1-preview.png)

## Rebuilding Isaac Jr.'s sprite sheet

Needs Python with Pillow (`pip install pillow`). From `art-source/isaac-jr`:

```
python build.py          # full build once every frame is painted
python build_rough.py    # fills any missing frame with a stand-in, so it always works
```

Both write `ZombieDreams/Assets/Art/IsaacJr/IsaacJr.png`. Unity re-imports it automatically.
