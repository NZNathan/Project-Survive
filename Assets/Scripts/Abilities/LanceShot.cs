using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceShot : Ability
{

    //Raycast Variables
    private float abilityRange = 9.1f;

    public LanceShot()
    {
        //---- Setup ability stats ----

        //Setup looks
        icon = AbilitySprite.LANCE;
        name = "Lance Shot";
        animation = "attack";
        causeOfDeath = "Took a bullet to the knee";

        //Setup cooldown
        cooldownTime = 5f;

        //Setup force
        abilityVelocity = -20;

        //Stunned time applied to the target
        stunTime = 0.5f;

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
        return casterStrength * 2;
    }

    protected override IEnumerator abilityActionSequence()
    {
        cooldownStartTime = Time.time;

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
        b.Setup(caster, direction);

        //Play sound
        caster.audioSource.clip = abilitySound;
        caster.audioSource.Play();

        RaycastHit2D[] hitObject = Physics2D.RaycastAll(pos, direction, abilityRange, CMoveCombatable.attackMask, -10, 10);
        Debug.DrawRay(pos, direction * abilityRange, Color.blue, 3f);

        //If the Raycast hits an object on the layer Enemy
        foreach (RaycastHit2D r in hitObject)
        {
            if (r && r.transform.gameObject != caster.gameObject && caster.attacking)
            {
                //If an object has been hit first
                if (r.transform.gameObject.tag == "Object")
                    continue;

                //Hit attack
                CHitable objectHit = r.transform.gameObject.GetComponentInParent<CHitable>();

                if (objectHit.isInvuln())
                    continue;

                //Apply damage and knockback
                objectHit.setAttacker(caster);
                objectHit.loseHealth(abilityDamage);
                objectHit.knockback(pos, abilityKnockback, objectHit.objectHeight); //Need to use original pos for knockback so the position of where you attacked from is the knockback
                objectHit.knockUp(pos, abilityKnockback, abilityKnockUp, objectHit.objectHeight);

                CMoveCombatable targetHit = r.transform.gameObject.GetComponentInParent<CMoveCombatable>();

                if (targetHit != null)
                {
                    targetHit.causeOfDeath = causeOfDeath;
                }

                //caster.audioSource.clip = abilitySound;
                //caster.audioSource.Play();
            }
        }


        caster.canCombo = false;
        caster.canMove = true;
        caster.attacking = false;
    }

}
