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
| `Assets/Art/` | 16x16 placeholder pixel art (Isaac Jr., toy block, carpet tile). Swap in real art later. |

![Step 1 preview](docs/step1-preview.png)
