using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackState : AIState {

    public AttackState(Enemy character)
    {
        this.character = character;

        //Look at the target
        if (character.target.transform.position.x < character.transform.position.x && character.transform.localScale.x != -1)
            character.faceLeft();
        else if (character.target.transform.position.x > character.transform.position.x && character.transform.localScale.x != 1)
            character.faceRight();

        //Attack the target
        character.attackTarget();
    }


    public override void action()
    {
        if (!character.attacking)
            character.popState();
    }
}
