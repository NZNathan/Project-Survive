using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : Ability
{

    private CMoveCombatable caster;

    string abilityName = "Parry";

    private float abilityDuration = 0.7f; //Scale to player damage?

    //The force to be applied to the caster in the attack direction
    private float abilityVelocity = 150;
    private float stepAmount = 0.1f;

    //Animation name in animator
    private string animation = "parry";

    //Cooldown Variables
    private float cooldownTime = 5f;
    private bool cooldown = false;

    //Sound Variables
    private AudioClip parrySound;

    //Directional Variables
    private Vector2 pos;
    private Vector2 direction;


    public void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        this.caster = caster;
        this.pos = pos;

        //Direction is either walking direction of caster or facing direction if not moving
        if(caster.rb2D.velocity == Vector2.zero)
            direction = Vector2.right * caster.transform.localScale.x;
        else
            direction = caster.rb2D.velocity.normalized;
    }

    public bool canComboAttack()
    {
        return false;
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
        return abilityActionSequence();
    }

    public IEnumerator abilityActionSequence()
    {
        caster.setInvulnerable(abilityDuration);

        float duration = 0;

		yield return new WaitForSeconds(1f);

        caster.canMove = true;
        caster.attacking = false;
    }

}
