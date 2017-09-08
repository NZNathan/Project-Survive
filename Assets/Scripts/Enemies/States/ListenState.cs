using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListenState : AIState {

    public Enemy talker { get; private set; }

    public ListenState(Enemy character, Enemy talker)
    {
        this.character = character;
        this.talker = talker;
    }


    public override void action()
    {
        if (character.isDead())
        {
            character.popState();
            character.pushState(new DeadState(character));
        }

        if (character.isFalling())
        {
            character.popState();
            character.pushState(new KnockedDown(character));
        }

        if (character.isStunned())
        {
            character.popState();
            character.pushState(new StunnedState(character));
        }

        //If target is no longer talking or is destroyed
        if (talker == null || talker.peekState().GetType() != typeof(ConverseState))
        {
            character.target = null;
            character.popState();
            character.pushState(new IdleState(character));
        }
    }

}
