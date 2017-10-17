using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Ability
{
    //How far the ray will be cast
    private float abilityRange = 0.8f;

    //Combo Variables
    private Ability comboAttack = new BasicAttackCombo();
    private float lastAttack = -1f;
    private float comboChainTime = 0.35f;

    //Initialise here
    public BasicAttack()
    {
        //---- Setup ability stats ----
        //Setup looks
        icon = AbilitySprite.DASHSTRIKE;
        name = "Basic Strike";
        animation = "attack";
        causeOfDeath = "Cut down";

        //Setup cooldown
        cooldownTime = 0f;

        //Setup force
        abilityVelocity = 45;

        //Setup stun
        stunTime = 0.1f;
    }

    public override void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        base.setTarget(caster, pos);
    }

    protected override int getDamage(int casterStrength)
    {
        return casterStrength;
    }

    public override bool canComboAttack()
    {
        return lastAttack + comboChainTime > Time.time || comboAttack.canComboAttack();
    }

    public override string getAnimation()
    {
        return animation;
    }

    protected override IEnumerator abilityActionSequence()
    {

        //Wait until the attack frame in the animation has been reached
        while (!caster.getAttackTrigger().hasAttackTriggered())
            yield return null;

        caster.rb2D.AddForce(direction * abilityVelocity / Time.timeScale);
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
                    CMoveCombatable targetHit = r.transform.gameObject.GetComponentInParent<CMoveCombatable>();

                    if (objectHit.isInvuln() || targetHit.faction == caster.faction || objectHit.isKnockedback()) //Add faction to hitables to use here instead of tags
                        continue;

                    //Set attacker and info on hit 
                    objectHit.setAttacker(caster);
                    objectHit.lastAttackInfo = "A basic hit"; 
                    //objectHit.knockback(pos, abilityKnockback, objectHit.objectHeight); //Need to use original pos for knockback so the position of where you attacked from is the knockback
                    objectHit.loseHealth(abilityDamage);

                    //Apply stun to the target
                    //objectHit.applyStun(stunTime);

                    caster.audioSource.clip = caster.attackSound;
                    caster.audioSource.Play();
                    caster.attackHit();

                   

                    if(targetHit != null)
                    {
                        targetHit.causeOfDeath = causeOfDeath;
                    }

                    lastAttack = Time.time;
                    //caster.canCombo = true;
                    caster.setComboAnimation(true);

                    hitTarget = true;
                    break;
                }
            }

            if (!hitTarget)
            {
                caster.audioSource.clip = caster.missSound;
                caster.audioSource.Play();
            }

            //Wait till attack animation is over
            while (!caster.getAttackTrigger().isAttackOver())
                yield return null;

            caster.getAttackTrigger().resetAttack();
    
        }

        if (caster.canCombo)
        {
            caster.attack(comboAttack);
            yield break;
        }
        else
        {
            //Pause for caster
            yield return new WaitForSeconds(caster.pauseAfterAttack);
            caster.setComboAnimation(false);
            
        }
        caster.canCombo = false;
        caster.canMove = true;
        caster.attacking = false;
    }

}
