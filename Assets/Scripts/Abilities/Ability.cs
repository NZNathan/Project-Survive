using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability {

    protected CMoveCombatable caster;

    //Damage to be applied
    protected int abilityDamage; 

    //Amount of force applied to the caster
    protected int abilityVelocity = 0;

    //Amount of force applied to the target
    protected int abilityKnockback = 0;

    //Amount of force applied in an upward direction on the target
    protected int abilityKnockUp = 0;

    //Stunned time applied to the target
    protected float stunTime = 0f;

    //Ability Names
    public string name;
    protected string animation;
    protected string causeOfDeath;

    //Cooldown Variables
    protected float cooldownStartTime = 0f;
    protected float cooldownTime = 5f;
    protected float newCooldownTime = 5f;
    private float agilityMod = 0.1f;
    protected bool cooldown = false;

    //Icon 
    public AbilitySprite icon;

    //Sound Variables
    protected AudioClip abilitySound;

    //Directional Variables
    protected Vector2 pos;
    protected Vector2 direction;

    public virtual void setTarget(CMoveCombatable caster, Vector2 pos)
    {
        this.caster = caster;
        this.pos = pos;

        //Get direction based on caster facing direction
        direction = new Vector2(caster.transform.localScale.x, 0);

        //Reset combo on caster
        caster.canCombo = false;

        newCooldownTime = cooldownTime - (caster.agility * agilityMod);

        abilityDamage = getDamage(caster.strength);
    }

    /// <summary>
    /// The formula to determine the damage of this ability
    /// </summary>
    /// <returns></returns>
    protected virtual int getDamage(int casterStrength)
    {
        return casterStrength * 2;
    }

    public virtual bool canComboAttack()
    {
        return false;
    }

    public void setCooldown(bool cooldown)
    {
        this.cooldown = cooldown;
    }

    public bool onCooldown()
    {
        return cooldownStartTime + getCooldown() > Time.time || cooldown;
    }

    public float getCooldown()
    {
        if(newCooldownTime > 0)
            return newCooldownTime;
        else
            return 0.01f;
    }

    public float getAbilityVelocity()
    {
        return abilityVelocity;
    }

    public virtual string getAnimation()
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

    protected abstract IEnumerator abilityActionSequence();
    
}
