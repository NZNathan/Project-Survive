using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : Ability
{

    private float abilityDuration = 0.7f; //Scale to player damage?

    //The force to be applied to the caster in the attack direction
    private float stepAmount = 0.1f;


    //Sound Variables
    private AudioClip parrySound;

    public Parry()
    {
        icon = AbilitySprite.PARRY;
        animation = "parry";
        name = "Parry";

        cooldownTime = 5f;

        abilityKnockback = 200;
        abilityVelocity = 150;

         stunTime = 2f;
    }

    public override void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        this.caster = caster;
        this.pos = pos;

        //Direction is either walking direction of caster or facing direction if not moving
        if(caster.rb2D.velocity == Vector2.zero)
            direction = Vector2.right * caster.transform.localScale.x;
        else
            direction = caster.rb2D.velocity.normalized;
    }

    public void parriedEnemy(CMoveCombatable enemy)
    {
        //Apply stun to the target
        enemy.applyStun(stunTime);

        enemy.knockback(pos, abilityKnockback, enemy.objectHeight);
    }

    protected override IEnumerator abilityActionSequence()
    {
        //Start Cooldown
        cooldownStartTime = Time.time;

        //Set up immunities
        bool stunImmunity = caster.stunImmunity;
        bool knockbackImmunity = caster.knockbackImmunity;
        caster.stunImmunity = true;
        caster.knockbackImmunity = true;

        caster.parrying = true;

		yield return new WaitForSeconds(1f);

        caster.parrying = false;
        caster.stunImmunity = stunImmunity;
        caster.knockbackImmunity = knockbackImmunity;
        caster.canMove = true;
        caster.attacking = false;
    }

}
