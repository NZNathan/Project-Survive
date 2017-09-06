using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverseState : AIState
{

    bool talking = false;
    float timeStarted = 0;
    float talkTime = 2.3f;

    public ConverseState(Enemy character)
    {
        this.character = character;
    }

    public override void action()
    {
        if (!talking)
        {
            UIManager.instance.newTextMessage(character.gameObject, WorldManager.instance.banterGen.getSmallTalk());
            timeStarted = Time.time;
            talking = true;
        }
        else if (Time.time > timeStarted + talkTime)
        {
            character.target = null;
            character.popState();
            character.pushState(new IdleState(character));
        }
    }


}
