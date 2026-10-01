# Renders text-grid sprites to a big PNG so you can look at them.
# Usage:  python preview.py out.png grids/body_down_idle0.txt [more grids...] [--scale 8] [--grid]
#   --grid  draws faint pixel lines and a coordinate ruler every 8 pixels (handy for editing)
import sys
from PIL import Image, ImageDraw
from palette import PALETTE


def load_grid(path):
    with open(path) as f:
        rows = [line.rstrip("\n") for line in f if line.strip("\n") != ""]
    return rows


def grid_to_image(rows):
    h = len(rows)
    w = max(len(r) for r in rows)
    img = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    for y, row in enumerate(rows):
        for x, ch in enumerate(row):
            if ch not in PALETTE:
                img.putpixel((x, y), (255, 0, 255, 255))  # unknown char shows as hot pink
            elif PALETTE[ch] is not None:
                img.putpixel((x, y), PALETTE[ch] + (255,))
    return img


def render(images, scale=8, grid=False, labels=None):
    pad = 2
    w = sum(i.width for i in images) + pad * (len(images) + 1)
    h = max(i.height for i in images) + pad * 2
    top = 14 if labels else 0
    sheet = Image.new("RGBA", (w * scale, h * scale + top), (255, 255, 255, 255))
    d = ImageDraw.Draw(sheet)
    x = pad
    for n, im in enumerate(images):
        # checkerboard behind each sprite so transparency is visible
        for yy in range(im.height):
            for xx in range(im.width):
                c = (232, 232, 240) if (xx + yy) % 2 == 0 else (248, 248, 252)
                X, Y = (x + xx) * scale, (pad + yy) * scale + top
                d.rectangle([X, Y, X + scale - 1, Y + scale - 1], fill=c)
        big = im.resize((im.width * scale, im.height * scale), Image.NEAREST)
        sheet.alpha_composite(big, (x * scale, pad * scale + top))
        if grid:
            for xx in range(im.width + 1):
                col = (255, 80, 80, 255) if xx % 8 == 0 else (200, 200, 210, 255)
                d.line([((x + xx) * scale, pad * scale + top), ((x + xx) * scale, (pad + im.height) * scale + top)], fill=col)
            for yy in range(im.height + 1):
                col = (255, 80, 80, 255) if yy % 8 == 0 else (200, 200, 210, 255)
                d.line([(x * scale, (pad + yy) * scale + top), ((x + im.width) * scale, (pad + yy) * scale + top)], fill=col)
        if labels:
            d.text((x * scale, 1), labels[n], fill=(0, 0, 0, 255))
        x += im.width + pad
    return sheet


if __name__ == "__main__":
    args = sys.argv[1:]
    scale = 8
    if "--scale" in args:
        i = args.index("--scale"); scale = int(args[i + 1]); del args[i:i + 2]
    grid = "--grid" in args
    args = [a for a in args if a != "--grid"]
    out, paths = args[0], args[1:]
    imgs = [grid_to_image(load_grid(p)) for p in paths]
    names = [p.replace("\\", "/").split("/")[-1].replace(".txt", "") for p in paths]
    render(imgs, scale, grid, names).save(out)
    print("wrote", out)
