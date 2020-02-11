using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SnakeColliderType {
    COLLECTIBLE,
    TAIL,
    MAGNET
}

public enum ScreenType {
    GAME,
    SHOP_MENU,
    MAIN_MENU
}

public enum ShopSection {
    HATS,
    COLORSCHEME,
    POWERUPS
}

public enum PlayerHatTypes {
    TYPE_DEFAULT,
    TYPE_CYLINDER,
    TYPE_CAPPY,
    TYPE_PROPELLER,
    TYPE_4,
    TYPE_5
}

public enum PlayerColorTypes {
    COLOR_DEFAULT,
    COLOR_2,
    COLOR_3,
    COLOR_4,
    COLOR_5,
    COLOR_6
}

public enum PlayerPowerupTypes {
    INVINCIBILTY,
    MAGNET,
    THIN
}
