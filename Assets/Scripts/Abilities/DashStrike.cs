using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStrike : Ability {

    //Raycast Variables
    private float abilityRange = 3.1f;
    private float timeBeforeRay = 0.35f;

    public DashStrike()
    {
        //---- Setup ability stats ----
        //Setup looks
        icon = AbilitySprite.DASHSTRIKE;
        name = "Dash Strike";
        animation = "attack";
        causeOfDeath = "Not fast enough";

        //Setup cooldown
        cooldownTime = 5f;

        //Setup force
        abilityKnockback = 50;
        abilityKnockUp = 300;
        abilityVelocity = 1600;
    }

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
        //Start Cooldown
        cooldownStartTime = Time.time;

        string oldLayer = LayerMask.LayerToName(caster.gameObject.layer);
        caster.gameObject.layer = C.noCollisionLayer;

        caster.setInvulnerable(timeBeforeRay);

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
                    CMoveCombatable targetHit = r.transform.gameObject.GetComponentInParent<CMoveCombatable>();

                    if (objectHit.isInvuln() || targetHit.faction == caster.faction)
                        continue;

                    //Apply damage and knockback
                    objectHit.setAttacker(caster);
                    objectHit.loseHealth(abilityDamage);
                    objectHit.knockback(pos, abilityKnockback, objectHit.objectHeight); //Need to use original pos for knockback so the position of where you attacked from is the knockback
                    objectHit.knockUp(pos, abilityKnockback, abilityKnockUp, objectHit.objectHeight);

                    

                    if (targetHit != null)
                    {
                        targetHit.causeOfDeath = causeOfDeath;
                    }

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
