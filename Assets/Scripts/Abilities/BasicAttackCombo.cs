using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackCombo : Ability
{
    //Raycast Variables
    private float abilityRange = 0.8f;

    //Combo Variables
    private Ability comboAttack = new BasicAttackFinisher();
    private float lastAttack = -1f;
    private float comboChainTime = 0.3f;


    //Initialise here
    public BasicAttackCombo()
    {
        //---- Setup ability stats ----
        //Setup looks
        icon = AbilitySprite.DASHSTRIKE;
        name = "Basic Attack Combo";
        animation = "attack";

        //Setup cooldown
        cooldownTime = 0f;

        //Setup force
        abilityVelocity = 5;

        //Setup stun
        stunTime = 0.1f;
    }

    public override void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        base.setTarget(caster, pos);
    }

    public override bool canComboAttack()
    {
        return lastAttack + comboChainTime > Time.time || comboAttack.canComboAttack();
    }

    protected override int getDamage(int casterStrength)
    {
        return casterStrength + 2;
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
            Debug.DrawRay(newPos, direction * abilityRange, Color.red, 3f);

            bool hitTarget = false;

            //If the Raycast hits an object on the layer Enemy
            foreach (RaycastHit2D r in hitObject)
            {

                if (r && r.transform.gameObject != caster.gameObject)
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

                    if (objectHit.tag == caster.tag)
                        continue;

                    //Apply damage and knockback
                    objectHit.setAttacker(caster);
                    objectHit.loseHealth(abilityDamage);

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
            caster.canCombo = false;
        }

        caster.canMove = true;
        caster.attacking = false;
    }

}
