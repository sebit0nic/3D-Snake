using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SnakeColliderType {
    COLLECTIBLE,
    TAIL,
    MAGNET
}

public enum ScreenType {
    MAIN_MENU,
    GAME,
    SHOP_MENU
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
    TYPE_VIKING,
    TYPE_CROWN
}

public enum PlayerColorTypes {
    COLOR_CLASSIC,
    COLOR_BLACKWHITE,
    COLOR_PLAYFUL,
    COLOR_ARCTIC,
    COLOR_AUTUMN,
    COLOR_DARK
}

public enum PlayerPowerupTypes {
    INVINCIBILTY,
    MAGNET,
    THIN
}

public enum PurchaseableColorType {
    BASE,
    PLANET,
    PARTICLE
}

public enum PurchaseableMaterialType {
    BASE,
    SKYBOX
}
