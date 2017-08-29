using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CMoveCombatable : CMoveable {

    [Header("Combat Variables")]
    public GameObject weapon;

    //Character Stats
    protected int level = 1;
    protected int strength;
    protected int agility;
    protected int endurance;

    //Weapon Variables
    protected bool weaponDrawn = false;

    //Ability Variables
    protected Ability basicAttack;
    protected Ability heavyAttack;
    protected Ability[] abilities;

    //Raycast Variables
    public static LayerMask attackMask; //Layer that all attackable objects are on
    protected IEnumerator attackAction;

    //Attack Variables
    public bool attacking = false;
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
    public bool stunned = false;

    [Header("Audio Variables")]
    public AudioClip attackSound;
    public AudioClip missSound;


    protected new void Start()
    {
        base.Start();
        attackMask = LayerMask.GetMask("Hitable");
        originalLayer = gameObject.layer;

        attackTrigger = GetComponentInChildren<AttackRayTrigger>();

        //Ability Setup
        basicAttack = new BasicAttack();
        heavyAttack = new HeavyAttack();

        //TEMP
        abilities = new Ability[2];
        abilities[0] = new DashStrike();
        abilities[1] = new DodgeRoll();
    }

    public abstract void attackHit();

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
        gameObject.layer = LayerMask.NameToLayer("NoCharacterCollisions");
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
        gameObject.layer = LayerMask.NameToLayer("NoCharacterCollisions");
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

    protected bool attack(Vector2 target, Vector2 dir, Ability ability)
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

            //Replace ability action with combo ability, if it can be a combo attack
            ability = ability.getComboAttack();

            animator.SetTrigger(ability.getAnimation());

            //Get position of player
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + objectHeight / 2);

            //Face attack direction
            if (target.x < pos.x)
                faceLeft();
            else
                faceRight();

            //Set as a variable so it can be referenced and stopped else where and to get a unique action that matches the ability
            ability.setTarget(this, pos, dir);
            attackAction = ability.getAction();

            //UIManager.instance.newTextMessage(this.gameObject, WorldManager.instance.banterGen.getAttackYell());

            StartCoroutine(attackAction);

            if(!falling)
                rb2D.velocity = Vector2.zero; //Resets so running doesn't stack but reseting velocity so you can avoid knockback

            return true; //Attack successful
        }

        return false; //Attack failed
    }

    public AttackRayTrigger getAttackTrigger()
    {
        return attackTrigger;
    }

    public void setComboAnimation(bool value)
    {
        animator.SetBool("combo", value);
    }

}
