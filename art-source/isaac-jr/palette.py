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
}

FRAME_SIZE = 48  # every body frame is 48 x 48 pixels; feet touch the bottom row
