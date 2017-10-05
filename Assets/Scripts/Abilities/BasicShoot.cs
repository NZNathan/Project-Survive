using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShoot : Ability
{

    private CMoveCombatable caster;

    string abilityName = "Basic Shot";

    private int abilityDamage; //Scale to player damage?

    //The force to be applied to the caster in the attack direction
    private float abilityVelocity = -10;

    //Animation name in animator
    private string animation = "attack";

    //Knockback applied to target that is hit by attack
    private int abilityKnockback = 0;
    //Stunned time applied to the target
    private float stunTime = 0.1f;

    //Cooldown of the ability
    private float cooldownTime = 3.5f;
    private bool cooldown = false;

    //Ability Icon
    public Sprite icon;


    //Combo Variables
    private Ability comboAttack = new BasicAttackCombo();
    private float lastAttack = -1f;
    private float comboChainTime = 0.35f;

    //Directional Variables
    private Vector2 pos;
    private Vector2 direction;

    //Initialise here
    public override void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        this.caster = caster;
        this.pos = pos;

        //Get direction based on caster facing direction
        direction = new Vector2(caster.transform.localScale.x, 0);

        caster.canCombo = false;
        abilityDamage = caster.attackDamage;
    }

    public bool canComboAttack()
    {
        return lastAttack + comboChainTime > Time.time || comboAttack.canComboAttack();
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

    protected override IEnumerator abilityActionSequence()
    {

        //Wait until the attack frame in the animation has been reached
        while (!caster.getAttackTrigger().hasAttackTriggered())
            yield return null;

        caster.getAttackTrigger().resetTrigger();

        //After shot nudge caster back a tad
        caster.rb2D.AddForce(direction * abilityVelocity / Time.timeScale);

        //Instantiate Bullet
        Bullet b = ObjectPool.instance.getBullet();

        //Setup and turn on bullet
        b.gameObject.SetActive(true);
        b.Setup(caster, abilityDamage, stunTime, direction);

        caster.canCombo = false;
        caster.canMove = true;
        caster.attacking = false;
    }

}
