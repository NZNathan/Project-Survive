using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListenState : AIState {

    public Enemy talker { get; private set; }
    private int tick = 0; //tick is one frame
    private int tickUntillCall = 5;


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
            return;
        }

        //Only run aggro check every 5 ticks to save computation
        if (tick >= tickUntillCall)
        {
            //Transition to Move State If hostile target is within range
            if (enemyNearby())
            {
                character.popState();
                character.pushState(new MoveState(character));
            }

            tick = 0;
        }

        tick++;
    }

}
