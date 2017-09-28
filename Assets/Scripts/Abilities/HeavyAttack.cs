using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttack : Ability {

    private CMoveCombatable caster;

    string abilityName = "Heavy Attack";

    //Ability Variables
    private int abilityDamage; //Scale to player damage?
    private float abilityVelocity = 640;
    private string animation = "attack";
    private int abilityKnockback = 1000;
    private int abilityKnockUp = 300;
    private float cooldownTime = 0f;

    //Raycast Variables
    private float abilityRange = 0.65f;

    //Ability Icon
    public Sprite icon;

    //Directional Variables
    private Vector2 pos;
    private Vector2 direction;


    public void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        this.caster = caster;
        this.pos = pos;

        //Get direction based on caster facing direction
        direction = new Vector2(caster.transform.localScale.x, 0);

        abilityDamage = caster.attackDamage * 2;
    }

    /// <summary>
    /// Returns itself, as heavy attack cannot combo
    /// </summary>
    public Ability getComboAttack()
    {
        return this;
    }

    public bool canComboAttack()
    {
        return false;
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

    public Sprite getIcon()
    {
        return icon;
    }

    public IEnumerator getAction()
    {
        return abilityActionSequence();
    }

    public IEnumerator abilityActionSequence()
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
