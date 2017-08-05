using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Ability
{

    private CMoveCombatable caster;

    //Ability Variables
    private int abilityDamage; //Scale to player damage?
    private float abilityVelocity = 220;
    private float cooldown = 0f;
    private string animation = "attack";
    private int abilityKnockback = 500;

    //Sound Variables
    private AudioClip abilityMissSound;
    private AudioClip abilityHitSound;

    //Raycast Variables
    private float abilityRange = 0.5f;
    private float timeBeforeRay = 0.25f;

    //Directional Variables
    private Vector2 pos;
    private Vector2 direction;


    public void setTarget(CMoveCombatable caster, Vector2 pos, Vector2 direction)
    {
        this.caster = caster;
        this.pos = pos;
        this.direction = direction;

        abilityDamage = caster.attackDamage;
    }

    public float getCooldown()
    {
        return cooldown;
    }

    public float getAbilityVelocity()
    {
        return abilityVelocity;
    }

    public string getAnimation()
    {
        return animation;
    }

    public void setIEnum(IEnumerator abilityAction)
    {
        abilityAction = abilityActionSequence(pos, direction);
        Debug.Log(abilityAction);
    }

    public IEnumerator getAction()
    {
        return abilityActionSequence(pos, direction);
    }

    public IEnumerator abilityActionSequence(Vector2 pos, Vector2 direction)
    {
        caster.rb2D.AddForce(direction * abilityVelocity);

        yield return new WaitForSeconds(timeBeforeRay);

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
                    //Hit attack
                    CHitable objectHit = r.transform.gameObject.GetComponentInParent<CHitable>();

                    if (objectHit.isInvuln())
                        continue;

                    //Apply damage and knockback
                    objectHit.setAttacker(caster);
                    objectHit.loseHealth(abilityDamage);
                    objectHit.knockback(pos, abilityKnockback, objectHit.objectHeight); //Need to use original pos for knockback so the position of where you attacked from is the knockback

                    caster.audioSource.clip = abilityHitSound;
                    //caster.audioSource.Play();

                    hitTarget = true;
                    break;
                }
            }

            if (!hitTarget)
            {
                caster.audioSource.clip = abilityMissSound;
                //caster.audioSource.Play();
            }

            yield return new WaitForSeconds(caster.pauseAfterAttack);
        }
        caster.canMove = true;
        caster.attacking = false;
    }

}
