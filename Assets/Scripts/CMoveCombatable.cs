using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStat {STR, AGL, END }; 

public abstract class CMoveCombatable : CMoveable {

    [Header("Character Stats")]
    public int level = 1;
    public int strength = 1;
    public int agility = 1;
    public int endurance = 1;
    protected int endMod = 10;

    //Character Class
    protected CClass characterClass;

    //Character Traits
    protected Trait[] traits;
    private int traitsLimit = 6;

    //Weapon Variables
    protected bool weaponDrawn = false;

    //Raycast Variables
    public static LayerMask attackMask; //Layer that all attackable objects are on
    protected IEnumerator attackAction;

    [Header("Combat Variables")]
    public GameObject weapon;

    //Attack Variables
    [HideInInspector]
    public bool attacking = false;
    //[HideInInspector]
    public bool canCombo = false; //So player can get inputs for combos in while attacking
    protected AttackRayTrigger attackTrigger;

    //Basic Attack Variables
    public float pauseAfterAttack = 0.7f;
    public int attackDamage = 5;
    protected bool lastAttackHit = false;

    //Heavy Attack Variables
    protected float chargeTime = 0.5f;
    protected float startedHolding = float.MaxValue;

    //Stun Variables
    protected bool stunned = false;
    protected float stunTime = 0;

    //Immunities
    [Header("Immunities")]
    public bool stunImmunity = false;
    public bool knockbackImmunity = false;

    [Header("Audio Variables")]
    public AudioClip attackSound;
    public AudioClip missSound;


    protected new void Start()
    {
        base.Start();

        //Set up layers
        attackMask = LayerMask.GetMask("Hitable");
        originalLayer = gameObject.layer;

        //Get attackTrigger from child
        attackTrigger = GetComponentInChildren<AttackRayTrigger>();

        //Set up traits with a limit on it
        traits = new Trait[traitsLimit];

        //Ability Setup
        if(characterClass == null)
            chooseRandomClass();

        //Alter attack speed in the animator
        float atkspd = 1f;
        animator.SetFloat("attackSpeed", atkspd);

        //Set up stats
        maxHealth = endurance * endMod;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Method to be run when the Character takes a hit
    /// </summary>
    public abstract void attackHit();

    /// <summary>
    /// Takes a health bar and sets it as this characters health bar, assumes given health bar is already in position and matches the characters health
    /// </summary>
    /// <param name="newHealthBar"></param>
    public void setHealthbar(HealthBar newHealthBar)
    {
        healthBar = newHealthBar;
    }

    public void chooseRandomClass()
    {
        float r = Random.Range(0f, 1f);

        if(r > 0.5f)
            characterClass = new MeleeClass();
        else
            characterClass = new GunnerClass();
    }

    /// <summary>
    /// Adds the amount to the specified stat
    /// </summary>
    public void addToStat(CharacterStat stat, int amount)
    {
        if (stat == CharacterStat.STR)
            strength += amount;
        else if (stat == CharacterStat.AGL)
            agility += amount;
        else if (stat == CharacterStat.END)
            endurance += amount;
    }

    public Trait[] getTraits()
    {
        return traits;
    }

    protected void drawWeapon()
    {
        weaponDrawn = !weaponDrawn;
        weapon.SetActive(weaponDrawn);
    }

    public void startCollisionsOff(float collisionlessTime)
    {
        StartCoroutine("collisionsOff", collisionlessTime);
    }

    IEnumerator collisionsOff(float collisionlessTime)
    {
        gameObject.layer = noCollisionLayer;
        yield return new WaitForSeconds(collisionlessTime);
        gameObject.layer = originalLayer;
    }

    public override void knockUp(Vector2 target, int knockbackForce, int knockupForce, float targetHeight)
    {
        if (attackAction != null)
        {
            StopCoroutine(attackAction);
            attacking = false;
            canMove = true;
            attackTrigger.resetTrigger();
        }

        falling = true;

        rb2D.AddForce(Vector2.up * knockupForce);

        animator.SetTrigger("inAir");

        Vector3 dir = getDirection(target, targetHeight) * -1;
        float startPos = (transform.position + dir * knockbackForce / 1000).y; //What if hit vertically

        if(fallingCo != null)
            StopCoroutine(fallingCo);

        fallingCo = StartCoroutine("fallDown", startPos);
    }

    IEnumerator fallDown(float floorY)
    {
        gameObject.layer = noCollisionLayer;
        stunned = true;

        yield return new WaitForSeconds(.1f);

        float fallVelocity = 35;
        //rb2D.velocity = Vector2.zero; //Reset velocity so it isn't taken into account for knockup
        while (transform.position.y > floorY + 0.03f && transform.position.y > WorldManager.lowerBoundary)
        {
            rb2D.AddForce(Vector2.down * fallVelocity);
            yield return new WaitForFixedUpdate();
        }

        rb2D.velocity = new Vector2(0, 0);
        animator.SetTrigger("hitFloor");

        yield return new WaitForSeconds(.6f);

        animator.SetTrigger("getUp");
        gameObject.layer = originalLayer;

        //yield return new WaitForSeconds(.2f); //Wait till up

        falling = false;
        stunned = false; //Add in option to roll up off ground
    }

    public virtual bool attack(Ability ability)
    {
        //Check if ability is not on cooldown
        if (!ability.onCooldown() && !falling & !stunned)
        {
            if(attackAction != null)
                StopCoroutine(attackAction);

            //Stop movement and call attack animataion
            canMove = false;
            attacking = true;
            canCombo = false;
            animator.SetFloat("movementSpeed", 0);
            animator.ResetTrigger("stopAttack");

            animator.SetTrigger(ability.getAnimation());

            //Get position of player
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + objectHeight / 2);

            //Set as a variable so it can be referenced and stopped else where and to get a unique action that matches the ability
            ability.setTarget(this, pos);
            attackAction = ability.getAction();

            //UIManager.instance.newTextMessage(this.gameObject, WorldManager.instance.banterGen.getAttackYell());

            StartCoroutine(attackAction);

            if(!falling)
                rb2D.velocity = Vector2.zero; //Resets so running doesn't stack but reseting velocity so you can avoid knockback

            return true; //Attack successful
        }

        return false; //Attack failed
    }

    public override void applyStun(float stunTime)
    {
        if (!stunImmunity)
        {
            this.stunTime = stunTime;

            //Stop enemy if they are currently attacking and stun them 
            if (attackAction != null)
                StopCoroutine(attackAction);


            attacking = false;
            animator.SetTrigger("stopAttack");
            
            StopCoroutine("stun");
            if (!falling)
                StartCoroutine("stun");
        }
    }

    public IEnumerator stun()
    {
        stunned = true;
        animator.SetFloat("movementSpeed", 0);

        yield return new WaitForSeconds(stunTime);

        attackTrigger.resetTrigger();
        stunned = false;
    }

    public int getBaseDamage()
    {
        return (int) (strength * 1.2);
    }

    public bool isStunned()
    {
        return stunned;
    }

    public AttackRayTrigger getAttackTrigger()
    {
        return attackTrigger;
    }

    public void setComboAnimation(bool value)
    {
        animator.SetBool("combo", value);
    }

    public CClass getClass()
    {
        return characterClass;
    }

}
