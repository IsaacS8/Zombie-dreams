# Builds the small item / effect sprite sheets from the text grids in grids/.
#   python build.py
# Sheets (each a single row of frames):
#   Pillow.png  1 frame   24x24  the thrown pillow (the game spins it)
#   Gem.png     4 frames  16x16  the XP "dream shard" twinkling
#   Poof.png    6 frames  48x48  the purple dream-smoke puff when a zombie vanishes
#   Slash.png   3 frames  32x32  Scarlet's claw-mark slash on a zombie
#   Beam.png    2 frames  96x16  the Night Light beam (points right; the game spins it)
#   Sheep.png   3 frames  24x24  a Counting Sheep, side view facing right
#   Ring.png    3 frames  64x64  the Alarm Clock shockwave ring (the game grows it)
# Colors come from the shared palette in ../isaac-jr/palette.py.
import os
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.join(HERE, "..", "isaac-jr"))
from PIL import Image  # noqa: E402
from palette import PALETTE  # noqa: E402
from preview import load_grid, grid_to_image  # noqa: E402

ART = os.path.join(HERE, "..", "..", "ZombieDreams", "Assets", "Art")
SHEETS = {
    "Pillow": {"prefix": "pillow", "frames": 1, "size": 24},
    "Gem": {"prefix": "gem", "frames": 4, "size": 16},
    "Poof": {"prefix": "poof", "frames": 6, "size": 48},
    "Slash": {"prefix": "slash", "frames": 3, "size": 32},
    "Beam": {"prefix": "beam", "frames": 2, "size": (96, 16)},    # Night Light beam, pointing right
    "Sheep": {"prefix": "sheep", "frames": 3, "size": 24},        # Counting Sheep (side view, facing right)
    "Ring": {"prefix": "ring", "frames": 3, "size": 64},          # Alarm Clock shockwave ring
}


def build(only=None):
    problems = []
    for name, cfg in SHEETS.items():
        if only and name not in only:
            continue
        size = cfg["size"]
        w, h = size if isinstance(size, tuple) else (size, size)
        sheet = Image.new("RGBA", (w * cfg["frames"], h), (0, 0, 0, 0))
        for i in range(cfg["frames"]):
            gname = f"{cfg['prefix']}_{i}"
            path = os.path.join(HERE, "grids", gname + ".txt")
            if not os.path.exists(path):
                problems.append(f"missing {gname}.txt")
                continue
            rows = load_grid(path)
            if len(rows) != h or any(len(r) != w for r in rows):
                problems.append(f"{gname}: must be {w}x{h}")
            for y, row in enumerate(rows):
                for x, ch in enumerate(row):
                    if ch not in PALETTE:
                        problems.append(f"{gname}: unknown character {ch!r} at x={x}, y={y}")
            sheet.alpha_composite(grid_to_image(rows), (i * w, 0))
        if not any(p.startswith(cfg["prefix"]) or f"missing {cfg['prefix']}" in p for p in problems):
            out = os.path.join(ART, name, name + ".png")
            os.makedirs(os.path.dirname(out), exist_ok=True)
            sheet.save(out)
            print("wrote", os.path.normpath(out))
    if problems:
        print("PROBLEMS:\n  - " + "\n  - ".join(problems))
        return False
    return True


if __name__ == "__main__":
    sys.exit(0 if build(sys.argv[1:] or None) else 1)
