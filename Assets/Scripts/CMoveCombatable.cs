using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CMoveCombatable : CMoveable {

    [Header("Combat Variables")]
    public GameObject weapon;

    //Weapon Variables
    protected bool weaponDrawn = false;

    protected Ability[] abilities;

    //Raycast Variables
    public static LayerMask attackMask; //Layer that all attackable objects are on
    protected IEnumerator attackAction;

    [HideInInspector]
    public bool attacking = false;

    //Basic Attack Variables
    public float pauseAfterAttack = 0.7f;
    public int attackDamage = 5;

    [Header("Audio Variables")]
    public AudioClip attackSound;
    public AudioClip missSound;


    protected new void Start()
    {
        base.Start();
        attackMask = LayerMask.GetMask("Hitable");

        //TEMP
        abilities = new Ability[2];
        abilities[0] = new BasicAttack();
        abilities[1] = new DashStrike();
    }

    protected void drawWeapon()
    {
        weaponDrawn = !weaponDrawn;
        weapon.SetActive(weaponDrawn);
    }

    protected bool attack(Vector2 target, Vector2 dir, Ability ability)
    {
        //Check if ability is not on cooldown
        if (!ability.onCooldown())
        {
            //Stop movement and call attack animataion
            canMove = false;
            attacking = true;
            animator.SetFloat("movementSpeed", 0);
            animator.ResetTrigger("stopAttack");
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

            StartCoroutine(attackAction);

            rb2D.velocity = Vector2.zero; //Resets so running doesn't stack but reseting velocity so you can avoid knockback

            rb2D.AddForce(dir * ability.getAbilityVelocity()); //DUPLICATE ADD FORCE IN RAYCAST??

            return true; //Attack successful
        }

        return false; //Attack failed
    }

}
