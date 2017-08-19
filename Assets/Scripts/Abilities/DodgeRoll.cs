using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRoll : Ability
{

    private CMoveCombatable caster;

    string abilityName = "Dodge Roll";

    private float abilityDuration = 0.7f; //Scale to player damage?

    //The force to be applied to the caster in the attack direction
    private float abilityVelocity = 150;
    private float stepAmount = 0.1f;

    //Animation name in animator
    private string animation = "dodgeroll";

    //Cooldown Variables
    private float cooldownTime = 5f;
    private bool cooldown = false;

    //Sound Variables
    private AudioClip rollSound;

    //Directional Variables
    private Vector2 pos;
    private Vector2 direction;


    public void setTarget(CMoveCombatable caster, Vector2 pos, Vector2 direction)
    {
        this.caster = caster;
        this.pos = pos;
        this.direction = direction;

    }

    public Ability getComboAttack()
    {
        return this;
    }

    public void setCooldown(bool cooldown)
    {
        this.cooldown = cooldown;
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
        caster.rb2D.AddForce(direction * abilityVelocity);
        caster.setInvulnerable(abilityDuration);
        caster.startCollisionsOff(abilityDuration);

        float duration = 0;

        while (duration < abilityDuration)
        {
            caster.rb2D.AddForce(direction * abilityVelocity);

            duration += stepAmount;
            yield return new WaitForSeconds(stepAmount);
        }

        caster.canMove = true;
        caster.attacking = false;
    }

}
