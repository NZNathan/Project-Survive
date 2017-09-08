using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FightState : AIState {

    public FightState(Enemy character)
    {
        this.character = character;
    }

    public override void action()
    {
        return;
    }
}
