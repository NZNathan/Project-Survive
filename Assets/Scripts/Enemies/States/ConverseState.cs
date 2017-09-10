using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConverseState : AIState
{

    int talking = 0;
    string[] conversationLines;
    FloatingText currentSpeech;
    float timeStarted = 0;
    Enemy talkingTarget;

    public ConverseState(Enemy character)
    {
        this.character = character;

        //Check if target still exists
        if(character.target == null)
        {
            character.popState();
            return;
        }

        talkingTarget = character.target.GetComponent<Enemy>();

        //Check if the target is still idle or if their stack is empty
        if (talkingTarget.peekState() == null || talkingTarget.peekState().GetType() == typeof(IdleState))
        {
            talkingTarget.pushState(new ListenState(talkingTarget, character));

            //Make characters face each other
            talkingTarget.facePoint(character.transform.position);
            character.facePoint(talkingTarget.transform.position);
        }
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

        //If target is no longer listening or no longer listening to you
        if (talkingTarget == null || talkingTarget.peekState().GetType() != typeof(ListenState) || ((ListenState)talkingTarget.peekState()).talker != character)
        {
            character.target = null;
            character.popState();
            character.pushState(new IdleState(character));

            if (currentSpeech != null)
                currentSpeech.turnoff();

            return;
        }

        conversation();
    }

    private void conversation()
    {
        if (talking == 0)
        {
            conversationLines = WorldManager.instance.banterGen.getConvo();
            currentSpeech = UIManager.instance.newTextMessage(character.gameObject, conversationLines[0]);
            timeStarted = Time.time;
            talking = 1;
        }
        else if (Time.time > timeStarted + currentSpeech.textDuration && talking < conversationLines.Length)
        {
            if(talking % 2 == 0)
                currentSpeech = UIManager.instance.newTextMessage(character.gameObject, conversationLines[talking]);
            else
                currentSpeech = UIManager.instance.newTextMessage(talkingTarget.gameObject, conversationLines[talking]);

            timeStarted = Time.time;
            talking++;
        }
        else if (Time.time > timeStarted + currentSpeech.textDuration && talking >= conversationLines.Length)
        {
            character.target = null;
            talkingTarget.popState();
            talkingTarget.pushState(new IdleState(talkingTarget));
            character.popState();
            character.pushState(new IdleState(character));
        }
    }


}
