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
    public float attackRayRange = 0.9f;
    protected IEnumerator attackAction;

    [HideInInspector]
    public bool attacking = false;

    //Basic Attack Variables
    [Tooltip("Distance traveled as attacking")]
    public float attackVelocity = 10f;
    public float pauseAfterAttack = 0.7f;
    public int attackDamage = 5;

    protected float attackPause = 0.25f; //"pause after starting attack before sends out ray

    [Tooltip("Knockback applied to target that is hit by attack")]
    public int attackKnockback = 500; //knockback

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

    protected void attack(Vector2 target, Vector2 dir, Ability ability)
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

    }

    protected IEnumerator rayCastAttack(Vector2 pos, Vector2 direction, float pause)
    {
        rb2D.AddForce(direction * attackVelocity);

        yield return new WaitForSeconds(pause);

        //Check if attack can go through
        if (!dead)
        {

            Vector2 newPos = new Vector2(transform.position.x, transform.position.y + objectHeight / 2);

            RaycastHit2D[] hitObject = Physics2D.RaycastAll(newPos, direction, attackRayRange, attackMask, -10, 10);
            Debug.DrawRay(newPos, direction * attackRayRange, Color.blue, 3f);

            bool hitTarget = false;

            //If the Raycast hits an object on the layer Enemy
            foreach (RaycastHit2D r in hitObject)
            {
                if (r && r.transform.gameObject != this.gameObject && attacking)
                {
                    //Hit attack
                    CHitable objectHit = r.transform.gameObject.GetComponentInParent<CHitable>();

                    //Apply damage and knockback
                    objectHit.setAttacker(this);
                    objectHit.loseHealth(attackDamage);
                    objectHit.knockback(pos, attackKnockback, objectHit.objectHeight); //Need to use original pos for knockback so the position of where you attacked from is the knockback

                    audioSource.clip = attackSound;
                    audioSource.Play();

                    hitTarget = true;
                    break;
                }
            }

            if (!hitTarget)
            {
                audioSource.clip = missSound;
                audioSource.Play();
            }

            yield return new WaitForSeconds(pauseAfterAttack);
        }
        canMove = true;
        attacking = false;
    }

}
