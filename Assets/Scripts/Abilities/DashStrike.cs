using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStrike : Ability {

    private CMoveCombatable caster;

    string abilityName = "Dash Strike";

    //Ability Variables
    private int abilityDamage; //Scale to player damage?
    private float abilityVelocity = 1600;
    private string animation = "dash";
    private int abilityKnockback = 50;
    private float cooldownTime = 5f;
    private bool cooldown = false;

    //Sound Variables
    private AudioClip abilitySound;

    //Ability Icon
    public AbilitySprite icon = AbilitySprite.DASHSTRIKE;

    //Raycast Variables
    private float abilityRange = 3.1f;
    private float timeBeforeRay = 0.35f;

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

    public void setCooldown(bool cooldown)
    {
        this.cooldown = cooldown;
    }

    public bool canComboAttack()
    {
        return false;
    }

    public bool onCooldown()
    {
        return cooldown;
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
        return AbilityIconList.instance.getAbilitSprite(icon);
    }

    public IEnumerator getAction()
    {
        return abilityActionSequence();
    }

    IEnumerator abilityActionSequence()
    {
        string oldLayer = LayerMask.LayerToName(caster.gameObject.layer);
        caster.gameObject.layer = C.noCollisionLayer;

        caster.rb2D.AddForce(direction * abilityVelocity / Time.timeScale);

        yield return new WaitForSeconds(timeBeforeRay);

        //Check if attack can go through
        if (!caster.isDead())
        {
            //Actual distance travelled by caster
            float distanceCovered = Mathf.Abs(caster.transform.position.x - pos.x);

            RaycastHit2D[] hitObject = Physics2D.RaycastAll(pos, direction, distanceCovered, CMoveCombatable.attackMask, -10, 10);
            Debug.DrawRay(pos, direction * distanceCovered, Color.blue, 3f);

            bool hitTarget = false;

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

                    caster.audioSource.clip = abilitySound;
                    caster.audioSource.Play();

                    hitTarget = true;
                }
            }

            if (!hitTarget)
            {
                caster.audioSource.clip = abilitySound;
                caster.audioSource.Play();
            }


            caster.gameObject.layer = LayerMask.NameToLayer(oldLayer);
            yield return new WaitForSeconds(caster.pauseAfterAttack);
        }
        caster.getAttackTrigger().resetTrigger();
        caster.canMove = true;
        caster.attacking = false;
    }

}
