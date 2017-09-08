using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AIState
{
    //Hold a movement field to determine how to move?
    protected Enemy character;

    //public abstract void exitState();

    public virtual void action()
    {
        if (character.isDead())
        {
            character.popState();
            character.pushState(new DeadState(character));
        }

        if (character.isFalling())
        {
            character.pushState(new KnockedDown(character));
        }

        if (character.isStunned())
        {
            character.pushState(new StunnedState(character));
        }
    }
}
