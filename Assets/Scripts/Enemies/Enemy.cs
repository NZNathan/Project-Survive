using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CMoveCombatable {

    //Revenge Target Variables
    protected bool revengeTarget = false;

    [Header("AI Variables")]
    public float aggroRange = 5f;
    public float attackRange = 0.65f;
    private Stack<AIState> state;

    //Stun Variables
    public float stunTime = 0.5f;

    [Header("Drop On Death Variables")]
    public Item[] dropableItems;
    public int xpWorth;

    //Knockback collisions off time
    protected float collisionOffTime = 0.3f;

    //Movement Variables
    //[HideInInspector]
    public Transform target;
    protected Player player;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        
        animator = GetComponentInChildren<Animator>();
        player = Player.instance;
        target = player.transform;

        //Manage State
        state = new Stack<AIState>();
        state.Push(new IdleState(this));
    }

    #region States

    public void popState()
    {
        state.Pop();
    }

    public void pushState(AIState aiState)
    {
        state.Push(aiState);
    }

    #endregion

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

    public void attackTarget()
    {
        Vector2 direction = getDirection(target.position, target.gameObject.GetComponent<CHitable>().objectHeight);

        attack(target.position, direction, basicAttack);
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
        attackTrigger.resetTrigger();

        if (!falling)
            StartCoroutine("stun");
    }

    protected override void death()
    {
        base.death();
        if(lastAttacker.tag == "Player")
            ((Player) lastAttacker).addXp(xpWorth);

        //TODO: Spawn item drops
        int itemType = UnityEngine.Random.Range(0, dropableItems.Length);

        float drop = UnityEngine.Random.Range(0f, 1f);

        if (dropableItems[itemType].dropRate > drop)
            Instantiate(dropableItems[itemType], transform.position, Quaternion.identity);
    }

    public override void knockback(Vector2 target, int force, float targetHeight)
    {
        rb2D.velocity = Vector2.zero;
        base.knockback(target, force, targetHeight);

        startCollisionsOff(collisionOffTime);
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

    //If the enemy is a revenge target, when they first appear on camera, zoom into them
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
        state.Peek().action();
        return;
        if(player == null)
            player = Player.instance;

        if (stunned || dead || player == null || player.isDead() || falling)
            return;

        //If the enemy has a target, and its out of attack range move closer to it, else make sure the move animation is not playing
        if (target != null && (player.transform.position - transform.position).magnitude > attackRange)
            rb2D.AddForce(movement());
        else
            animator.SetFloat("movementSpeed", 0);

        //If the enemy has a target and is within attacking range, and not already attacking, attack the target
        if (target != null && (player.transform.position - transform.position).magnitude < attackRange && !attacking)
            attackTarget();
    }

    //Just landed a hit on an enemy
    public override void attackHit()
    {
        //AI Reaaction here
    }
}
