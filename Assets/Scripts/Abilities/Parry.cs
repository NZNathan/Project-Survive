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

    //Enemy Affects
    private float stunTime = 2f;
    private int abilityKnockback = 200;

    //Animation name in animator
    private string animation = "attack";

    //Cooldown Variables
    private float cooldownTime = 5f;
    private bool cooldown = false;

    //Ability Icon
    public Sprite icon;

    //Sound Variables
    private AudioClip parrySound;

    //Directional Variables
    private Vector2 pos;
    private Vector2 direction;


    public override void setTarget(CMoveCombatable caster, Vector2 pos)
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

    public override string getAnimation()
    {
        return animation;
    }

    public Sprite getIcon()
    {
        return icon;
    }

    public void parriedEnemy(CMoveCombatable enemy)
    {
        //Apply stun to the target
        enemy.applyStun(stunTime);

        enemy.knockback(pos, abilityKnockback, enemy.objectHeight);
    }

    protected override IEnumerator abilityActionSequence()
    {
        //Set up immunities
        bool stunImmunity = caster.stunImmunity;
        bool knockbackImmunity = caster.knockbackImmunity;
        caster.stunImmunity = true;
        caster.knockbackImmunity = true;

        caster.parrying = true;

		yield return new WaitForSeconds(1f);

        caster.parrying = false;
        caster.stunImmunity = stunImmunity;
        caster.knockbackImmunity = knockbackImmunity;
        caster.canMove = true;
        caster.attacking = false;
    }

}
