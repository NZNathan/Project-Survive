using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : AIState
{
    public DeadState(Enemy character)
    {
        this.character = character;
    }

    public override void action()
    {
        return;
    }
}
