using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AIState
{
    //Hold a movement field to determine how to move?
    protected Enemy character;

    //public abstract void exitState();

    public virtual void action()
    {
        if (character.isDead())
        {
            character.popState();
            character.pushState(new DeadState(character));
            return;
        }

        if (character.isFalling())
        {
            character.pushState(new KnockedDown(character));
            return;
        }

        if (character.isStunned())
        {
            //When the character is hit, make the attacker the new target and push a move state, so after they become unstunned they'll start moving to new target, instead of grabbing the closest target
            character.target = character.getAttacker().transform;
            if(character.target != null)
                character.pushState(new MoveState(character));

            character.pushState(new StunnedState(character));
            return;
        }
    }

    /// <summary>
    /// Gets all characters on the hitable layer within aggro range and finds the closest enemy (if any) and sets them as the target
    /// </summary>
    /// <returns></returns>
    protected bool enemyNearby()
    {
        Collider2D[] hitColliders = new Collider2D[10];

        int collidersLength = Physics2D.OverlapCircleNonAlloc(character.transform.position, character.aggroRange, hitColliders, CMoveCombatable.attackMask);
        int i = 0;

        Transform closestEnemy = null;

        while (i < collidersLength)
        {
            //Try and get the component from the collider to check if its actually a character
            CMoveCombatable target = hitColliders[i].GetComponent<CMoveCombatable>();

            //if the target is actually an Enemy and is hostile to the character
            if (target != null && FactionManager.instance.isHostile(character.faction, target.faction))
            {

                //If there is no closer enemy. assign the new target as the closest
                if (closestEnemy == null)
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
}
