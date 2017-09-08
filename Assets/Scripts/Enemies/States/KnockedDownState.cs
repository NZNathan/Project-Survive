using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KnockedDown : AIState
{
    public KnockedDown(Enemy character)
    {
        this.character = character;
    }

    public override void action()
    {
        if (!character.isFalling())
        {
            character.popState();
        }
    }
}
