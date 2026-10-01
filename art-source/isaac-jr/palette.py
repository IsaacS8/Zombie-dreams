# The colors Isaac Jr. and Scarlet are allowed to use.
# Each sprite in grids/ is a text picture: one character = one pixel.
# Change a color here, rebuild, and every frame updates.

PALETTE = {
    ".": None,                 # transparent
    "o": (32, 22, 38),         # outline (soft purple-black)

    # Hair (brown, spiky)
    "h": (72, 41, 17),         # hair shadow
    "H": (105, 63, 29),        # hair
    "j": (146, 94, 48),        # hair highlight

    # Skin
    "s": (210, 140, 98),       # skin shadow
    "S": (243, 179, 124),      # skin
    "t": (252, 212, 170),      # skin highlight
    "m": (196, 92, 104),       # mouth / blush

    # Pajamas (light blue)
    "b": (74, 98, 170),        # pajama shadow
    "B": (120, 152, 224),      # pajama
    "c": (172, 198, 246),      # pajama highlight
    "y": (255, 226, 110),      # yellow star print
    "w": (240, 244, 252),      # white (cuffs, collar, eye shine)

    # Slippers (pink)
    "p": (168, 66, 98),        # slipper shadow
    "P": (226, 112, 140),      # slipper
    "q": (248, 166, 186),      # slipper highlight

    # Scarlet the cat (ginger)
    "r": (164, 64, 30),        # cat shadow / stripes
    "R": (222, 108, 48),       # cat fur
    "f": (250, 168, 98),       # cat fur highlight
    "W": (252, 240, 226),      # cat cream (muzzle, chest, paws)
    "g": (92, 178, 96),        # cat eyes (green)
    "n": (236, 124, 146),      # cat nose / inner ears

    # Zombies (sleepwalkers): sickly green skin, purple pajamas
    "z": (88, 128, 92),        # zombie skin shadow
    "Z": (126, 170, 120),      # zombie skin
    "x": (172, 208, 150),      # zombie skin highlight
    "k": (52, 36, 66),         # zombie eyes (half-closed, sleepy)
    "e": (236, 232, 190),      # zombie eye white / bags highlight
    "a": (84, 64, 128),        # zombie pajama shadow
    "A": (130, 106, 184),      # zombie pajama
    "d": (180, 160, 226),      # zombie pajama highlight
    "l": (255, 236, 150),      # zombie pajama print (moons/stars, pale yellow)
    "u": (128, 120, 140),      # zombie hair (dull gray-purple, bed head)
    "U": (170, 162, 182),      # zombie hair highlight
    "v": (96, 88, 108),        # zombie hair shadow

    # Items and effects: dream-shard (XP gem) teal, dream-smoke purple
    "i": (62, 176, 196),       # gem mid
    "I": (140, 230, 232),      # gem highlight
    "J": (30, 104, 150),       # gem shadow
    "D": (214, 198, 246),      # dream smoke, lightest
}

FRAME_SIZE = 48  # every body frame is 48 x 48 pixels; feet touch the bottom row
