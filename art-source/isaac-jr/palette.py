# The Zombie Dreams color palette: ONE shared set of 30 colors for all the pixel art.
# Each sprite in grids/ is a text picture: one character = one pixel.
# Change a color here, rebuild, and every sprite that uses it updates.
#
# To keep the art simple, several characters point at the SAME color
# (for example 'k' zombie eyes and 'o' outline are both the dark outline color).

# --- the 30 colors --------------------------------------------------------
OUTLINE = (32, 22, 38)          # dark purple-black: outlines, eyes
WHITE = (240, 244, 252)         # cuffs, collars, shine, cream, light-effect cores
STAR = (255, 226, 110)          # yellow: pajama stars, moon prints, sparkles, lamp light

SKIN_SHADOW, SKIN, SKIN_LIGHT = (210, 140, 98), (243, 179, 124), (252, 212, 170)
HAIR_SHADOW, HAIR, HAIR_LIGHT = (72, 41, 17), (105, 63, 29), (146, 94, 48)
BLUE_SHADOW, BLUE, BLUE_LIGHT = (74, 98, 170), (120, 152, 224), (172, 198, 246)   # Isaac Jr.'s pajamas
PINK_SHADOW, PINK, PINK_LIGHT = (168, 66, 98), (226, 112, 140), (248, 166, 186)   # slippers, mouth, cat nose
GINGER_SHADOW, GINGER, GINGER_LIGHT = (164, 64, 30), (222, 108, 48), (250, 168, 98)   # Scarlet's fur
MOLD_SHADOW, MOLD, MOLD_LIGHT = (88, 128, 92), (126, 170, 120), (172, 208, 150)       # zombie skin
GRAPE_SHADOW, GRAPE, GRAPE_LIGHT, GRAPE_PALE = (84, 64, 128), (130, 106, 184), (180, 160, 226), (214, 198, 246)   # zombie pajamas, dream smoke
GRAY, GRAY_LIGHT = (128, 120, 140), (170, 162, 182)     # zombie hair
TEAL_SHADOW, TEAL, TEAL_LIGHT = (30, 104, 150), (62, 176, 196), (140, 230, 232)       # XP dream shard

PALETTE = {
    ".": None,                 # transparent
    "o": OUTLINE,              # outline
    "k": OUTLINE,              # zombie eyes
    "w": WHITE,                # white
    "W": WHITE,                # cat cream
    "e": WHITE,                # zombie eye white
    "y": STAR,                 # yellow star / moon
    "l": STAR,                 # zombie pajama print

    "s": SKIN_SHADOW, "S": SKIN, "t": SKIN_LIGHT,
    "h": HAIR_SHADOW, "H": HAIR, "j": HAIR_LIGHT,
    "b": BLUE_SHADOW, "B": BLUE, "c": BLUE_LIGHT,
    "p": PINK_SHADOW, "P": PINK, "q": PINK_LIGHT,
    "m": PINK_SHADOW,          # mouth / blush
    "n": PINK_LIGHT,           # cat nose / inner ears

    "r": GINGER_SHADOW, "R": GINGER, "f": GINGER_LIGHT,
    "g": MOLD,                 # cat eyes (green)

    "z": MOLD_SHADOW, "Z": MOLD, "x": MOLD_LIGHT,
    "a": GRAPE_SHADOW, "A": GRAPE, "d": GRAPE_LIGHT, "D": GRAPE_PALE,
    "v": GRAY, "u": GRAY, "U": GRAY_LIGHT,

    "i": TEAL, "I": TEAL_LIGHT, "J": TEAL_SHADOW,
}

FRAME_SIZE = 48  # Isaac Jr.'s body frames are 48 x 48 pixels; feet touch the bottom row

if __name__ == "__main__":
    print(len({c for c in PALETTE.values() if c}), "distinct colors")
