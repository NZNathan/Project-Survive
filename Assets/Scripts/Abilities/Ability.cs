using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability {

    protected CMoveCombatable caster;

    //Ability Variables

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
    protected string name;

    //Cooldown Variables
    protected float cooldownTime = 5f;
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

        abilityDamage = getDamage(caster.strength);

        //GDebug
        abilitySound = caster.attackSound;
    }

    ///
    /// The formula to determine the damage of this ability
    ///
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

    public abstract string getAnimation();

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
