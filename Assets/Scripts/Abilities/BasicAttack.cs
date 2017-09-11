using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Ability
{

    private CMoveCombatable caster;

    string abilityName = "Attack";

    private int abilityDamage; //Scale to player damage?

    //The force to be applied to the caster in the attack direction
    private float abilityVelocity = 0;

    //Animation name in animator
    private string animation = "attack";

    //Knockback applied to target that is hit by attack
    private int abilityKnockback = 0;
    //Stunned time applied to the target
    private float stunTime = 0.1f;

    //Cooldown of the ability
    private float cooldownTime = 0f;

    //How far the ray will be cast
    private float abilityRange = 0.4f;

    //Combo Variables
    private Ability comboAttack = new BasicAttackCombo();
    private float lastAttack = -1f;
    private float comboChainTime = 0.35f;

    //Directional Variables
    private Vector2 pos;
    private Vector2 direction;

    //Initialise here
    public void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        this.caster = caster;
        this.pos = pos;

        //Get direction based on caster facing direction
        direction = new Vector2(caster.transform.localScale.x, 0);

        caster.canCombo = false;
        abilityDamage = caster.attackDamage;
    }

    public Ability getComboAttack()
    {
        return this;
        if (canComboAttack())
        {
            lastAttack = 0;
            return comboAttack.getComboAttack();
        }
        return this;
    }

    public bool canComboAttack()
    {
        return lastAttack + comboChainTime > Time.time || comboAttack.canComboAttack();
    }

    public void setCooldown(bool cooldown)
    {
        return;
    }

    public bool onCooldown()
    {
        return false;
    }

    public float getCooldown()
    {
        return cooldownTime;
    }

    public float getAbilityVelocity()
    {
        return abilityVelocity;
    }

    public string getAnimation()
    {
        return animation;
    }

    public string getName()
    {
        return abilityName;
    }

    public IEnumerator getAction()
    {
        return abilityActionSequence();
    }

    public IEnumerator abilityActionSequence()
    {

        //Wait until the attack frame in the animation has been reached
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

                    if (objectHit.isInvuln() || objectHit.tag == caster.tag || objectHit.isKnockedback()) //Add faction to hitables to use here instead of tags
                        continue;

                    //Apply damage and knockback
                    objectHit.setAttacker(caster);
                    //objectHit.knockback(pos, abilityKnockback, objectHit.objectHeight); //Need to use original pos for knockback so the position of where you attacked from is the knockback
                    objectHit.loseHealth(abilityDamage);

                    //Apply stun to the target
                    objectHit.applyStun(stunTime);

                    caster.audioSource.clip = caster.attackSound;
                    caster.audioSource.Play();
                    caster.attackHit();

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
