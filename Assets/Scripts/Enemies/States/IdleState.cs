﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AIState
{

    private int tick = 0; //tick is one frame
    private int tickUntillCall = 5;
    private int tickUntillConvoCall = 150;

    public IdleState(Enemy character)
    {
        this.character = character;
    }

    public override void action()
    {
        base.action();

        //Only run aggro check every 5 ticks to save computation
        if (tick % tickUntillCall == 0)
        {
            //Transition to Move State If player is within range
            if (enemyNearby())
            {
                character.popState();
                character.pushState(new MoveState(character));
            }
        }
        
        //Only run convo call check every 50 ticks to save computation
        if (tick >= tickUntillConvoCall)
        {
            //Transition to Move State If player is within range
            if (friendlyNearby())
            {
                character.popState();
                character.pushState(new MoveState(character));
            }
            tick = -1;
        }
        
        tick++;
    }

    /// <summary>
    /// Gets all characters on the hitable layer within aggro range and finds the closest enemy (if any) and sets them as the target
    /// </summary>
    /// <returns></returns>
    bool enemyNearby()
    {
        Collider2D[] hitColliders = new Collider2D[10];

        int collidersLength = Physics2D.OverlapCircleNonAlloc(character.transform.position, character.aggroRange, hitColliders, CMoveCombatable.attackMask);
        int i = 0;

        Transform closestEnemy = null;

        while (i < collidersLength)
        {
            //Try and get the component from the collider to check if its actually a character
            CMoveCombatable target = hitColliders[i].GetComponent<CMoveCombatable>();

            //if the target is actually a CMoveCombatable and is hostile to the character
            if (target != null && FactionManager.instance.isHostile(character.faction, target.faction))
            {
                
                //If there is no closer enemy. assign the new target as the closest
                if(closestEnemy == null)
                    closestEnemy = target.transform;
                //If the new target is closer than the previous closeset enemy
                else if ((target.transform.position - character.transform.position).magnitude < (closestEnemy.position - character.transform.position).magnitude)
                    closestEnemy = target.transform;
            }

            i++;
        }
        if (closestEnemy == null)
            return false;

        character.target = closestEnemy;
        return true;
    }

    /// <summary>
    /// Gets all characters on the hitable layer within half aggro range and finds the closest friendly (if any) and sets them as the target
    /// </summary>
    /// <returns></returns>
    bool friendlyNearby()
    {
        Collider2D[] hitColliders = new Collider2D[10];

        int collidersLength = Physics2D.OverlapCircleNonAlloc(character.transform.position, character.aggroRange/2, hitColliders, CMoveCombatable.attackMask);
        int i = 0;

        Transform closestFriend = null;

        while (i < collidersLength)
        {
            //Try and get the component from the collider to check if its actually a character
            CMoveCombatable target = hitColliders[i].GetComponent<CMoveCombatable>();

            //if the target is actually a CMoveCombatable and is hostile to the character
            if (target != null && !FactionManager.instance.isHostile(character.faction, target.faction) && target.gameObject != character.gameObject)
            {

                //If there is no closer enemy. assign the new target as the closest
                if (closestFriend == null)
                    closestFriend = target.transform;
                //If the new target is closer than the previous closeset enemy
                else if ((target.transform.position - character.transform.position).magnitude < (closestFriend.position - character.transform.position).magnitude)
                    closestFriend = target.transform;
            }

            i++;
        }
        if (closestFriend == null)
            return false;

        character.target = closestFriend;
        return true;
    }
}
