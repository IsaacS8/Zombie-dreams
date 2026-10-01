# Builds the zombie sprite sheet from the text grids in grids/.
#   python build.py
# One row of 4 walk frames (walk0..walk3), each 48x48, facing RIGHT.
# The game flips the sprite when the zombie walks left, so we only draw one direction.
# Colors come from the shared palette in ../isaac-jr/palette.py.
import os
import sys

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.join(HERE, "..", "isaac-jr"))
from PIL import Image  # noqa: E402
from palette import PALETTE  # noqa: E402
from preview import load_grid, grid_to_image  # noqa: E402

FRAMES = ["walk0", "walk1", "walk2", "walk3"]
SIZE = 64
OUT = os.path.join(HERE, "..", "..", "ZombieDreams", "Assets", "Art", "BigSnorer", "BigSnorer.png")


def main():
    sheet = Image.new("RGBA", (SIZE * len(FRAMES), SIZE), (0, 0, 0, 0))
    problems = []
    for i, name in enumerate(FRAMES):
        path = os.path.join(HERE, "grids", f"bigsnorer_{name}.txt")
        if not os.path.exists(path):
            problems.append(f"missing {path}")
            continue
        rows = load_grid(path)
        if len(rows) != SIZE or any(len(r) != SIZE for r in rows):
            problems.append(f"{name}: must be {SIZE}x{SIZE}")
        for y, row in enumerate(rows):
            for x, ch in enumerate(row):
                if ch not in PALETTE:
                    problems.append(f"{name}: unknown character {ch!r} at x={x}, y={y}")
        sheet.alpha_composite(grid_to_image(rows), (i * SIZE, 0))
    if problems:
        print("PROBLEMS:\n  - " + "\n  - ".join(problems))
        sys.exit(1)
    os.makedirs(os.path.dirname(OUT), exist_ok=True)
    sheet.save(OUT)
    print("wrote", os.path.normpath(OUT))


if __name__ == "__main__":
    main()
