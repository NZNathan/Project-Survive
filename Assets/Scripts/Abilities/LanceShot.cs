using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceShot : Ability
{

    public LanceShot()
    {
        //---- Setup ability stats ----
        
        //Setup looks
        icon = AbilitySprite.LANCE;
        name = "Lance Shot";
        animation = "attack";

        //Setup cooldown
        cooldownTime = 5f;

        //Setup force
        abilityVelocity = -20;

        //Stunned time applied to the target
        stunTime = 0.5f;
    }

    //Initialise here
    public override void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        base.setTarget(caster, pos);
    }

    protected override int getDamage(int casterStrength)
    {
        return casterStrength * 2;
    }

    protected override IEnumerator abilityActionSequence()
    {

        //Wait until the attack frame in the animation has been reached
        while (!caster.getAttackTrigger().hasAttackTriggered())
            yield return null;

        caster.getAttackTrigger().resetTrigger();

        //After shot nudge caster back a tad
        caster.rb2D.AddForce(direction * abilityVelocity / Time.timeScale);

        //Instantiate Bullet
        LanceBullet b = ObjectPool.instance.getLanceBullet();

        //Setup and turn on bullet
        b.gameObject.SetActive(true);
        b.Setup(caster, abilityDamage, stunTime, direction);

        caster.canCombo = false;
        caster.canMove = true;
        caster.attacking = false;
    }

}
