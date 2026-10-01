# build_rough.py: a quick stand-in for build.py while not every frame is painted yet.
#   python build_rough.py
# For any frame that has no hand-painted grid yet, it makes one from a finished view:
#   right / left        -> reuse the 3/4 view (down_right), flipped for left
#   up_right / up_left  -> reuse the back view (up)
#   idle1               -> the upper body sinks 1 pixel
#   walk frames         -> a bounce (body hops 1 pixel up, and the pose flips left/right)
# As soon as the real grids exist (body_right_walk0.txt etc.) they are used instead,
# so just re-run this (or build.py once everything is painted).
import json
import os
from PIL import Image
from palette import FRAME_SIZE
from preview import load_grid, grid_to_image
import build as real

HERE = os.path.dirname(os.path.abspath(__file__))
FALLBACK_BASE = {"right": "down_right", "up_right": "up"}  # body views we don't have yet -> a stand-in
N = FRAME_SIZE


def exists(name):
    return os.path.exists(real.grid_path(name))


def rough_frame(body, frame):
    """Make a stand-in frame image from the finished idle0 view `body`."""
    base = grid_to_image(load_grid(real.grid_path(f"body_{body}_idle0")))
    px = base.load()
    out = Image.new("RGBA", (N, N), (0, 0, 0, 0))
    o = out.load()
    anim = frame.rstrip("0123456789")
    idx = int(frame[len(anim):])
    for y in range(N):
        for x in range(N):
            c = px[x, y]
            if c[3] == 0:
                continue
            ny, nx = y, x
            if anim == "idle" and idx == 1 and y <= 27:
                ny = y + 1                       # upper body sinks (breathing)
            if anim == "walk" and idx in (1, 3):
                ny = y - 1                       # hop
            if 0 <= ny < N and 0 <= nx < N:
                o[nx, ny] = c
    if anim == "walk" and idx in (2, 3):
        out = out.transpose(Image.FLIP_LEFT_RIGHT)
    return out


def build():
    layout = json.load(open(os.path.join(HERE, "layout.json")))
    frames = real.frame_names(layout)
    sheet = Image.new("RGBA", (N * len(frames), N * len(layout["directions"])), (0, 0, 0, 0))
    used_rough = 0
    for row, direction in enumerate(layout["directions"]):
        spec = layout["layout"][direction]
        for col, frame in enumerate(frames):
            anim = frame.rstrip("0123456789")
            index = int(frame[len(anim):])
            name = f"body_{spec['body']}_{frame}"
            if exists(name):
                body = grid_to_image(load_grid(real.grid_path(name)))
            else:
                used_rough += 1
                base_view = spec["body"] if exists(f"body_{spec['body']}_idle0") else FALLBACK_BASE[spec["body"]]
                body = rough_frame(base_view, frame)
            if spec.get("mirror"):
                body = body.transpose(Image.FLIP_LEFT_RIGHT)
            img = Image.new("RGBA", (N, N), (0, 0, 0, 0))
            cat_img = None
            if spec.get("cat"):
                cat_frame = layout["cat_frames"][anim][index]
                cat_img = grid_to_image(load_grid(real.grid_path(f"cat_{spec['cat']}_{cat_frame}")))
                if spec.get("cat_mirror"):
                    cat_img = cat_img.transpose(Image.FLIP_LEFT_RIGHT)
                cx, cy = spec["cat_pos"]
                cy += layout["shoulder_bob"][anim][index]
            if cat_img is not None and spec.get("cat_layer") == "behind":
                img.alpha_composite(cat_img, (cx, cy))
            img.alpha_composite(body)
            if cat_img is not None and spec.get("cat_layer") != "behind":
                img.alpha_composite(cat_img, (cx, cy))
            sheet.alpha_composite(img, (col * N, row * N))
    os.makedirs(os.path.dirname(real.UNITY_OUT), exist_ok=True)
    sheet.save(real.UNITY_OUT)
    os.makedirs(os.path.dirname(real.PREVIEW_OUT), exist_ok=True)
    big = sheet.resize((sheet.width * 4, sheet.height * 4), Image.NEAREST)
    bg = Image.new("RGBA", big.size, (236, 232, 246, 255))
    bg.alpha_composite(big)
    bg.save(real.PREVIEW_OUT)
    print(f"wrote sheet {sheet.size}; {used_rough} of {len(frames) * 8} frames are stand-ins")


if __name__ == "__main__":
    build()
