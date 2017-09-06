using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AIState {

    public AttackState(Enemy character)
    {
        this.character = character;
        character.attackTarget();
    }

    public override void action()
    {
        if (!character.attacking)
            character.popState();
    }
}
