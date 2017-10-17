using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetShot : Ability
{ 
    //Combo Variables
    private float comboChainTime = 0.35f;

    public RicochetShot()
    {
        icon = AbilitySprite.RICOCHET;
        animation = "attack";
        name = "Ricochet Shot";

        cooldownTime = 5f;
        stunTime = 0.1f;
        abilityKnockback = 0;
        abilityVelocity = -10;
    }

    //Initialise here
    public override void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        base.setTarget(caster, pos);
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
        RicochetBullet b = ObjectPool.instance.getRicochetBullet();

        //Setup and turn on bullet
        b.gameObject.SetActive(true);
        b.Setup(caster, abilityDamage, stunTime, direction);

        caster.canCombo = false;
        caster.canMove = true;
        caster.attacking = false;
    }

}
