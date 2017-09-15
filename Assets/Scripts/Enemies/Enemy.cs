using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CMoveCombatable {

    //Revenge Target Variables
    public bool isBoss = false;
    protected bool revengeTarget = false;

    [Header("AI Variables")]
    public float aggroRange = 5f;
    public float attackRange = 0.65f;
    private Stack<AIState> state;

    [Header("Drop On Death Variables")]
    public DropableItem[] dropableItems;
    public int xpWorth;

    //Knockback collisions off time
    protected float collisionOffTime = 0.3f;

    //Movement Variables
    //[HideInInspector]
    public Transform target;
    private float stopDistance = .2f; //Distance to stop away from target by defualt

    // Use this for initialization
    new void Start()
    {
        base.Start();

        animator = GetComponentInChildren<Animator>();

        //Manage State
        state = new Stack<AIState>();
        state.Push(new IdleState(this));
    }

    #region States

    public AIState peekState()
    {
        if(state.Count > 0)
            return state.Peek();

        return null;
    }

    public void popState()
    {
        state.Pop();
    }

    public void pushState(AIState aiState)
    {
        state.Push(aiState);
    }

    #endregion

    public Vector3 getTargetPositon()
    {
        Vector3 targetOffset = new Vector3(-stopDistance * transform.localScale.x, 0, 0);
        
        return target.position + targetOffset;
    }

    public override Vector2 movement()
    {//Abstract out different move types?

        float movementSpeed = walkSpeed;

        if (target.transform.position.x < transform.position.x && transform.localScale.x != -1)
            faceLeft();
        else if (target.transform.position.x > transform.position.x && transform.localScale.x != 1)
            faceRight();

        Vector3 dir = getDirection(getTargetPositon(), target.gameObject.GetComponent<CHitable>().objectHeight);

        animator.SetFloat("movementSpeed", 2.5f);

        return (dir * movementSpeed);

    }

    public void attackTarget()
    {
        attack(basicAttack);
    }

    public override void loseHealth(int damage)
    {
        StartCoroutine("flash");
        currentHealth -= damage;

        //Stop showHealth so it doesn't remove the health bar off an earilier call
        if (!isBoss)
        {
            StopCoroutine("showHealth");
            StartCoroutine("showHealth");
        }

        healthBar.loseHealth((float)currentHealth / (float)maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            death();
        }
        else
        {
            setInvulnerable(invulnTime);
        }

        target = lastAttacker.transform;
    }

    protected override void death()
    {
        base.death();
        if(lastAttacker.tag == "Player")
            ((Player) lastAttacker).addXp(xpWorth);

        //Item drops
        foreach(DropableItem dropableItem in dropableItems)
        {
            dropableItem.dropItem(transform.position); 
        }
    }

    public override void knockback(Vector2 target, int force, float targetHeight)
    {
        rb2D.velocity = Vector2.zero;
        base.knockback(target, force, targetHeight);

        startCollisionsOff(collisionOffTime);
    }

    public void setRevengeTarget()
    {
        revengeTarget = true;
        StopCoroutine("onBecameVisible");
        StartCoroutine("onBecameVisible");
    }

    public void setBoss()
    {
        isBoss = true;
        StopCoroutine("onBecameVisible");
        StartCoroutine("onBecameVisible");
    }

    //If the enemy is a revenge target, or a boss, when they first appear on camera, zoom into them
    private IEnumerator onBecameVisible()
    {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        while (!renderer.isVisible)
        {
            Debug.Log("Revenegs");
            yield return new WaitForSeconds(0.3f);
        }

        UIManager.instance.newBossGUI(this);
        WorldManager.instance.zoomIn(transform);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        state.Peek().action();
    }

    //Just landed a hit on an enemy
    public override void attackHit()
    {
        //AI Reaaction here
    }
}
