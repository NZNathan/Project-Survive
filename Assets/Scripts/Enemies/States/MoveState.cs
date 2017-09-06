using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : AIState
{
    //Hold a movement field to determine how to move?

    public MoveState(Enemy character)
    {
        this.character = character;
    }

    public override void action()
    {
        base.action();

        //Transition to Idle State if target gets too far away or they don't exist anymore
        if (character.target == null || (character.target.position - character.transform.position).magnitude > character.aggroRange)
        {
            character.target = null;
            character.animator.SetFloat("movementSpeed", 0f);
            character.popState();
            character.pushState(new IdleState(character));
            return;
        }

        

        //Transition to Attack State if target gets within range
        if ((character.target.position - character.transform.position).magnitude < character.attackRange)
        {
            character.animator.SetFloat("movementSpeed", 0f);
            character.pushState(new AttackState(character));
            return;
        }

        //Leave till last so don't move if switching states
        character.rb2D.AddForce(movement());
    }

    private Vector2 movement()
    {

        float movementSpeed = character.walkSpeed;

        Vector3 dir = character.getDirection(character.target.position, character.target.gameObject.GetComponent<CHitable>().objectHeight);

        if (dir.x < 0 && character.transform.localScale.x != -1)
            character.faceLeft();
        else if (dir.x > 0 && character.transform.localScale.x != 1)
            character.faceRight();

        character.animator.SetFloat("movementSpeed", 2.5f);
        
        return (dir * movementSpeed);

    }
}