using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CMoveCombatable {

    //Components
    private new SpriteRenderer renderer;

    //Revenge Target Variables
    public bool isBoss = false;
    public bool revengeTarget = false;
    public bool hasBeenSeen = false;
    private int tick = 0;

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
        renderer = GetComponentInChildren<SpriteRenderer>();

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

    public int[] getStats()
    {
        int[] stats = new int[4];
        stats[0] = level;
        stats[1] = strength;
        stats[2] = agility;
        stats[3] = endurance;

        return stats;
    }

    public void setupRevengeTarget(RevengeTarget self)
    {
        //Set Details
        firstName = self.firstName;
        lastName = self.lastName;
        faction = self.faction;

        //Set Stats
        level = self.level;
        strength = self.strength;
        agility = self.agility;
        endurance = self.endurance;

        traits = self.traits;

        //Set Sprites
        setSpriteSet(self.sprites);

        //Set animator
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = self.animatorController;
    }

    public Vector3 getTargetPositon()
    {
        Vector3 targetOffset = new Vector3(-stopDistance * transform.localScale.x, 0, 0);
        
        return target.position + targetOffset;
    }

    public override Vector2 movement()
    {//Abstract out different move types?

        float movementSpeed = sprintSpeed;

        if (target.transform.position.x < transform.position.x && transform.localScale.x != -1)
            faceLeft();
        else if (target.transform.position.x > transform.position.x && transform.localScale.x != 1)
            faceRight();

        Vector3 dir = getDirection(getTargetPositon(), target.gameObject.GetComponent<CHitable>().objectHeight);

        if (movementSpeed == walkSpeed)
            animator.SetFloat("movementSpeed", 2.5f);
        else
            animator.SetFloat("movementSpeed", 5f);

        return (dir * movementSpeed);

    }

    public void attackTarget()
    {
        attack(characterClass.basicAttack);
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

        //If idle and attacked, move toward new attack target
        popState();
        pushState(new MoveState(this));

        target = lastAttacker.transform;
    }

    protected override void death()
    {
        base.death();

        //If the player killed this enemy, award xp to player
        if(lastAttacker.tag == "Player")
            ((Player) lastAttacker).addXp(xpWorth);

        //If enemy was a boss, then turn off boss GUI
        if(isBoss)
            UIManager.instance.closeBossGUI();

        //Item drops
        foreach(DropableItem dropableItem in dropableItems)
        {
            dropableItem.dropItem(transform.position, transform.parent); 
        }
    }

    public override void knockback(Vector2 target, int force, float targetHeight)
    {
        rb2D.velocity = Vector2.zero;
        base.knockback(target, force, targetHeight);

        startCollisionsOff(collisionOffTime);
    }

    //If the enemy is a revenge target, or a boss, when they first appear on camera, zoom into them
    private void onBecameVisible()
    {
        //If not a boos or revenge target, then stop calling this method
        if (!isBoss && !revengeTarget)
        {
            hasBeenSeen = true;
            return;
        }

        if (renderer.isVisible)
        {
            UIManager.instance.newBossGUI(this);
            WorldManager.instance.zoomIn(transform);

            hasBeenSeen = true;
            Debug.Log("Bos has been seen");
        }

        tick = 0;
    }

    private new void Update()
    {
        if (!WorldManager.isPaused)
        {
            base.Update();

            if (!hasBeenSeen && tick >= 4)
                onBecameVisible();

            tick++;
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (!WorldManager.isPaused)
            state.Peek().action();
    }

    //Just landed a hit on an enemy
    public override void attackHit()
    {
        //AI Reaaction here
    }
}
