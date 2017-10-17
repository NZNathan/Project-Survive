using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShoot : Ability
{

    public BasicShoot()
    {
        //---- Setup ability stats ----
        
        //Setup looks
        icon = AbilitySprite.DODGEROLL;
        name = "Basic Shot";
        animation = "attack";
        causeOfDeath = "Gunned down";

        //Setup cooldown
        cooldownTime = 0f;

        //Setup force
        abilityVelocity = -20;

        //Setup sound
        abilitySound = MusicManager.instance.gunShot;

    }

    //Initialise here
    public override void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        base.setTarget(caster, pos);
    }

    protected override int getDamage(int casterStrength)
    {
        return casterStrength;
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
        Bullet b = ObjectPool.instance.getBullet();

        //Setup and turn on bullet
        b.gameObject.SetActive(true);
        b.Setup(caster, abilityDamage, stunTime, direction);

        //Play sound
        caster.audioSource.clip = abilitySound;
        caster.audioSource.Play();

        caster.canCombo = false;
        caster.canMove = true;
        caster.attacking = false;
    }

}
