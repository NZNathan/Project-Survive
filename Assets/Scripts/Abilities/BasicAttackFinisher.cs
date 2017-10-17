using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackFinisher : Ability
{
    
    //Raycast Variables
    private float abilityRange = 0.8f;

    //Initialise here
    public BasicAttackFinisher()
    {
        //---- Setup ability stats ----
        //Setup looks
        icon = AbilitySprite.DASHSTRIKE;
        name = "Basic Attack Finisher";
        animation = "attack";

        //Setup cooldown
        cooldownTime = 0f;

        //Setup force
        abilityVelocity = 5;
        abilityKnockback = 500;
        abilityKnockUp = 500;

        //Setup stun
        stunTime = 0.1f;
    }

    public override void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        base.setTarget(caster, pos);
    }

    protected override int getDamage(int casterStrength)
    {
        return casterStrength + 4;
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
                    objectHit.knockback(pos, abilityKnockback, objectHit.objectHeight); //Need to use original pos for knockback so the position of where you attacked from is the knockback
                    objectHit.knockUp(pos, abilityKnockback, abilityKnockUp, objectHit.objectHeight);
                    objectHit.loseHealth(abilityDamage);

                    caster.audioSource.clip = caster.attackSound;
                    caster.audioSource.Play();
                    caster.attackHit();

                    hitTarget = true;
                    break;
                }
            }

            if (!hitTarget)
            {
                caster.audioSource.clip = caster.missSound;
                caster.audioSource.Play();
            }

            if (caster.pauseAfterAttack < 0.5f)
                yield return new WaitForSeconds(0.5f);
            else
                yield return new WaitForSeconds(caster.pauseAfterAttack);
        }
        caster.canMove = true;
        caster.attacking = false;
        caster.canCombo = false;
    }

}
