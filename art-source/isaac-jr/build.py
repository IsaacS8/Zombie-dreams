# Builds Isaac Jr.'s sprite sheet from the text grids.
#
#   python build.py            -> writes the sheet into the Unity project + a big preview
#   python build.py --check    -> only checks the grids for mistakes
#
# How it works:
#   * grids/body_<view>_<anim><n>.txt are 48x48 pictures of Isaac Jr. (no cat).
#   * grids/cat_<view>_<n>.txt are small pictures of Scarlet.
#   * layout.json says, for each of the 8 facing directions, which body view to use
#     (left-side directions reuse a right-side body, mirrored), which Scarlet picture to
#     put on his shoulder, where, and whether she's drawn in front of or behind him.
#   * The sheet has one row per direction and one column per frame (idle0, idle1, walk0..walk3).
import json
import os
import sys
from PIL import Image
from palette import PALETTE, FRAME_SIZE
from preview import load_grid, grid_to_image, render

HERE = os.path.dirname(os.path.abspath(__file__))
GRIDS = os.path.join(HERE, "grids")
UNITY_OUT = os.path.join(HERE, "..", "..", "ZombieDreams", "Assets", "Art", "IsaacJr", "IsaacJr.png")
PREVIEW_OUT = os.path.join(HERE, "preview", "sheet_preview.png")


def grid_path(name):
    return os.path.join(GRIDS, name + ".txt")


def check_grid(name, size=None, problems=None):
    path = grid_path(name)
    if not os.path.exists(path):
        problems.append(f"missing grid: {name}.txt")
        return None
    rows = load_grid(path)
    for y, row in enumerate(rows):
        for x, ch in enumerate(row):
            if ch not in PALETTE:
                problems.append(f"{name}: unknown character {ch!r} at x={x}, y={y}")
    if size:
        if len(rows) != size or any(len(r) != size for r in rows):
            widths = sorted(set(len(r) for r in rows))
            problems.append(f"{name}: must be {size}x{size}, got {len(rows)} rows with widths {widths}")
    return rows


def frame_names(layout):
    names = []
    for anim, count in layout["animations"].items():
        names += [f"{anim}{i}" for i in range(count)]
    return names


def build(check_only=False):
    with open(os.path.join(HERE, "layout.json")) as f:
        layout = json.load(f)
    problems = []
    frames = frame_names(layout)
    sheet = Image.new("RGBA", (FRAME_SIZE * len(frames), FRAME_SIZE * len(layout["directions"])), (0, 0, 0, 0))
    previews, labels = [], []

    for row, direction in enumerate(layout["directions"]):
        spec = layout["layout"][direction]
        for col, frame in enumerate(frames):
            anim = frame.rstrip("0123456789")
            index = int(frame[len(anim):])
            body_rows = check_grid(f"body_{spec['body']}_{frame}", FRAME_SIZE, problems)
            if body_rows is None:
                continue
            body = grid_to_image(body_rows)
            if spec.get("mirror"):
                body = body.transpose(Image.FLIP_LEFT_RIGHT)

            img = Image.new("RGBA", (FRAME_SIZE, FRAME_SIZE), (0, 0, 0, 0))
            cat_img = None
            if spec.get("cat"):
                cat_frame = layout["cat_frames"][anim][index]
                cat_rows = check_grid(f"cat_{spec['cat']}_{cat_frame}", None, problems)
                if cat_rows is not None:
                    cat_img = grid_to_image(cat_rows)
                    if spec.get("cat_mirror"):
                        cat_img = cat_img.transpose(Image.FLIP_LEFT_RIGHT)
                    cx, cy = spec["cat_pos"]
                    cy += layout["shoulder_bob"][anim][index]
            if cat_img is not None and spec.get("cat_layer") == "behind":
                img.alpha_composite(cat_img, (cx, cy))
            img.alpha_composite(body)
            if cat_img is not None and spec.get("cat_layer") != "behind":
                img.alpha_composite(cat_img, (cx, cy))

            sheet.alpha_composite(img, (col * FRAME_SIZE, row * FRAME_SIZE))
            previews.append(img)
            labels.append(f"{direction} {frame}")

    if problems:
        print("PROBLEMS:")
        for p in problems:
            print("  -", p)
    else:
        print("all grids OK")
    if check_only:
        return not problems

    os.makedirs(os.path.dirname(UNITY_OUT), exist_ok=True)
    sheet.save(UNITY_OUT)
    os.makedirs(os.path.dirname(PREVIEW_OUT), exist_ok=True)
    big = sheet.resize((sheet.width * 4, sheet.height * 4), Image.NEAREST)
    bg = Image.new("RGBA", big.size, (236, 232, 246, 255))
    bg.alpha_composite(big)
    bg.save(PREVIEW_OUT)
    print("wrote", os.path.normpath(UNITY_OUT))
    print("wrote", os.path.normpath(PREVIEW_OUT))
    return not problems


if __name__ == "__main__":
    ok = build(check_only="--check" in sys.argv)
    sys.exit(0 if ok else 1)
