using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackCombo : Ability
{

    private CMoveCombatable caster;

    string abilityName = "Basic Attack Combo";

    //Ability Variables
    private int abilityDamage; //Scale to player damage?
    private float abilityVelocity = 5;
    private string animation = "attack";
    private int abilityKnockback = 0;
    private int abilityKnockUp = 0;
    private float cooldownTime = 0f;

    //Raycast Variables
    private float abilityRange = 0.4f;

    //Combo Variables
    private Ability comboAttack = new BasicAttackFinisher();
    private float lastAttack = -1f;
    private float comboChainTime = 0.3f;

    //Directional Variables
    private Vector2 pos;
    private Vector2 direction;


    public void setTarget(CMoveCombatable caster, Vector2 pos, Vector2 direction)
    {
        this.caster = caster;
        this.pos = pos;
        this.direction = direction;

        abilityDamage = (int) (caster.attackDamage * 1.2f);
    }

    public Ability getComboAttack()
    {
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
        return; //No cooldown
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
        return abilityActionSequence(pos, direction);
    }

    public IEnumerator abilityActionSequence(Vector2 pos, Vector2 direction)
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
                    caster.canCombo = true;
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

            if(caster.pauseAfterAttack < 0.5f)
                yield return new WaitForSeconds(0.5f);
            else
                yield return new WaitForSeconds(caster.pauseAfterAttack);
        }
        caster.setComboAnimation(false);
        caster.canCombo = false;
        caster.canMove = true;
        caster.attacking = false;
    }

}
