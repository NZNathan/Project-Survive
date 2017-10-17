using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttack : Ability {

    //Raycast Variables
    private float abilityRange = 0.65f;

    public HeavyAttack()
    {
        //---- Setup ability stats ----
        //Setup looks
        icon = AbilitySprite.DASHSTRIKE;
        name = "Heavy Strike";
        animation = "attack";

        //Setup cooldown
        cooldownTime = 0f;

        //Setup force
        abilityKnockback = 1000;
        abilityKnockUp = 300;
        abilityVelocity = 640;

    }

    //Initialise here
    public override void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        base.setTarget(caster, pos);
    }

    public override string getAnimation()
    {
        return animation;
    }

    protected override IEnumerator abilityActionSequence()
    {
        caster.rb2D.AddForce(direction * abilityVelocity / Time.timeScale);

        while (!caster.getAttackTrigger().hasAttackTriggered())
            yield return null;

        caster.getAttackTrigger().resetTrigger();

        //Check if attack can go through
        if (!caster.isDead())
        {

            Vector2 newPos = new Vector2(caster.transform.position.x, caster.transform.position.y + caster.objectHeight / 2);

            RaycastHit2D[] hitObject = Physics2D.RaycastAll(newPos, direction, abilityRange, CMoveCombatable.attackMask, -10, 10);
            Debug.DrawRay(newPos, direction * abilityRange, Color.black, 3f);

            bool hitTarget = false;

            //If the Raycast hits an object on the layer Enemy
            foreach (RaycastHit2D r in hitObject)
            {

                if (r && r.transform.gameObject != caster.gameObject && caster.attacking)
                {
                    //If an object has been hit first
                    if (r.transform.gameObject.tag == "Object")
                    {
                        if (r.collider.isTrigger)
                            continue;
                        else
                            break;
                    }

                    //Hit attack
                    CHitable objectHit = r.transform.gameObject.GetComponentInParent<CHitable>();

                    if (objectHit.isInvuln() || objectHit.tag == caster.tag || objectHit.isKnockedback())
                        continue;

                    //Apply damage and knockback
                    objectHit.setAttacker(caster);
                    objectHit.knockback(pos, abilityKnockback, objectHit.objectHeight); //Need to use original pos for knockback so the position of where you attacked from is the knockback
                    objectHit.knockUp(pos, abilityKnockback, abilityKnockUp, objectHit.objectHeight);
                    objectHit.loseHealth(abilityDamage);

                    caster.audioSource.clip = caster.attackSound;
                    caster.audioSource.Play();
                    caster.attackHit();

                    hitTarget = true;
                }
            }

            if (!hitTarget)
            {
                caster.audioSource.clip = caster.missSound;
                caster.audioSource.Play();
            }

            yield return new WaitForSeconds(caster.pauseAfterAttack);
        }
        caster.canMove = true;
        caster.attacking = false;
    }

}
