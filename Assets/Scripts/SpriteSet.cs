using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSet {

    public enum Part {HEAD, BODY, R_SHOULDER, R_ARM, L_SHOULDER, L_ARM, PELVIS, R_THIGH, R_LEG, L_THIGH, L_LEG, HEADBACKWARDS, HAT, BEARD};

    //Sprite Parts
    Sprite[] spriteSet;

    public SpriteSet(Sprite[] sprites)
    {
        spriteSet = sprites;

        Debug.Log(sprites[(int)Part.L_LEG]);
    }

    public Sprite[] getSprites()
    {
        return spriteSet;
    }

}
