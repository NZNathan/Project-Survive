using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRoll : Ability
{
    //How long the roll is for
    private float abilityDuration = 0.7f;

    //The time between adding force to the dodgeroll
    private float stepAmount = 0.1f;

    public DodgeRoll()
    {
        //---- Setup ability stats ----
        
        //Setup looks
        icon = AbilitySprite.DODGEROLL;
        name = "Dodge Roll";
        animation = "dodgeroll";

        //Setup cooldown
        cooldownTime = 5f;

        //Setup force
        abilityVelocity = 150;
    }

    public override void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        base.setTarget(caster, pos);

        //Direction is either walking direction of caster or facing direction if not moving
        if(caster.rb2D.velocity == Vector2.zero)
            direction = Vector2.right * caster.transform.localScale.x;
        else
            direction = caster.rb2D.velocity.normalized;

    }

    protected override IEnumerator abilityActionSequence()
    {
        //Start Cooldown
        cooldownStartTime = Time.time;

        caster.rb2D.AddForce(direction * abilityVelocity);
        caster.setInvulnerable(abilityDuration);
        caster.startCollisionsOff(abilityDuration);

        if (caster.rb2D.velocity.x < 0)
            caster.faceLeft();
        else
            caster.faceRight();

        float duration = 0;

        while (duration < abilityDuration)
        {
            caster.rb2D.AddForce(direction * abilityVelocity);

            duration += stepAmount;
            yield return new WaitForSeconds(stepAmount);
        }

        caster.rb2D.velocity /= 2;
        caster.canMove = true;
        caster.attacking = false;
    }

}
