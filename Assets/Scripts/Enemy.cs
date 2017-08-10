using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CMoveCombatable {

    //Revenge Target Variables
    private bool revengeTarget = false;

    [Header("AI Variables")]
    public float aggroRange = 5f;
    public float attackRange = 0.9f;
    AIState state;

    //Stun Variables
    public float stunTime = 0.5f;
    private bool stunned = false;

    //Knockback collisions off time
    private float collisionOffTime = 0.3f;

    //Movement Variables
    private Transform target;
    private Player player;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        
        animator = GetComponentInChildren<Animator>();
        player = Player.instance;
        target = player.transform;
    }

    protected override Vector2 movement()
    {
        float movementSpeed = walkSpeed;

        Vector3 dir = getDirection(target.position, target.gameObject.GetComponent<CHitable>().objectHeight);

        if (dir.x < 0 && transform.localScale.x != -1 * facingFront)
            faceLeft();
        else if (dir.x > 0 && transform.localScale.x != 1 * facingFront)
            faceRight();

        animator.SetFloat("movementSpeed", 2.5f);
        //rb2D.MovePosition(transform.position + dir * movementSpeed * Time.deltaTime);
        return(dir * movementSpeed);

    }

    void attackTarget()
    {
        Vector2 direction = getDirection(target.position, target.gameObject.GetComponent<CHitable>().objectHeight);

        attack(target.position, direction, abilities[0]);
    }

    public override void loseHealth(int damage)
    {
        base.loseHealth(damage);

        //Stop enemy if they are currently attacking and stun them 
        if(attackAction != null)
            StopCoroutine(attackAction);

        StopCoroutine("stun");
        attacking = false;
        animator.SetTrigger("stopAttack");

        StartCoroutine("stun");
    }

    public override void knockback(Vector2 target, int force, float targetHeight)
    {
        base.knockback(target, force, targetHeight);

        StartCoroutine("collisionsOff");
    }

    IEnumerator collisionsOff()
    {
        gameObject.layer = LayerMask.NameToLayer("NoCharacterCollisions");
        yield return new WaitForSeconds(collisionOffTime);
        gameObject.layer = LayerMask.NameToLayer("Hitable");
    }

    public IEnumerator stun()
    {
        stunned = true;
        animator.SetFloat("movementSpeed", 0);

        yield return new WaitForSeconds(stunTime);

        stunned = false;
    }

    public void setRevengeTarget()
    {
        revengeTarget = true;
        StartCoroutine("onBecameVisible");
    }

    private IEnumerator onBecameVisible()
    {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        while (!renderer.isVisible)
        {
            Debug.Log("Revenegs");
            yield return new WaitForSeconds(0.3f);
        }

        WorldManager.instance.zoomIn(transform);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if(player == null)
            player = Player.instance;

        if (stunned || dead || player == null || player.isDead())
            return;

        //Abstract out a get target method for futre enemies?
        if ((player.transform.position - transform.position).magnitude < aggroRange)
            target = player.transform;
        else
            target = null;

        if (target != null && (player.transform.position - transform.position).magnitude > attackRange)
            rb2D.AddForce(movement());
        else
            animator.SetFloat("movementSpeed", 0);

        if (target != null && (player.transform.position - transform.position).magnitude < attackRange && !attacking)
            attackTarget();
    }

    public override void attackHit()
    {
        //AI Reaaction here
    }
}
