using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConverseState : AIState
{

    int talking = 0;
    FloatingText currentSpeech;
    float timeStarted = 0;
    Enemy talkingTarget;

    public ConverseState(Enemy character)
    {
        this.character = character;
        talkingTarget = character.target.GetComponent<Enemy>();
        talkingTarget.facePoint(character.transform.position);

        //If the target is no longer idle
        if (talkingTarget.peekState().GetType() == typeof(IdleState))
            talkingTarget.pushState(new ListenState(talkingTarget, character));
    }


    public override void action()
    {
        if (character.isDead())
        {
            character.popState();
            character.pushState(new DeadState(character));

            if (currentSpeech != null)
                currentSpeech.turnoff();
        }

        if (character.isFalling())
        {
            character.popState();
            character.pushState(new KnockedDown(character));

            if (currentSpeech != null)
                currentSpeech.turnoff();
        }

        if (character.isStunned())
        {
            character.popState();
            character.pushState(new StunnedState(character));

            if (currentSpeech != null)
                currentSpeech.turnoff();
        }

        //If target is no longer listening
        if (talkingTarget == null || talkingTarget.peekState().GetType() != typeof(ListenState))
        {
            character.target = null;
            character.popState();
            character.pushState(new IdleState(character));

            if (currentSpeech != null)
                currentSpeech.turnoff();

            return;
        }

        if (talking == 0)
        {
            currentSpeech = UIManager.instance.newTextMessage(character.gameObject, WorldManager.instance.banterGen.getSmallTalk());
            timeStarted = Time.time;
            talking = 1;
        }
        else if (Time.time > timeStarted + currentSpeech.textDuration && talking == 1)
        {
            currentSpeech = UIManager.instance.newTextMessage(talkingTarget.gameObject, WorldManager.instance.banterGen.getSmallTalk());
            timeStarted = Time.time;
            talking = 2;
        }
        else if (Time.time > timeStarted + currentSpeech.textDuration && talking == 2)
        {
            character.target = null;
            talkingTarget.popState();
            talkingTarget.pushState(new IdleState(talkingTarget));
            character.popState();
            character.pushState(new IdleState(character));
        }
    }


}
